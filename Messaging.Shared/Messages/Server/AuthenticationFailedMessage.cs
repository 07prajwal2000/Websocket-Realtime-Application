using Messaging.Shared.Packets;

namespace Messaging.Shared.Messages.Server;

public class AuthenticationFailedMessage
{
    private static readonly MessageType MessageType = MessageType.FailedAuthentication;
    public readonly string Message;

    public AuthenticationFailedMessage(string message)
    {
        Message = message;
    }

    public static byte[] GetBytes(string message = "[ERROR] : AUTHENTICATION FAILED")
    {
        var packet = new WritePacket(4 + 4 + message.Length);
        packet.SetMessageType(MessageType);
        packet.WriteString(message);
        return packet.ToBytes();
    }

    public static AuthenticationFailedMessage FromPacket(ReadPacket packet)
    {
        var msg = packet.ReadString();
        return new(msg);
    }
}
