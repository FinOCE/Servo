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

    public static void Test(Replay replay)
    {
        // Filter frames
        List<Frame> frames = GetAnalysisFrames(replay, 0.5);

        // Create dictionary containing class definitions
        Dictionary<int, string> classes = new();
        replay.ClassIndexes.ForEach(v => classes.Add(v.Index, v.Class));        

        // Refactor actors
        List<Actor> actors = new();
        foreach (var actor in frames[100].ActorStates)
        {
            actors.Add(new()
            {
                Id = actor.Id,
                Class = actor.ClassId != null ? classes[(int)actor.ClassId] : null,
                Object = actor.TypeId != null ? replay.Objects[(int)actor.TypeId] : null,
                Name = actor.NameId != null ? replay.Names[(int)actor.NameId] : null,
                Position = actor.Position,
                Rotation = actor.Rotation
            });
        };

        // Output test result
        string output = JsonConvert.SerializeObject(actors, Formatting.Indented);
        using StreamWriter outfile = File.AppendText($"../../../Replays/Output/{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}.json");
        outfile.Write(output);
    }

    /// <summary>
    /// Get frames separated by a given number of milliseconds.
    /// </summary>
    /// <param name="replay">The replay to get frames from</param>
    /// <param name="expectedDelta">The number of milliseconds between collected frames</param>
    /// <returns>The filtered frames</returns>
    public static List<Frame> GetAnalysisFrames(Replay replay, double expectedDelta = 0)
    {
        // Return all frames if no delta is provided
        if (expectedDelta <= 0)
            return replay.Frames;

        // Get frames with filtered delta and actors
        List<Frame> analysisFrames = new();

        double delta = int.MaxValue;
        foreach (Frame frame in replay.Frames)
        {
            delta += frame.Delta;

            if (delta >= expectedDelta)
            {
                // Ignore frames that have no actors
                bool hasActors = false;

                frame.ActorStates.ForEach(actor => {
                    var props = actor.Properties.Values.Where(v => v.Data is RigidBodyState).FirstOrDefault();
                    if (props != null)
                    {
                        var rbs = props.Data as RigidBodyState;

                        if (!rbs!.Sleeping)
                            hasActors = true;
                    }
                });

                if (!hasActors)
                    continue;

                // Reset delta and add frame to analysis frames list
                delta = 0;
                analysisFrames.Add(frame);
            }
        }

        return analysisFrames;
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