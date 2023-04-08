namespace Servo.Utilities.Training;

public abstract class Drill
{
    public GameState GameState { get; init; }

    protected abstract CarState[] CarState { get; set; }

    protected abstract BallState BallState { get; set; }

    protected abstract GameInfoState GameInfoState { get; set; }

    public abstract int Duration { get; init; }

    public Drill(int index)
    {
        GameState = new();
        
        foreach (CarState state in CarState)
            GameState.SetCarState(index, state);
        GameState.BallState = BallState;
        GameState.GameInfoState = GameInfoState;
    }

    public abstract float CalculateFitness(Packet packet);
}