using Messaging.Server;
using WatsonWebsocket;

const int PORT = 5553;

var server = new WatsonWsServer(port: PORT);
var callbacks = new WebSocketCallbacks(server);
server.MessageReceived += callbacks.OnMessageReceived;
server.ClientConnected += callbacks.ClientConnected;
server.ClientDisconnected += callbacks.ClientDisConnected;

server.Start();
Console.WriteLine("Type 'exit' to close server");
Console.WriteLine("Type 'help' to list commands");
var command = Console.ReadLine();

while (!string.Equals(command, "exit", StringComparison.CurrentCultureIgnoreCase))
{
    command = Console.ReadLine();
}

server.Stop();