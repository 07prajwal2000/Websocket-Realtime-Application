using Messaging.Shared.Packets;

namespace Messaging.Shared.Messages.Server;

public class AuthenticationSuccessMessage
{
    private static readonly MessageType MessageType = MessageType.SuccessAuthentication;
    public readonly string Message;

    public AuthenticationSuccessMessage(string message)
    {
        Message = message;
    }

    public static byte[] GetBytes(string message = "[SUCCESS] : AUTHENTICATION SUCCESS")
    {
        var packet = new WritePacket(4 + 4 + message.Length);
        packet.SetMessageType(MessageType);
        packet.WriteString(message);
        return packet.ToBytes();
    }

    public static AuthenticationSuccessMessage FromPacket(ReadPacket packet)
    {
        var msg = packet.ReadString();
        return new(msg);
    }
}
