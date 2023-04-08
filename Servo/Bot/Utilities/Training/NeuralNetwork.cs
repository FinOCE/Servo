namespace Servo.Utilities.Training;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private readonly int[] Layers;
    
    private float[][] Neurons = null!;
    
    private float[][] Biases = null!;
    
    private float[][][] Weights = null!;
    
    private readonly int[] Activations;

    public float Fitness = 0;

    public float LearningRate = 0.01f;
    
    public float Cost = 0;

    public NeuralNetwork(int[] layers, string[] layerActivations)
    {
        Layers = new int[layers.Length];
        layers.CopyTo(Layers, 0);
        
        Activations = new int[layers.Length - 1];
        for (int i = 0; i < layers.Length - 1; i++)
        {
            string action = layerActivations[i];
            Activations[i] = action switch
            {
                "sigmoid" => 0,
                "tanh" => 1,
                "relu" => 2,
                "leakyrelu" => 3,
                _ => 2
            };
        }
        
        InitNeurons();
        InitBiases();
        InitWeights();
    }


    private void InitNeurons()
    {
        List<float[]> neuronsList = new();
        
        for (int i = 0; i < Layers.Length; i++)
            neuronsList.Add(new float[Layers[i]]);
        
        Neurons = neuronsList.ToArray();
    }

    private void InitBiases()
    {
        List<float[]> biasList = new();
        
        for (int i = 1; i < Layers.Length; i++)
        {
            float[] bias = new float[Layers[i]];
            
            for (int j = 0; j < Layers[i]; j++)
            {
                Random random = new();
                bias[j] = (float)random.NextDouble() - 0.5f;
            }
            
            biasList.Add(bias);
        }
        
        Biases = biasList.ToArray();
    }

    private void InitWeights()
    {
        List<float[][]> weightsList = new();
        
        for (int i = 1; i < Layers.Length; i++)
        {
            List<float[]> layerWeightsList = new();
            int neuronsInPreviousLayer = Layers[i - 1];
            
            for (int j = 0; j < Layers[i]; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    Random random = new();
                    neuronWeights[k] = (float)random.NextDouble() - 0.5f;
                }
                
                layerWeightsList.Add(neuronWeights);
            }
            
            weightsList.Add(layerWeightsList.ToArray());
        }
        
        Weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
            Neurons[0][i] = inputs[i];
        
        for (int i = 1; i < Layers.Length; i++)
        {
            int layer = i - 1;
            
            for (int j = 0; j < Layers[i]; j++)
            {
                float value = 0f;
                
                for (int k = 0; k < Layers[i - 1]; k++)
                    value += Weights[i - 1][j][k] * Neurons[i - 1][k];
                
                Neurons[i][j] = Activate(value + Biases[i - 1][j], layer);
            }
        }
        
        return Neurons[Layers.Length - 1];
    }
    
    public float Activate(float value, int layer)
    {
        return Activations[layer] switch
        {
            0 => Sigmoid(value),
            1 => Tanh(value),
            2 => Relu(value),
            3 => Leakyrelu(value),
            _ => Relu(value)
        };
    }
    
    public float ActivateDer(float value, int layer)
    {
        return Activations[layer] switch
        {
            0 => SigmoidDer(value),
            1 => TanhDer(value),
            2 => ReluDer(value),
            3 => LeakyreluDer(value),
            _ => ReluDer(value)
        };
    }

    public static float Sigmoid(float x)
    {
        float k = (float)Math.Exp(x);
        return k / (1.0f + k);
    }
    
    public static float Tanh(float x)
    {
        return (float)Math.Tanh(x);
    }
    
    public static float Relu(float x)
    {
        return (0 >= x) ? 0 : x;
    }
    
    public static float Leakyrelu(float x)
    {
        return (0 >= x) ? 0.01f * x : x;
    }
    
    public static float SigmoidDer(float x)
    {
        return x * (1 - x);
    }
    public static float TanhDer(float x)
    {
        return 1 - (x * x);
    }
    public static float ReluDer(float x)
    {
        return (0 >= x) ? 0 : 1;
    }
    public static float LeakyreluDer(float x)
    {
        return (0 >= x) ? 0.01f : 1;
    }

    public void BackPropagate(float[] inputs, float[] expected)
    {
        float[] output = FeedForward(inputs);

        Cost = 0;

        for (int i = 0; i < output.Length; i++)
            Cost += (float)Math.Pow(output[i] - expected[i], 2);
        
        Cost /= 2;

        float[][] gamma;

        List<float[]> gammaList = new();
        
        for (int i = 0; i < Layers.Length; i++)
            gammaList.Add(new float[Layers[i]]);
        
        gamma = gammaList.ToArray();

        int layer = Layers.Length - 2;
        
        for (int i = 0; i < output.Length; i++)
            gamma[Layers.Length - 1][i] = (output[i] - expected[i]) * ActivateDer(output[i], layer);
        
        for (int i = 0; i < Layers[^1]; i++)
        {
            Biases[Layers.Length - 2][i] -= gamma[Layers.Length - 1][i] * LearningRate;
            
            for (int j = 0; j < Layers[^2]; j++)
                Weights[Layers.Length - 2][i][j] -= gamma[Layers.Length - 1][i] * Neurons[Layers.Length - 2][j] * LearningRate;
        }

        for (int i = Layers.Length - 2; i > 0; i--)
        {
            layer = i - 1;
            
            for (int j = 0; j < Layers[i]; j++)
            {
                gamma[i][j] = 0;
                
                for (int k = 0; k < gamma[i + 1].Length; k++)
                    gamma[i][j] += gamma[i + 1][k] * Weights[i][k][j];
                
                gamma[i][j] *= ActivateDer(Neurons[i][j], layer);
            }
            for (int j = 0; j < Layers[i]; j++)
            {
                Biases[i - 1][j] -= gamma[i][j] * LearningRate;
                for (int k = 0; k < Layers[i - 1]; k++)
                {
                    Weights[i - 1][j][k] -= gamma[i][j] * Neurons[i - 1][k] * LearningRate;
                }
            }
        }
    }

    public void Mutate(int high, float val)
    {
        for (int i = 0; i < Biases.Length; i++)
            for (int j = 0; j < Biases[i].Length; j++)
            {
                Random random = new();
                Biases[i][j] = ((float)random.NextDouble() * high <= 5) ? Biases[i][j] += (((float)random.NextDouble() * val * 2) - val) : Biases[i][j];
            }

        for (int i = 0; i < Weights.Length; i++)
            for (int j = 0; j < Weights[i].Length; j++)
                for (int k = 0; k < Weights[i][j].Length; k++)
                {
                    Random random = new();
                    Weights[i][j][k] = ((float)random.NextDouble() * high <= 2) ? Weights[i][j][k] += (((float)random.NextDouble() * val * 2) - val) : Weights[i][j][k];
                }
    }

    public int CompareTo(NeuralNetwork? other)
    {
        if (other == null)
            return 1;

        if (Fitness > other.Fitness)
            return 1;
        
        else if (Fitness < other.Fitness)
            return -1;
        
        else
            return 0;
    }

    public NeuralNetwork Copy(NeuralNetwork nn)
    {
        for (int i = 0; i < Biases.Length; i++)
            for (int j = 0; j < Biases[i].Length; j++)
                nn.Biases[i][j] = Biases[i][j];
        
        for (int i = 0; i < Weights.Length; i++)
            for (int j = 0; j < Weights[i].Length; j++)
                for (int k = 0; k < Weights[i][j].Length; k++)
                    nn.Weights[i][j][k] = Weights[i][j][k];
        
        return nn;
    }

    public void Load(string path)
    {
        TextReader tr = new StreamReader(path);
        int NumberOfLines = (int)new FileInfo(path).Length;
        string[] ListLines = new string[NumberOfLines];
        
        int index = 1;
        
        for (int i = 1; i < NumberOfLines; i++)
            ListLines[i] = tr.ReadLine()!;
        
        tr.Close();
        
        if (new FileInfo(path).Length > 0)
        {
            for (int i = 0; i < Biases.Length; i++)
                for (int j = 0; j < Biases[i].Length; j++)
                {
                    Biases[i][j] = float.Parse(ListLines[index]);
                    index++;
                }

            for (int i = 0; i < Weights.Length; i++)
                for (int j = 0; j < Weights[i].Length; j++)
                    for (int k = 0; k < Weights[i][j].Length; k++)
                    {
                        Weights[i][j][k] = float.Parse(ListLines[index]); ;
                        index++;
                    }
        }
    }
    
    public void Save(string path)
    {
        File.Create(path).Close();
        StreamWriter writer = new(path, true);

        for (int i = 0; i < Biases.Length; i++)
            for (int j = 0; j < Biases[i].Length; j++)
                writer.WriteLine(Biases[i][j]);

        for (int i = 0; i < Weights.Length; i++)
            for (int j = 0; j < Weights[i].Length; j++)
                for (int k = 0; k < Weights[i][j].Length; k++)
                    writer.WriteLine(Weights[i][j][k]);
        
        writer.Close();
    }
}