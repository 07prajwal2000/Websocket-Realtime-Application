using Messaging.Server;
using WatsonWebsocket;

var ip = Environment.GetEnvironmentVariable("ip");
var port = Environment.GetEnvironmentVariable("port");
var server = new WatsonWsServer(ip, int.Parse(port));
var callbacks = new WebSocketCallbacks(server);
server.MessageReceived += callbacks.OnMessageReceived;
server.ClientConnected += callbacks.ClientConnected;
server.ClientDisconnected += callbacks.ClientDisConnected;
Console.WriteLine("Server has started on port: " + port);
server.Start();
Console.WriteLine("Type 'exit' to close server");
Console.WriteLine("Type 'help' to list commands");
var command = Console.ReadLine();

while (!string.Equals(command, "exit", StringComparison.CurrentCultureIgnoreCase))
{
    command = Console.ReadLine();
}

server.Stop();
