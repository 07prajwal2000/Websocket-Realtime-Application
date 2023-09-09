using Messaging.Shared.Packets;
namespace Messaging.Shared.Messages.Client;

public class AuthenticationMessage
{
    private static readonly MessageType MessageType = MessageType.Authentication;
    public readonly string userId;

    private AuthenticationMessage(string userId)
    {
        this.userId = userId;
    }

    public static byte[] GetBytes(in string userId)
    {
        var packet = new WritePacket(4 + 4 + userId.Length);
        packet.SetMessageType(MessageType);
        packet.WriteString(userId);
        return packet.ToBytes();
    }

    public static AuthenticationMessage FromPacket(ReadPacket packet)
    {
        var id = packet.ReadString();
        return new AuthenticationMessage(id);
    }

    public bool IsValid()
    {
        // run validation logic
        return true;
    }
}
