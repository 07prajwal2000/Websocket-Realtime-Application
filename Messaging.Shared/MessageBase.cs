using Messaging.Shared.MessageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared;

public abstract class MessageBase
{
    public MessageType MessageType { get; set; }
    public MessageBase(MessageType messageType)
    {
        MessageType = messageType;
    }

    public abstract ArraySegment<byte> ToArraySegment();
}
