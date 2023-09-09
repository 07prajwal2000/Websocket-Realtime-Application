namespace Messaging.Shared;

public enum MessageType
{
    Ping = 1,
    SuccessAuthentication,
    FailedAuthentication,
    Register,
    SendMessage,
    ListConnections,
    Broadcast,

    // Client message types
    Authentication
}