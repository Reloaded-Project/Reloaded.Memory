using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

#pragma warning disable CS1591

namespace Reloaded.Memory.Streams;

/// <summary>
///     Utility for writing to a pointer in Little Endian.
/// </summary>
[PublicAPI]
public unsafe struct LittleEndianReader : IEndianReader
{
    /// <summary>
    ///     Current pointer being read from.
    /// </summary>
    public byte* Ptr;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LittleEndianReader" /> struct with the given pointer.
    /// </summary>
    /// <param name="ptr">The pointer to read from.</param>
    public LittleEndianReader(byte* ptr) => Ptr = ptr;

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
        var result = LittleEndianHelper.Read((short*)Ptr);
        Ptr += sizeof(short);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort()
    {
        var result = LittleEndianHelper.Read((ushort*)Ptr);
        Ptr += sizeof(ushort);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt()
    {
        var result = LittleEndianHelper.Read((uint*)Ptr);
        Ptr += sizeof(uint);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt()
    {
        var result = LittleEndianHelper.Read((int*)Ptr);
        Ptr += sizeof(int);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULong()
    {
        var result = LittleEndianHelper.Read((ulong*)Ptr);
        Ptr += sizeof(ulong);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLong()
    {
        var result = LittleEndianHelper.Read((long*)Ptr);
        Ptr += sizeof(long);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloat()
    {
        var result = LittleEndianHelper.Read((float*)Ptr);
        Ptr += sizeof(float);
        return result;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble()
    {
        var result = LittleEndianHelper.Read((double*)Ptr);
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
    public short ReadShortAtOffset(int offset) => LittleEndianHelper.Read((short*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShortAtOffset(int offset) => LittleEndianHelper.Read((ushort*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadIntAtOffset(int offset) => LittleEndianHelper.Read((int*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUIntAtOffset(int offset) => LittleEndianHelper.Read((uint*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLongAtOffset(int offset) => LittleEndianHelper.Read((long*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULongAtOffset(int offset) => LittleEndianHelper.Read((ulong*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloatAtOffset(int offset) => LittleEndianHelper.Read((float*)(Ptr + offset));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDoubleAtOffset(int offset) => LittleEndianHelper.Read((double*)(Ptr + offset));

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
        if (BitConverter.IsLittleEndian)
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
