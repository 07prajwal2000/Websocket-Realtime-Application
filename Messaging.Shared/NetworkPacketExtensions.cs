using Messaging.Shared.Packets;

namespace Messaging.Shared;

public static class NetworkPacketExtensions
{
    public static MessageType GetMessageType(this ReadPacket packet, bool resetAfterRead = false)
    {
        packet.ResetPosition();
        var type = (MessageType) packet.ReadInt();
        if (resetAfterRead) packet.ResetPosition();
        return type;
    }
    
    public static void SetMessageType(this WritePacket packet, MessageType messageType)
    {
        packet.WriteInt((int)messageType);
    }
}
