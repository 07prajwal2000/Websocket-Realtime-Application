
using Messaging.Shared;
using Messaging.Shared.MessageTypes;
using System.Text;
using WatsonWebsocket;

namespace Messaging.Server;
public class WebSocketCallbacks
{
    private readonly WatsonWsServer server;
    private readonly List<string> connections = new();
    private readonly Dictionary<string, Guid> users = new();

    public WebSocketCallbacks(WatsonWsServer server)
    {
        this.server = server;
    }

    public void OnMessageReceived(object? s, MessageReceivedEventArgs e)
    {
        using var packet = NetworkPacket.CreateReadPacket(e.Data.Array!);
        var msgType = packet.GetMessageType();
        if (msgType == MessageType.ListConnections)
        {
            SendConnectionList(e.Client.Guid);
        }
        else if (msgType == MessageType.SendMessage)
        {
            var fromUser = (string) e.Client.Metadata;
            SendMessageToUser(packet, fromUser);
        }
    }

    public async void ClientConnected(object? s, ConnectionEventArgs e)
    {
        var uname = GenerateUsername();
        var msg = new RegisterMessage(uname, e.Client.Guid);
        var arr = msg.ToArraySegment();
        await server.SendAsync(e.Client.Guid, arr);
        Console.WriteLine("client connected: " + uname);
        connections.Add(uname);
        users.Add(uname, e.Client.Guid);
        e.Client.Metadata = uname;
    }

    private string GenerateUsername(int size = 5)
    {
        Span<char> username = stackalloc char[5];

        for (int i = 0; i < size; i++)
        {
            var rand = Random.Shared.Next(0, 26);
            var randChar = Convert.ToChar(97 + rand);
            username[i] = randChar;
        }
        var uname = new string(username);
        return uname;
    }

    public void ClientDisConnected(object? s, DisconnectionEventArgs e)
    {
        var uname = (string) e.Client.Metadata;
        connections.Remove(uname);
        users.Remove(uname);
    }

    private async void SendMessageToUser(NetworkPacket packet, string fromUser)
    {
        var user = packet.ReadString();
        var msg = packet.ReadString();

        if (!connections.Contains(user)) return;
        var msgPacket = new SendMessageFromServer(msg, fromUser);
        var succ = await server.SendAsync(users[user], msgPacket.ToArraySegment());
        Console.WriteLine("message sent to " + user + (succ ? " successfully" : " failed"));
    }

    private async void SendConnectionList(Guid id)
    {
        var sb = new StringBuilder();
        foreach (var client in connections)
        {
            sb.Append(client + ",");
        }
        sb.Remove(sb.Length - 1, 1);
        var conn = sb.ToString();
        var msg = new ListConnectionMessageFromServer(conn);
        var arr = msg.ToArraySegment();
        await server.SendAsync(id, arr);
    }
}