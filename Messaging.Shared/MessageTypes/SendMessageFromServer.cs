namespace Messaging.Shared.MessageTypes;

public class SendMessageFromServer : MessageBase
{
    public SendMessageFromServer(string message, string fromUser) : base(MessageType.SendMessage)
    {
        Message = message;
        FromUser = fromUser;
    }

    public string Message { get; private set; }
    public string FromUser { get; private set; }

    public override ArraySegment<byte> ToArraySegment()
    {
        using var packet = NetworkPacket.CreateWritePacket(16 + Message.Length + FromUser.Length);

        packet.SetMessageType(MessageType);
        packet.WriteString(FromUser);
        packet.WriteString(Message);

        return packet.ToBytesSegment(out _);
    }
}
