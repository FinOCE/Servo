namespace Servo.Utilities.Processed.BallPrediction;

/// <summary>
/// Processed version of the <see cref="rlbot.flat.BallPrediction"/> that uses sane data structures.
/// </summary>
public struct BallPrediction
{
    public PredictionSlice[] Slices;

    public int Length => Slices.Length;

    public BallPrediction(rlbot.flat.BallPrediction ballPrediction)
    {
        Slices = new PredictionSlice[ballPrediction.SlicesLength];
        for (int i = 0; i < ballPrediction.SlicesLength; i++)
            Slices[i] = new(ballPrediction.Slices(i)!.Value);
    }
}