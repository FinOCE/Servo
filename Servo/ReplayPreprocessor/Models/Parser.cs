namespace ReplayPreprocessor;

public class Parser
{
    public static Replay? GetReplay(string fileName)
    {
        try
        {
            return Replay.Deserialize($"../../../Replays/{fileName}.replay");
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// TEMPORARY: Eventually the JSON needs to be converted into a CSV to train the model on.
    /// </summary>
    public static void SaveJson(string fileName, Replay replay)
    {
        string parsedReplay = JsonConvert.SerializeObject(replay.Frames[0], Formatting.Indented);
        using StreamWriter outfile = File.AppendText($"../../../Replays/{fileName}.json");
        outfile.Write(parsedReplay);
    }
}