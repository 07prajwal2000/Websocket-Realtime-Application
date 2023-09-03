namespace Messaging.Shared;

public enum MessageType
{
    Ping = 1,
    Register,
    SendMessage,
    ListConnections,
    Broadcast,
}