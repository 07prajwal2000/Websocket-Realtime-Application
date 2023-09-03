using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared.MessageTypes;

public class ListConnectionMessageFromServer : MessageBase
{
    public string Connections { get; private set; }

    public ListConnectionMessageFromServer(string connections) : base(MessageType.ListConnections)
    {
        this.Connections = connections;
    }
    public override ArraySegment<byte> ToArraySegment()
    {
        using var packet = NetworkPacket.CreateWritePacket(Connections.Length + 12);
        packet.SetMessageType(MessageType);
        packet.WriteString(Connections);
        return packet.ToBytesSegment(out _);
    }

    public static ListConnectionMessageFromServer FromNetworkPacket(NetworkPacket packet)
    {
        var conn = packet.ReadString();
        return new ListConnectionMessageFromServer(conn);
    }
}
