int port;
try
{
    port = int.Parse(args[0]);
}
catch (Exception)
{
    Console.WriteLine("Could not get port from arguments to C# bot!");
    throw;
}

BotManager<ServoBot> botManager = new(0);
botManager.Start(port);