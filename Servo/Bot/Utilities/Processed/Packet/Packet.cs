namespace Servo.Utilities.Processed.Packet;

/// <summary>
/// Processed version of the <see cref="rlbot.flat.GameTickPacket"/> that uses sane data structures.
/// </summary>
public class Packet
{
    public Player[] Players;
    
    public BoostPadState[] BoostPadStates;

    public Ball Ball;

    public GameInfo GameInfo;

    public TeamInfo[] Teams;

    public Packet(rlbot.flat.GameTickPacket packet)
    {
        Players = new Player[packet.PlayersLength];
        for (int i = 0; i < packet.PlayersLength; i++)
            Players[i] = new(packet.Players(i)!.Value);

        BoostPadStates = new BoostPadState[packet.BoostPadStatesLength];
        for (int i = 0; i < packet.BoostPadStatesLength; i++)
            BoostPadStates[i] = new(packet.BoostPadStates(i)!.Value);

        Ball = new(packet.Ball!.Value);
        GameInfo = new(packet.GameInfo!.Value);

        Teams = new TeamInfo[packet.TeamsLength];
        for (int i = 0; i < packet.TeamsLength; i++)
            Teams[i] = new(packet.Teams(i)!.Value);
    }
}