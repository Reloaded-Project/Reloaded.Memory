using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Streams;

public partial class BufferedStreamReader<TStream>
{
    // Note: BitConverter.IsLittleEndian evaluates at JIT time and becomes a no-op.
    //       This code should technically be in its own extension class, but for performance reasons
    //       we are keeping it here; due to inlining issues.

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public T PeekLittleEndianStruct<T>() where T : unmanaged, ICanReverseEndian
    {
        if (BitConverter.IsLittleEndian)
            return Peek<T>();

        var value = Peek<T>();
        value.ReverseEndian();
        return value;
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    /// <param name="value">The value to return.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public void PeekLittleEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
        => value = PeekLittleEndianStruct<T>();

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public T ReadLittleEndianStruct<T>() where T : unmanaged, ICanReverseEndian
    {
        if (BitConverter.IsLittleEndian)
            return Read<T>();

        var value = Read<T>();
        value.ReverseEndian();
        return value;
    }

    /// <summary>
    ///     [Little Endian] Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    /// <param name="value">The read in value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public void ReadLittleEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
        => value = ReadLittleEndianStruct<T>();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public T PeekBigEndianStruct<T>() where T : unmanaged, ICanReverseEndian
    {
        if (!BitConverter.IsLittleEndian)
            return Peek<T>();

        var value = Peek<T>();
        value.ReverseEndian();
        return value;
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    /// <param name="value">The value to return.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public void PeekBigEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
        => value = PeekBigEndianStruct<T>();

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public T ReadBigEndianStruct<T>() where T : unmanaged, ICanReverseEndian
    {
        if (!BitConverter.IsLittleEndian)
            return Read<T>();

        var value = Read<T>();
        value.ReverseEndian();
        return value;
    }

    /// <summary>
    ///     [Big Endian] Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    /// <param name="value">The read in value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage]
    public void ReadBigEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
        => value = ReadBigEndianStruct<T>();
}
