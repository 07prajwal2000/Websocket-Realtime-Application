using System.Text;

namespace Messaging.Shared.Packets;

public class ReadPacket
{
    private int position;
    private readonly ArraySegment<byte> data;

    public ReadPacket(byte[] data)
    {
        this.data = data;
    }
    public ReadPacket(ArraySegment<byte> data)
    {
        this.data = data;
    }

    public float ReadFloat()
    {
        ReadOnlySpan<byte> temp = data;
        temp = temp[position..(position + 4)];
        IncrementPosition(4);
        return BitConverter.ToSingle(temp);
    }
    public int ReadInt()
    {
        ReadOnlySpan<byte> temp = data;
        temp = temp[position..(position + 4)];
        IncrementPosition(4);
        return BitConverter.ToInt32(temp);
    }
    public long ReadLong()
    {
        ReadOnlySpan<byte> temp = data;
        temp = temp[position..(position + 8)];
        IncrementPosition(8);
        return BitConverter.ToInt64(temp);
    }
    public bool ReadBool()
    {
        ReadOnlySpan<byte> temp = data;
        temp = temp[position..(position + 1)];
        IncrementPosition(1);
        return BitConverter.ToBoolean(temp);
    }
    public char ReadChar()
    {
        ReadOnlySpan<byte> temp = data;
        temp = temp[position..(position + 1)];
        IncrementPosition(1);
        return BitConverter.ToChar(temp);
    }
    public string ReadString()
    {
        var len = ReadInt();
        ReadOnlySpan<byte> value = data;
        value = value[position..(position + len)];
        IncrementPosition(len);
        return Encoding.UTF8.GetString(value);
    }

    private void IncrementPosition(int value)
    {
        position += value;
    }

    public void ResetPosition() => position = 0;
}
