namespace Servo;

public class ServoBot : RLBotDotNet.Bot
{
    public ServoBot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex) { }

    public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
    {
        Packet packet = new(gameTickPacket);

        Vector3 ballLocation = packet.Ball.Physics.Location;
        Vector3 carLocation = packet.Players[Index].Physics.Location;
        Orientation carRotation = packet.Players[Index].Physics.Rotation;

        Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);

        float steer;
        if (ballRelativeLocation.Y > 0)
            steer = 1;
        else
            steer = -1;

        // Examples of rendering in the game
        Renderer.DrawString3D("Ball", Color.Black, ballLocation, 3, 3);
        Renderer.DrawString3D(steer > 0 ? "Right" : "Left", Color.Aqua, carLocation, 3, 3);
        Renderer.DrawLine3D(Color.Red, carLocation, ballLocation);

        return new Controller
        {
            Throttle = 1,
            Steer = steer
        };
    }
    internal new FieldInfo GetFieldInfo() => new(base.GetFieldInfo());

    internal new BallPrediction GetBallPrediction() => new(base.GetBallPrediction());
}