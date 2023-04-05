// Get file name from console
string? fileName = null;
do
{
    Console.Write("Enter replay file name: ");
    string? input = Console.ReadLine();
    
    if (input is not null && input != "")
        fileName = input;
}
while (fileName is null);

// Fetch replay file and handle if it doesn't exist
Replay? replay = Parser.GetReplay(fileName);

if (replay is null)
{
    Console.WriteLine($"Could not find Replay/{fileName}.replay");
    Console.Read();
    return;
}

// Parse replay file
Parser.SaveJson(fileName, replay);

// Await key press to close console
Console.Read();