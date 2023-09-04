using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Messaging.Web;

public delegate void MessageReceived(string username, ArraySegment<byte> data);
public delegate void ClientConnected(string username, WebSocket webSocket);
public delegate void ClientDisconnected(string username, WebSocket webSocket);

public class MessagingServer
{
    private ConcurrentDictionary<string, WebSocket> connections = new();
    public static event MessageReceived OnMessageReceived;
    public static event ClientConnected OnClientConnected;
    public static event ClientDisconnected OnClientDisconnected;

    public MessagingServer()
    {
        
    }

    public async Task ReceiveConnection(WebSocket webSocket)
    {
        var username = GenerateUsername();
        connections.TryAdd(username, webSocket);
        ReceiveLoop(username, webSocket);
        OnClientConnected?.Invoke(username, webSocket);
    }

    private string GenerateUsername(int size = 8)
    {
        Span<char> username = stackalloc char[size];

        for (int i = 0; i < size; i++)
        {
            var rand = Random.Shared.Next(0, 26);
            var randChar = Convert.ToChar(97 + rand);
            username[i] = randChar;
        }
        var uname = new string(username);
        return uname;
    }

    private async Task ReceiveLoop(string username, WebSocket webSocket)
    {
        ArraySegment<byte> buffer = new byte[1024];
        var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            OnMessageReceived?.Invoke(username, buffer[..result.Count]);
            result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        connections.Remove(username, out _);
        OnClientDisconnected.Invoke(username, webSocket);
    }
}
