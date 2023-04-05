namespace Servo.Utilities.Processed.BallPrediction;

public struct PredictionSlice
{
    public Physics Physics;
    
    public float GameSeconds;

    public PredictionSlice(rlbot.flat.PredictionSlice predictionSlice)
    {
        Physics = new(predictionSlice.Physics!.Value);
        GameSeconds = predictionSlice.GameSeconds;
    }
}