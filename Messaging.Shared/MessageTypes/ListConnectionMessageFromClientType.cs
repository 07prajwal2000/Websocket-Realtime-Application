namespace Messaging.Shared.MessageTypes;

public class ListConnectionMessageFromClientType : MessageBase
{
    public ListConnectionMessageFromClientType() : base(MessageType.ListConnections) { }

    public override ArraySegment<byte> ToArraySegment()
    {
        using var packet = NetworkPacket.CreateWritePacket(4);
        packet.SetMessageType(MessageType);
        return packet.ToBytesSegment(out _);
    }

    public static MessageType FromNetworkPacket(NetworkPacket packet)
    {
        return packet.GetMessageType();
    }
}
