using System.Text;

namespace Messaging.Shared.Packets;

public class WritePacket
{
    private int position;
    private readonly byte[] data;

    public WritePacket(int size)
    {
        data = new byte[size];
    }
    
    public void WriteFloat(in float value)
    {
        Span<byte> location = data;
        location = location[position..(position + 4)];
        BitConverter.TryWriteBytes(location, value);
        IncrementPosition(sizeof(float));
    }
    
    public void WriteLong(in long value)
    {
        Span<byte> location = data;
        location = location[position..(position + 8)];
        BitConverter.TryWriteBytes(location, value);
        IncrementPosition(sizeof(long));
    }
    
    public void WriteInt(in int value)
    {
        Span<byte> location = data;
        location = location[position..(position + 4)];
        BitConverter.TryWriteBytes(location, value);
        IncrementPosition(sizeof(int));
    }
    
    public void WriteBool(in bool value)
    {
        Span<byte> location = data;
        location = location[position..(position + 1)];
        BitConverter.TryWriteBytes(location, value);
        IncrementPosition(sizeof(bool));
    }

    public int WriteString(in string value)
    {
        WriteInt(value.Length);
        Span<byte> location = data;
        location = location[position..(position + value.Length)];
        var totalRead = Encoding.UTF8.GetBytes(value, location);
        IncrementPosition(value.Length);
        return totalRead;
    }

    public void WriteChar(in char value)
    {
        Span<byte> location = data;
        
        BitConverter.TryWriteBytes(location, value);
        IncrementPosition(1);
    }

    public byte[] ToBytes() => data;

    public ArraySegment<byte> ToBytesTrimmed()
    {
        var segment = new ArraySegment<byte>(data);
        return segment[..position];
    }
    
    public ArraySegment<byte> ToBytesTrimmed(out int totalWritten)
    {
        totalWritten = position;
        return ToBytesTrimmed();
    }

    /// <summary>
    /// Resets to zero
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = 0;
        }
    }

    private void IncrementPosition(int value)
    {
        position += value;
    }
}
