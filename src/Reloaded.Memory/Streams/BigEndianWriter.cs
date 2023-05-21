using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

#pragma warning disable CS1591

namespace Reloaded.Memory.Streams;

/// <summary>
///     Utility for writing to a pointer in Big Endian.
/// </summary>
[PublicAPI]
public unsafe struct BigEndianWriter : IEndianWriter
{
    /// <summary>
    ///     Current pointer being written to.
    /// </summary>
    public byte* Ptr;

    /// <summary>
    ///     Creates a simple wrapper around a pointer that writes in Big Endian.
    /// </summary>
    /// <param name="ptr">Pointer to the item behind the writer.</param>
    public BigEndianWriter(byte* ptr) => Ptr = ptr;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(sbyte value)
    {
        *(sbyte*)Ptr = value;
        Ptr += sizeof(byte);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(byte value)
    {
        *Ptr = value;
        Ptr += sizeof(byte);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(short value)
    {
        BigEndianHelper.Write((short*)Ptr, value);
        Ptr += sizeof(short);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ushort value)
    {
        BigEndianHelper.Write((ushort*)Ptr, value);
        Ptr += sizeof(ushort);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(uint value)
    {
        BigEndianHelper.Write((uint*)Ptr, value);
        Ptr += sizeof(uint);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(int value)
    {
        BigEndianHelper.Write((int*)Ptr, value);
        Ptr += sizeof(int);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(long value)
    {
        BigEndianHelper.Write((long*)Ptr, value);
        Ptr += sizeof(long);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ulong value)
    {
        BigEndianHelper.Write((ulong*)Ptr, value);
        Ptr += sizeof(ulong);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(float value)
    {
        BigEndianHelper.Write((float*)Ptr, value);
        Ptr += sizeof(float);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(double value)
    {
        BigEndianHelper.Write((double*)Ptr, value);
        Ptr += sizeof(double);
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(Span<byte> data)
    {
        fixed (byte* dataPtr = data)
        {
            Unsafe.CopyBlockUnaligned(Ptr, dataPtr, (uint)data.Length);
            Ptr += data.Length;
        }
    }

    /*
        About the Methods Below:

            These are for faster writes of structs.

            While they don't reduce the instruction count by much (at all);
            they reduce the dependence of future instructions on earlier instructions,
            (future write does not need to wait for updated value of `Ptr`).

            This allows for better pipelining.

            Also the JIT can see `offset` parameters as constant when specified as constants
            and optimise that out accordingly.
    */

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(sbyte value, int offset) => *(sbyte*)(Ptr + offset) = value;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(byte value, int offset) => *(Ptr + offset) = value;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(short value, int offset) => BigEndianHelper.Write((short*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(ushort value, int offset) => BigEndianHelper.Write((ushort*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(int value, int offset) => BigEndianHelper.Write((int*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(uint value, int offset) => BigEndianHelper.Write((uint*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(long value, int offset) => BigEndianHelper.Write((long*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(ulong value, int offset) => BigEndianHelper.Write((ulong*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(float value, int offset) => BigEndianHelper.Write((float*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteAtOffset(double value, int offset) => BigEndianHelper.Write((double*)(Ptr + offset), value);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Seek(int offset) => Ptr += offset;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
    public void Write<T>(in T item) where T : unmanaged, ICanWriteToAnEndianWriter => item.Write(ref this);
}
