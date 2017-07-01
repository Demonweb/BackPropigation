using System;
public class Pattern
{
    private double[] inputs;
    private double[] outputs;
    public Pattern(string value, int inputDims, int outputDims)
    {
        char[] delimiterChars = { ',' };
        string[] line = value.Split(delimiterChars);
        if (line.Length != inputDims + outputDims)
            throw new Exception("Input does not match network configuration");
        inputs = new double[inputDims];
        for (int i = 0; i < inputDims; i++)
        {
            inputs[i] = double.Parse(line[i]);
        }
        outputs = new double[outputDims];
        for (int i = 0; i < outputDims; i++)
        {
            outputs[i] = double.Parse(line[i + inputDims]);
        }
    }
    public int MaxOutput
    {
        get
        {
            int item = -1;
            double max = double.MinValue;
            for (int i = 0; i < Outputs.Length; i++)
            {
                if (Outputs[i] > max)
                {
                    max = Outputs[i];
                    item = i;
                }
            }
            return item;
        }
    }
    public double[] Inputs
    {
        get { return inputs; }
    }
    public double[] Outputs
    {
        get { return outputs; }
    }
}