using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

#pragma warning disable CS1591

namespace Reloaded.Memory.Streams;

/// <summary>
///     Utility for writing to a pointer in Little Endian.
/// </summary>
[PublicAPI]
public unsafe struct BigEndianReader : IEndianReader
{
    /// <summary>
    ///     Current pointer being read from.
    /// </summary>
    public byte* Ptr;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BigEndianReader" /> struct with the given pointer.
    /// </summary>
    /// <param name="ptr">The pointer to read from.</param>
    public BigEndianReader(byte* ptr) => Ptr = ptr;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte()
    {
        var result = *Ptr;
        Ptr += sizeof(byte);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSByte()
    {
        var result = *(sbyte*)Ptr;
        Ptr += sizeof(sbyte);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShort()
    {
        var result = BigEndianHelper.Read((short*)Ptr);
        Ptr += sizeof(short);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort()
    {
        var result = BigEndianHelper.Read((ushort*)Ptr);
        Ptr += sizeof(ushort);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt()
    {
        var result = BigEndianHelper.Read((uint*)Ptr);
        Ptr += sizeof(uint);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt()
    {
        var result = BigEndianHelper.Read((int*)Ptr);
        Ptr += sizeof(int);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULong()
    {
        var result = BigEndianHelper.Read((ulong*)Ptr);
        Ptr += sizeof(ulong);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLong()
    {
        var result = BigEndianHelper.Read((long*)Ptr);
        Ptr += sizeof(long);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloat()
    {
        var result = BigEndianHelper.Read((float*)Ptr);
        Ptr += sizeof(float);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble()
    {
        var result = BigEndianHelper.Read((double*)Ptr);
        Ptr += sizeof(double);
        return result;
    }

    /*
         Note: See equivalent section in LittleEndianWriter.
    */

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByteAtOffset(int offset) => *(Ptr + offset);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSByteAtOffset(int offset) => *(sbyte*)(Ptr + offset);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShortAtOffset(int offset) => BigEndianHelper.Read((short*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShortAtOffset(int offset) => BigEndianHelper.Read((ushort*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadIntAtOffset(int offset) => BigEndianHelper.Read((int*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUIntAtOffset(int offset) => BigEndianHelper.Read((uint*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLongAtOffset(int offset) => BigEndianHelper.Read((long*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULongAtOffset(int offset) => BigEndianHelper.Read((ulong*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloatAtOffset(int offset) => BigEndianHelper.Read((float*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDoubleAtOffset(int offset) => BigEndianHelper.Read((double*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Seek(int offset) => Ptr += offset;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Read<T>(ref T item) where T : ICanBeReadByAnEndianReader => item.Read(ref this);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Read<T>() where T : unmanaged, ICanBeReadByAnEndianReader
    {
        if (!BitConverter.IsLittleEndian)
        {
            T result = *(T*)Ptr;
            Ptr += sizeof(T);
            return result;
        }

        var value = new T();
        value.Read(ref this);
        return value;
    }
}
