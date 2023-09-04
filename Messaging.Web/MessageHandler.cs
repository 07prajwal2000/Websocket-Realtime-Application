using System.Net.WebSockets;

namespace Messaging.Web;

public class MessageHandler
{
    private readonly ILogger<HttpMessageHandler> logger;

    public MessageHandler(ILogger<HttpMessageHandler> logger)
    {
        this.logger = logger;
    }

    public void Initialize()
    {
        MessagingServer.OnMessageReceived += OnMessageReceived;
        MessagingServer.OnClientDisconnected += OnClientDisconnected;
        MessagingServer.OnClientConnected += OnClientConnected;
    }

    private void OnClientDisconnected(string username, WebSocket webSocket)
    {
        logger.LogInformation($"client connedted: {username}");
    }

    private void OnMessageReceived(string username, ArraySegment<byte> data)
    {
        
    }

    private void OnClientConnected(string username, WebSocket webSocket)
    {
        logger.LogInformation($"client connedted: {username}");
    }
}
