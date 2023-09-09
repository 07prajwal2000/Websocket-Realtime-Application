using Messaging.Shared.Packets;

namespace Messaging.Shared.Messages;

public class PingMessage
{
    public MessageType MessageType { get; private set; } = MessageType.Ping;
    private long serverTimestamp;
    public long Latency { get; private set; }

    public PingMessage(long timestamp)
    {
        serverTimestamp = timestamp;
        CalculateLatency();
    }

    public static byte[] GetBytes()
    {
        var packet = new WritePacket(12);
        packet.SetMessageType(MessageType.Ping);
        packet.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        return packet.ToBytes();
    }

    public static PingMessage FromPacket(ReadPacket packet)
    {
        var timestamp = packet.ReadLong();
        var msg = new PingMessage(timestamp);
        return msg;
    }

    private void CalculateLatency()
    {
        var currentStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Latency = currentStamp - serverTimestamp;
    }
}
