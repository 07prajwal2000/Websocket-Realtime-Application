namespace Messaging.Shared.MessageTypes;

public class RegisterMessage : MessageBase
{
    public readonly Guid Id;

    public string Username { get; private set; }

    public RegisterMessage(string username, Guid id) : base(MessageType.Register)
    {
        Username = username;
        Id = id;
    }
    
    public override ArraySegment<byte> ToArraySegment()
    {
        using var packet = NetworkPacket.CreateWritePacket(100);
        packet.WriteInt((int) MessageType);
        packet.WriteString(Username);
        packet.WriteString(Id.ToString());
        return packet.ToBytesSegment(out _);
    }

    public static RegisterMessage FromNetworkPacket(NetworkPacket packet)
    {
        var username = packet.ReadString();
        var id = Guid.Parse(packet.ReadString());
        return new RegisterMessage(username, id);
    }
}
