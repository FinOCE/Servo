namespace Servo.Utilities.Processed.Packet;

public class Ball
{
    public Physics Physics;

    public Ball(rlbot.flat.BallInfo ballInfo)
    {
        Physics = new(ballInfo.Physics!.Value);
    }
}