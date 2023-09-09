using Messaging.Shared;
using Messaging.Shared.Messages;
using Messaging.Shared.Messages.Server;
using Messaging.Shared.Packets;
using System.Net.Sockets;

namespace Messaging.Client;

internal class Client
{
    private TcpClient client = new TcpClient();
    private readonly CancellationTokenSource cancellationToken = new();

    public void Start()
    {
        client.Connect("localhost", 5555);
        Task.Run(Receive, cancellationToken.Token);
    }

    public void Stop()
    {
        client.Close();
        cancellationToken.Cancel();
    }

    private async Task Receive()
    {
        ArraySegment<byte> buffer = new byte[1024];
        while (!cancellationToken.IsCancellationRequested)
        {
            var totalRead = await client.Client.ReceiveAsync(buffer, cancellationToken.Token);
            if (totalRead == 0)
            {
                break;
            }
            var packet = new ReadPacket(buffer[..totalRead]);
            await ReceiveLogic(packet);
        }
        Console.WriteLine("Server closed");
    }

    private async Task ReceiveLogic(ReadPacket packet)
    {
        var type = packet.GetMessageType();
        switch (type)
        {
            case MessageType.Ping:
                ServerPingMessage(packet);
                break;
            case MessageType.SuccessAuthentication:
                SuccessAuthMessage(packet);
                break;
            case MessageType.FailedAuthentication:
                FailedAuthMessage(packet);
                break;
            case MessageType.Register:
                break;
            case MessageType.SendMessage:
                break;
            case MessageType.ListConnections:
                break;
            case MessageType.Broadcast:
                break;
            case MessageType.Authentication:
                break;
        }
    }

    private void SuccessAuthMessage(ReadPacket packet)
    {
        var message = AuthenticationSuccessMessage.FromPacket(packet);
        Console.WriteLine(message.Message);
    }
    private void FailedAuthMessage(ReadPacket packet)
    {
        var message = AuthenticationFailedMessage.FromPacket(packet);
        Console.WriteLine(message.Message);
    }
    private void ServerPingMessage(ReadPacket packet)
    {
        var msg = PingMessage.FromPacket(packet);
        Console.WriteLine("Latency: {0}ms", msg.Latency);
    }

    ~Client()
    {
        cancellationToken.Cancel();
    }
}
