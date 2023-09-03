namespace Messaging.Shared.MessageTypes;

public class SendMessageToUserMessageTypeFromClient : MessageBase
{
    public string User { get; private set; }
    public string Message { get; private set; }

    public SendMessageToUserMessageTypeFromClient(string user, string message) : base(MessageType.SendMessage)
    {
        User = user;
        Message = message;
    }

    public override ArraySegment<byte> ToArraySegment()
    {
        using var packet = NetworkPacket.CreateWritePacket(16 + User.Length + Message.Length);
        packet.SetMessageType(MessageType);
        packet.WriteString(User);
        packet.WriteString(Message);
        return packet.ToBytesSegment(out _);
    }

    public static SendMessageToUserMessageTypeFromClient FromNetworkPacket(NetworkPacket packet)
    {
        var user = packet.ReadString();
        var msg = packet.ReadString();

        return new SendMessageToUserMessageTypeFromClient(user, msg);
    }
}
