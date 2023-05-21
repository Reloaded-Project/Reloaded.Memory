

// ReSharper disable RedundantUsingDirective
using System.Runtime.CompilerServices;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Utilities;
// ReSharper disable BuiltInTypeReferenceStyle

namespace Reloaded.Memory.Streams
{
    public partial class BufferedStreamReader<TStream> : IDisposable
    {

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int16 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 PeekLittleEndianInt16()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<Int16>();

        return Endian.Reverse(Peek<Int16>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int16 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 PeekLittleEndian(out Int16 value) => value = PeekLittleEndianInt16();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int16 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 ReadLittleEndianInt16()
    {
        if (BitConverter.IsLittleEndian)
            return Read<Int16>();

        return Endian.Reverse(Read<Int16>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int16 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 ReadLittleEndian(out Int16 value) => value = ReadLittleEndianInt16();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int16 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 PeekBigEndianInt16()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<Int16>();

        return Endian.Reverse(Peek<Int16>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int16 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 PeekBigEndian(out Int16 value) => value = PeekBigEndianInt16();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int16 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 ReadBigEndianInt16()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<Int16>();

        return Endian.Reverse(Read<Int16>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int16 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int16 ReadBigEndian(out Int16 value) => value = ReadBigEndianInt16();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt16 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 PeekLittleEndianUInt16()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<UInt16>();

        return Endian.Reverse(Peek<UInt16>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt16 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 PeekLittleEndian(out UInt16 value) => value = PeekLittleEndianUInt16();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt16 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 ReadLittleEndianUInt16()
    {
        if (BitConverter.IsLittleEndian)
            return Read<UInt16>();

        return Endian.Reverse(Read<UInt16>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt16 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 ReadLittleEndian(out UInt16 value) => value = ReadLittleEndianUInt16();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt16 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 PeekBigEndianUInt16()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<UInt16>();

        return Endian.Reverse(Peek<UInt16>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt16 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 PeekBigEndian(out UInt16 value) => value = PeekBigEndianUInt16();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt16 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 ReadBigEndianUInt16()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<UInt16>();

        return Endian.Reverse(Read<UInt16>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt16 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt16 ReadBigEndian(out UInt16 value) => value = ReadBigEndianUInt16();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int32 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 PeekLittleEndianInt32()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<Int32>();

        return Endian.Reverse(Peek<Int32>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int32 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 PeekLittleEndian(out Int32 value) => value = PeekLittleEndianInt32();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int32 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 ReadLittleEndianInt32()
    {
        if (BitConverter.IsLittleEndian)
            return Read<Int32>();

        return Endian.Reverse(Read<Int32>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int32 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 ReadLittleEndian(out Int32 value) => value = ReadLittleEndianInt32();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int32 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 PeekBigEndianInt32()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<Int32>();

        return Endian.Reverse(Peek<Int32>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int32 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 PeekBigEndian(out Int32 value) => value = PeekBigEndianInt32();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int32 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 ReadBigEndianInt32()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<Int32>();

        return Endian.Reverse(Read<Int32>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int32 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int32 ReadBigEndian(out Int32 value) => value = ReadBigEndianInt32();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt32 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 PeekLittleEndianUInt32()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<UInt32>();

        return Endian.Reverse(Peek<UInt32>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt32 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 PeekLittleEndian(out UInt32 value) => value = PeekLittleEndianUInt32();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt32 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 ReadLittleEndianUInt32()
    {
        if (BitConverter.IsLittleEndian)
            return Read<UInt32>();

        return Endian.Reverse(Read<UInt32>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt32 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 ReadLittleEndian(out UInt32 value) => value = ReadLittleEndianUInt32();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt32 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 PeekBigEndianUInt32()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<UInt32>();

        return Endian.Reverse(Peek<UInt32>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt32 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 PeekBigEndian(out UInt32 value) => value = PeekBigEndianUInt32();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt32 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 ReadBigEndianUInt32()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<UInt32>();

        return Endian.Reverse(Read<UInt32>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt32 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt32 ReadBigEndian(out UInt32 value) => value = ReadBigEndianUInt32();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int64 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 PeekLittleEndianInt64()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<Int64>();

        return Endian.Reverse(Peek<Int64>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int64 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 PeekLittleEndian(out Int64 value) => value = PeekLittleEndianInt64();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int64 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 ReadLittleEndianInt64()
    {
        if (BitConverter.IsLittleEndian)
            return Read<Int64>();

        return Endian.Reverse(Read<Int64>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Int64 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 ReadLittleEndian(out Int64 value) => value = ReadLittleEndianInt64();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int64 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 PeekBigEndianInt64()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<Int64>();

        return Endian.Reverse(Peek<Int64>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int64 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 PeekBigEndian(out Int64 value) => value = PeekBigEndianInt64();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int64 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 ReadBigEndianInt64()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<Int64>();

        return Endian.Reverse(Read<Int64>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Int64 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Int64 ReadBigEndian(out Int64 value) => value = ReadBigEndianInt64();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt64 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 PeekLittleEndianUInt64()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<UInt64>();

        return Endian.Reverse(Peek<UInt64>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt64 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 PeekLittleEndian(out UInt64 value) => value = PeekLittleEndianUInt64();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt64 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 ReadLittleEndianUInt64()
    {
        if (BitConverter.IsLittleEndian)
            return Read<UInt64>();

        return Endian.Reverse(Read<UInt64>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, UInt64 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 ReadLittleEndian(out UInt64 value) => value = ReadLittleEndianUInt64();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt64 from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 PeekBigEndianUInt64()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<UInt64>();

        return Endian.Reverse(Peek<UInt64>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt64 from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 PeekBigEndian(out UInt64 value) => value = PeekBigEndianUInt64();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt64 from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 ReadBigEndianUInt64()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<UInt64>();

        return Endian.Reverse(Read<UInt64>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, UInt64 from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public UInt64 ReadBigEndian(out UInt64 value) => value = ReadBigEndianUInt64();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Single from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single PeekLittleEndianSingle()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<Single>();

        return Endian.Reverse(Peek<Single>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Single from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single PeekLittleEndian(out Single value) => value = PeekLittleEndianSingle();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Single from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single ReadLittleEndianSingle()
    {
        if (BitConverter.IsLittleEndian)
            return Read<Single>();

        return Endian.Reverse(Read<Single>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Single from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single ReadLittleEndian(out Single value) => value = ReadLittleEndianSingle();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Single from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single PeekBigEndianSingle()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<Single>();

        return Endian.Reverse(Peek<Single>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Single from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single PeekBigEndian(out Single value) => value = PeekBigEndianSingle();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Single from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single ReadBigEndianSingle()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<Single>();

        return Endian.Reverse(Read<Single>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Single from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Single ReadBigEndian(out Single value) => value = ReadBigEndianSingle();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Double from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double PeekLittleEndianDouble()
    {
        if (BitConverter.IsLittleEndian)
            return Peek<Double>();

        return Endian.Reverse(Peek<Double>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Double from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double PeekLittleEndian(out Double value) => value = PeekLittleEndianDouble();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Double from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double ReadLittleEndianDouble()
    {
        if (BitConverter.IsLittleEndian)
            return Read<Double>();

        return Endian.Reverse(Read<Double>());
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, Double from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double ReadLittleEndian(out Double value) => value = ReadLittleEndianDouble();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Double from the stream without incrementing the position.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double PeekBigEndianDouble()
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<Double>();

        return Endian.Reverse(Peek<Double>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Double from the stream without incrementing the position.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double PeekBigEndian(out Double value) => value = PeekBigEndianDouble();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Double from the stream.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double ReadBigEndianDouble()
    {
        if (!BitConverter.IsLittleEndian)
            return Read<Double>();

        return Endian.Reverse(Read<Double>());
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, Double from the stream.
    /// </summary>
    /// <param name="value">The variable to receive the output value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public Double ReadBigEndian(out Double value) => value = ReadBigEndianDouble();
    }
}
