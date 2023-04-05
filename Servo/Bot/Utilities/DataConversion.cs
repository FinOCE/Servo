namespace Servo.Utilities;

public static class DataConversion
{
    public static Vector3 ToVector3(this rlbot.flat.Vector3 vector)
    {
        return new(vector.X, vector.Y, vector.Z);
    }

    public static Vector2 ToVector2(this rlbot.flat.Vector3 vector)
    {
        return new(vector.X, vector.Y);
    }
}