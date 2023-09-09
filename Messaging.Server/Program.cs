using Messaging.Server;

var server = new Server(5555);

server.Start();

Console.WriteLine("press key to stop");
Console.ReadKey();

server.Stop();