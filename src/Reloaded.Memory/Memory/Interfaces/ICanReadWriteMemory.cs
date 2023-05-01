using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Utility;

namespace Reloaded.Memory.Memory.Interfaces;

/// <summary>
///     A simple interface that provides read/write access to arbitrary sequential memory.
/// </summary>
public interface ICanReadWriteMemory
{
    /// <summary>
    ///     Reads a generic type from a specified memory address.
    /// </summary>
    /// <typeparam name="T">Any unmanaged type.</typeparam>
    /// <param name="offset">The memory address/offset to read from.</param>
    /// <param name="value">Variable to receive the read in struct.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    void ReadRef<T>(nuint offset, ref T value) where T : unmanaged;

    /// <summary>
    ///     Reads a generic type from a specified memory address; using Marshalling.
    /// </summary>
    /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
    /// <param name="offset">The memory address to read from.</param>
    /// <param name="value">Local variable to receive the read in struct.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    void ReadWithMarshalling<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(nuint offset, [DisallowNull] ref T value);

    /// <summary>
    ///     Reads raw data from a specified memory address.
    /// </summary>
    /// <param name="offset">The memory address to read from.</param>
    /// <param name="value">Span to receive the read in bytes.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    void ReadRaw(nuint offset, Span<byte> value);

    /// <summary>
    ///     Writes a generic type to a specified memory address.
    /// </summary>
    /// <typeparam name="T">
    ///     An individual struct type of a class with an explicit <see cref="StructLayoutAttribute" />
    ///     attribute.
    /// </typeparam>
    /// <param name="offset">The memory address to write to.</param>
    /// <param name="item">The item to write to the address.</param>
    /// <exception cref="MemoryException">Failed to write memory.</exception>
    void Write<T>(nuint offset, in T item) where T : unmanaged;

    /// <summary>
    ///     Writes a generic type to a specified memory address; using marshalling.
    /// </summary>
    /// <typeparam name="T">
    ///     An individual struct type of a class with an explicit <see cref="StructLayoutAttribute" />
    ///     attribute.
    /// </typeparam>
    /// <param name="offset">The memory address to write to.</param>
    /// <param name="item">The item to write to the address.</param>
    /// <exception cref="MemoryException">Failed to write memory.</exception>
    void WriteWithMarshalling<T>(nuint offset, [DisallowNull] in T item);

    /// <summary>
    ///     Writes raw data to a specified memory address.
    /// </summary>
    /// <param name="offset">The memory address to read from.</param>
    /// <param name="data">The bytes to write to memoryAddress.</param>
    /// <exception cref="MemoryException">Failed to write memory.</exception>
    void WriteRaw(nuint offset, Span<byte> data);
}

/// <summary>
///     Extension methods for implementers of <see cref="ICanReadWriteMemory" />.
/// </summary>
public static class CanReadWriteMemoryExtensions
{
    /// <summary>
    ///     Reads a generic type from a specified memory address.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <typeparam name="TData">Any unmanaged type.</typeparam>
    /// <param name="source">Implementation of <see cref="ICanReadWriteMemory" />.</param>
    /// <param name="offset">The memory address/offset to read from.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    public static TData Read<TSource, TData>(this TSource source, nuint offset)
        where TSource : ICanReadWriteMemory where TData : unmanaged
    {
        TData result = default;
        source.ReadRef(offset, ref result);
        return result;
    }

    /// <summary>
    ///     Reads a generic type from a specified memory address.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <typeparam name="TData">Any unmanaged type.</typeparam>
    /// <param name="source">Implementation of <see cref="ICanReadWriteMemory" />.</param>
    /// <param name="offset">The memory address/offset to read from.</param>
    /// <param name="value">Variable to receive the read in struct.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Read<TSource, TData>(this TSource source, nuint offset, out TData value)
        where TSource : ICanReadWriteMemory where TData : unmanaged => value = source.Read<TSource, TData>(offset);

    /// <summary>
    ///     Reads a generic type from a specified memory address; using Marshalling.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <typeparam name="TData">
    ///     An individual struct type of a class with an explicit <see cref="StructLayoutAttribute" />
    ///     attribute.
    /// </typeparam>
    /// <param name="source">Implementation of <see cref="ICanReadWriteMemory" />.</param>
    /// <param name="offset">The memory address to read from.</param>
    /// <param name="value">Local variable to receive the read in struct.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    public static void ReadWithMarshalling<TSource,
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        TData>(this TSource source, nuint offset, [DisallowNull] out TData? value)
        where TSource : ICanReadWriteMemory where TData : new()
    {
        value = ReadWithMarshalling<TSource, TData>(source, offset);
    }

    /// <summary>
    ///     Reads a generic type from a specified memory address; using Marshalling.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <typeparam name="TData">
    ///     An individual struct type of a class with an explicit <see cref="StructLayoutAttribute" />
    ///     attribute.
    /// </typeparam>
    /// <param name="source">Implementation of <see cref="ICanReadWriteMemory" />.</param>
    /// <param name="offset">The memory address to read from.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    public static TData ReadWithMarshalling<TSource,
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        TData>(this TSource source, nuint offset) where TSource : ICanReadWriteMemory where TData : new()
    {
        TData result = new();
        source.ReadWithMarshalling(offset, ref result);
        return result!;
    }

    /// <summary>
    ///     Reads raw data from a specified memory address.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <param name="source">Implementation of <see cref="ICanReadWriteMemory" />.</param>
    /// <param name="offset">The memory address to read from.</param>
    /// <param name="length">Number of bytes to place in returned array.</param>
    /// <exception cref="MemoryException">Failed to read memory.</exception>
    /// <remarks>
    ///     This method is provided for convenience. It is recommended you use the Span overload instead.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ReadRaw<TSource>(this TSource source, nuint offset, int length)
        where TSource : ICanReadWriteMemory
    {
        byte[] result = Polyfills.AllocateUninitializedArray<byte>(length);
        source.ReadRaw(offset, result.AsSpanFast());
        return result;
    }
}
