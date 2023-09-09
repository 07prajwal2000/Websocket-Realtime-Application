using Messaging.Shared;
using Messaging.Shared.Messages.Client;
using Messaging.Shared.Messages.Server;
using Messaging.Shared.Packets;
using System.Net.Sockets;

namespace Messaging.Server;

internal class Server
{
    private TcpListener server;
    private CancellationTokenSource cancellationToken;
    private readonly TimeSpan authenticationTimeout = TimeSpan.FromSeconds(10);
    
    public Server(int port)
    {
        server = TcpListener.Create(port);
    }
    public Server(int port, TimeSpan authTimeout)
    {
        server = TcpListener.Create(port);
        authenticationTimeout = authTimeout;
    }

    public void Start()
    {
        server.Start();
        cancellationToken = new CancellationTokenSource();
        Task.Run(AcceptClientsLoop, cancellationToken.Token);
    }

    public void Stop()
    {
        cancellationToken.Cancel();
        server.Stop();
    }

    private async Task AcceptClientsLoop()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var client = await server.AcceptTcpClientAsync();
            _ = Task.Run(async () => await AuthenticateClient(client), cancellationToken.Token);
            //Console.WriteLine("Client connected");
            //await SendPingMessage(client);
        }
    }

    private async Task AuthenticateClient(TcpClient client)
    {
        Console.WriteLine("Client connected");
        ArraySegment<byte> buffer = new byte[64];
        var authenticated = false;
        var authCancelToken = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            Console.WriteLine("\tstarted auth");
            await Task.Delay(authenticationTimeout);
            authCancelToken.Cancel();
            Console.WriteLine("\tended auth: " + authCancelToken.IsCancellationRequested);
            
            Console.WriteLine("Client auth failed");
            await client.Client.SendAsync(AuthenticationFailedMessage.GetBytes());
            await CloseClient(client);
        }, authCancelToken.Token);

        while (!cancellationToken.IsCancellationRequested || !authCancelToken.IsCancellationRequested)
        {
            var totalRead = await client.Client.ReceiveAsync(buffer, authCancelToken.Token);
            var packet = new ReadPacket(buffer[..totalRead]);
            var message = AuthenticationMessage.FromPacket(packet);
            if (message.IsValid())
            {
                authCancelToken.Cancel();
                authenticated = true;
                break;
            }
        }

        if (authenticated)
        {
            _ = Task.Run(async () => await ReceiveLoop(client), cancellationToken.Token);
        }
    }

    private async Task ReceiveLoop(TcpClient client)
    {
        await SendPingMessage(client);
        ArraySegment<byte> buffer = new byte[1024];
        var totalRead = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            totalRead = await client.Client.ReceiveAsync(buffer, cancellationToken.Token);
            if (totalRead == 0)
            {
                break;
            }

            var sliced = buffer[..totalRead];
            var packet = new ReadPacket(sliced);
        }
        Console.WriteLine("Client closed");
    }

    private async Task SendPingMessage(TcpClient client)
    {
        var packet = new WritePacket(4);
        packet.SetMessageType(MessageType.Ping);
        await client.Client.SendAsync(packet.ToBytesTrimmed());
    }

    private async Task CloseClient(TcpClient client)
    {
        await Task.Delay(3 * 1000);
        client.Close();
        client.Dispose();
    }

    ~Server()
    {
        cancellationToken.Cancel();
    }
}
