using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared;

public static class NetworkPacketExtensions
{
    public static MessageType GetMessageType(this NetworkPacket packet, bool resetAfterRead = false)
    {
        packet.ResetPosition();
        var type = (MessageType) packet.ReadInt();

        if (resetAfterRead) packet.ResetPosition();

        return type;
    }
    
    public static void SetMessageType(this NetworkPacket packet, MessageType messageType)
    {
        packet.WriteInt((int)messageType);
    }
}
