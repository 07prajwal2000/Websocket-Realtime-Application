using Messaging.Shared;
using Messaging.Shared.MessageTypes;
using WatsonWebsocket;


using var client = new WatsonWsClient("localhost", 8090);
client.MessageReceived += OnMessageReceived;

client.Start();

Console.WriteLine("enter exit to close or enter other to send message. Connected: " + client.Connected);
var message = Console.ReadLine();
while (!string.Equals(message, "exit", StringComparison.CurrentCultureIgnoreCase))
{
    message = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(message)) continue;

    await client.SendAsync(message);

    if (message?.StartsWith("list-clients", StringComparison.CurrentCultureIgnoreCase) ?? false)
    {
        SendListConnectionsMessage();
    }
    if (message?.StartsWith("send", StringComparison.CurrentCultureIgnoreCase) ?? false)
    {
        var msgs = message.Count(x => x == ' ');
        if (msgs < 2) continue;
        message = message.Remove(0, 5);
        var spIdx = message.IndexOf(" ");

        var user = message.Substring(0, spIdx + 1).Trim();
        var msg = message.Substring(spIdx + 1).Trim();

        SendMessageToUser(user, message);
    }
}

client.Dispose();

async void SendListConnectionsMessage()
{
    var msg = new ListConnectionMessageFromClientType();
    await client.SendAsync(msg.ToArraySegment());
}

async void SendMessageToUser(string user, string message)
{
    var msg = new SendMessageToUserMessageTypeFromClient(user, message);
    await client.SendAsync(msg.ToArraySegment());
}

void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
{
    using var packet = NetworkPacket.CreateReadPacket(e.Data.Array!);
    var type = packet.GetMessageType();
    Console.WriteLine("MESSAGE RECEIVED");
    if (type == MessageType.Register)
    {
        var message = RegisterMessage.FromNetworkPacket(packet);
        Console.WriteLine("Your username is: " + message.Username + "\nID: " + message.Id);
    }
    if (type == MessageType.ListConnections)
    {
        var message = ListConnectionMessageFromServer.FromNetworkPacket(packet);
        Console.WriteLine($"\nConnetions: {message.Connections}\nTotal Connections: {message.Connections.Count(c => c == ',') + 1}\n");
    }
    if (type == MessageType.SendMessage)
    {
        var fromUser = packet.ReadString();
        var message = packet.ReadString();
        Console.WriteLine("Message received: " + message + " from " + fromUser);
    }
}