namespace Servo.Utilities.Processed.FieldInfo;

/// <summary>
/// Processed version of the <see cref="rlbot.flat.FieldInfo"/> that uses sane data structures.
/// </summary>
public struct FieldInfo
{
    public GoalInfo[] Goals;
    
    public BoostPad[] BoostPads;

    public FieldInfo(rlbot.flat.FieldInfo fieldInfo)
    {
        Goals = new GoalInfo[fieldInfo.GoalsLength];
        BoostPads = new BoostPad[fieldInfo.BoostPadsLength];

        for (int i = 0; i < fieldInfo.GoalsLength; i++)
            Goals[i] = new(fieldInfo.Goals(i)!.Value);

        for (int i = 0; i < fieldInfo.BoostPadsLength; i++)
            BoostPads[i] = new(fieldInfo.BoostPads(i)!.Value);
    }
}