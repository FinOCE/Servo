// Get file name from console
//string? fileName = null;
//do
//{
//    Console.Write("Enter replay file name: ");
//    string? input = Console.ReadLine();

//    if (input is not null && input != "")
//        fileName = input;
//}
//while (fileName is null);
string fileName = "55542b99-566a-4f05-85e0-5b4be2c3934f";

// Fetch replay file and handle if it doesn't exist
Replay? replay = Parser.GetReplay(fileName);

if (replay is null)
{
    Console.WriteLine($"Could not find the replay. Press enter to exit...");
    Console.Read();
    return;
}

// Test the replay
Parser.Test(replay);

// Await key press to close console
Console.WriteLine("Task completed. Press enter to exit...");
Console.Read();