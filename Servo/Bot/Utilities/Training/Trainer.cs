namespace Servo.Utilities.Training;

public class Trainer : ServoBot
{
    private readonly List<NeuralNetwork> Nets = new();

    private int FitnessCooldown = 0;

    private Drill? Drill;

    public Trainer(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
    {
        // Start a drill here - RunDrill(someDrill);
    }
    
    public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
    {
        if (Drill != null)
        {
            if (FitnessCooldown-- <= 0)
            {
                Net.Fitness = Drill.CalculateFitness(new(gameTickPacket));
                FitnessCooldown = 60;
            }
        }
            
        
        return base.GetOutput(gameTickPacket);
    }

    public async Task RunDrill(Drill drill, int iterations)
    {
        for (int i = 0; i < iterations; i++)
            foreach (NeuralNetwork net in Nets)
            {
                // Setup drill
                SetGameState(drill.GameState);
                Net = net;

                // Run drill
                Drill = drill;
                await Task.Delay(drill.Duration);
                Drill = null;
            }
    }

    public void Iterate()
    {
        Nets.Sort();

        for (int i = 0; i < Nets.Count / 2; i++)
        {
            Nets[i] = Nets[i + Nets.Count / 2].Copy(DefaultNet());
            Nets[i].Mutate((int)(1 / 0.01f), 0.5f);
        }

        foreach (NeuralNetwork net in Nets)
            net.Fitness = 0;
    }
}