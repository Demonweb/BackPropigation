using System;
using System.Collections.Generic;
using System.IO;
public class Network 
{
    private int[] dimensions;
    private List<Pattern> patterns;
    private List<Layer> layer;

    public Network(int[] dimensions, string file)
    {
        this.dimensions = dimensions;
        Initialise();
        LoadPatterns(file);
    }
    public void Initialise()
    {
        layer = new List<Layer>();
        layer.Clear();
        layer.Add(new Layer(dimensions[0]));
        for (int i = 1; i < dimensions.Length; i++)
        {
            layer.Add(new Layer(dimensions[i],layer[i-1], new Random()));
        }
    }
    private void LoadPatterns(string file)
    {
        patterns = new List<Pattern>();
        StreamReader reader = File.OpenText(file);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            patterns.Add(new Pattern(line, Inputs.Count, Outputs.Count));
        }
        reader.Close();
    }
    public double Train()
    {
        double error = 0;
        foreach (Pattern pattern in patterns)
        {
            Activate(pattern);
            for (int i = 0; i < Outputs.Count; i++)
            {
                double delta = pattern.Outputs[i] - Outputs[i].Output;
                Outputs[i].CollectError(delta);
                error += Math.Pow(delta, 2);
            }
            AdjustWeights();
        }
        return error;
    }
    private void Activate(Pattern pattern)
    {
        for (int i = 0; i < Inputs.Count; i++)
        {
            Inputs[i].Output = pattern.Inputs[i];
        }
        for (int i = 1; i < layer.Count; i++)
        {
            foreach (Neuron neuron in layer[i])
            {
                neuron.Activate();
            }
        }
    }
    private void AdjustWeights()
    {
        int layerCount =layer.Count - 1;
        for (int i = layerCount; i > 0; i--)
        {
            foreach (Neuron neuron in layer[i])
            {
                neuron.AdjustWeights();
            }
        }
    }
    public List<double[]> HyperPlanes()
    {
        List<double[]> lines = new List<double[]>();
        foreach (Neuron n in Outputs)
        {
            lines.Add(n.HyperPlane);
        }
        return lines;
    }
    public List<float[]> Points2D()
    {
        int penultimate = layer.Count-2;
        if (layer[penultimate].Count != 2)
        {
            throw new Exception("Penultimate layer must be 2D for graphing.") ;
        }
        List<float[]> points = new List<float[]>();
        for (int i = 0; i < patterns.Count; i++)
        {
            Activate(patterns[i]);
            float[] point = new float[3];
            point[0] = (float)layer[penultimate][0].Output;
            point[1] = (float)layer[penultimate][1].Output;
            if (Outputs.Count > 1)
            {
                point[2] = patterns[i].MaxOutput;
            }
            else
            {
                point[2] = (patterns[i].Outputs[0] >= 0.5) ? 1 : 0;
            }
            points.Add(point);
        }
        return points;
    }
    private Layer Inputs
    {
        get { return layer[0]; }
    }
    private Layer Outputs
    {       
        get { return layer[layer.Count - 1]; }
    }
}