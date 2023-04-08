namespace Servo;

public class ServoBot : RLBotDotNet.Bot
{
    protected Func<NeuralNetwork> DefaultNet = () =>
        new(new int[] { 1, 3, 2 }, new string[] { "sigmoid", "sigmoid" });

    protected NeuralNetwork Net;
    
    public ServoBot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
    {
        Net = DefaultNet();
    }

    public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
    {
        Packet packet = new(gameTickPacket);

        Vector3 ballLocation = packet.Ball.Physics.Location;
        Vector3 carLocation = packet.Players[Index].Physics.Location;
        Orientation carRotation = packet.Players[Index].Physics.Rotation;

        Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);
        float steer = (float)(Math.Atan2(ballRelativeLocation.Y, ballRelativeLocation.X) / Math.PI);

        float[] output = Net.FeedForward(new float[] { steer });

        return new()
        {
            Throttle = 1,
            Steer = output[0]
        };
    }
    internal new FieldInfo GetFieldInfo() => new(base.GetFieldInfo());

    internal new BallPrediction GetBallPrediction() => new(base.GetBallPrediction());
}