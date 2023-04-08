int port = int.Parse(args[0]);
BotManager<Trainer> botManager = new(0);
botManager.Start(port);