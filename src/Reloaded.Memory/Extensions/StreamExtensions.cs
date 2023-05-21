#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using Reloaded.Memory.Internals;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Provides various extensions to streams.
/// </summary>
[PublicAPI]
public static unsafe class StreamExtensions
{
    private const int MaxStackLimit = 1024;

    /// <summary>
    ///     Pads the stream with any/random bytes until it is aligned.
    /// </summary>
    /// <typeparam name="TStream">Type of stream padding is</typeparam>
    /// <param name="stream">The stream that will be aligned to the alignment granularity.</param>
    /// <param name="alignment">The desired alignment of the stream.</param>
    /// <remarks>
    ///     Usually this will pad with 0x0, but that might not be true for 100% of streams.
    ///     It is dependent on implementation of <see cref="Stream.SetLength" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddPadding<TStream>(this TStream stream, int alignment) where TStream : Stream
    {
        var padding = Mathematics.RoundUp((int)stream.Position, alignment) - stream.Position;
        if (padding <= 0)
            return;

        stream.Position += padding;
        if (stream.Position > stream.Length)
            stream.SetLength(stream.Position);
    }

    /// <summary>
    ///     Pads the stream with <see paramref="value" /> bytes until it is aligned.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <param name="stream">The stream to add padding to.</param>
    /// <param name="value">Value to insert into the padding.</param>
    /// <param name="alignment">The desired alignment of the stream.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddPadding<TStream>(this TStream stream, byte value, int alignment = 2048) where TStream : Stream
    {
        var padding = Mathematics.RoundUp((int)stream.Position, alignment) - stream.Position;
        if (padding <= 0)
            return;

        // Hot path.
        using var alloc = new ArrayRental((int)padding);
        alloc.Array.AsSpan(0, (int)padding).Fill(value);
        stream.Write(alloc.Array, 0, (int)padding);
    }

    /// <summary>
    ///     Appends an unmanaged structure onto the <paramref name="stream" /> and advances the position.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to write.</typeparam>
    /// <param name="stream">Type of stream used to write to the output.</param>
    /// <param name="structure">Span of items to be written to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<TStream, T>(this TStream stream, Span<T> structure)
        where T : unmanaged where TStream : Stream
    {
        Span<byte> byteSpan = MemoryMarshal.Cast<T, byte>(structure);
        Polyfills.Write(stream, byteSpan);
    }

    /// <summary>
    ///     Appends an unmanaged structure onto the <paramref name="stream" /> and advances the position.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to write.</typeparam>
    /// <param name="stream">Type of stream used to write to the output.</param>
    /// <param name="structure">Span of items to be written to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<TStream, T>(this TStream stream, in T structure) where T : unmanaged where TStream : Stream
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        Span<T> structureSpan = MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in structure), 1);
        Span<byte> byteSpan = MemoryMarshal.Cast<T, byte>(structureSpan);
        stream.Write(byteSpan);
#else
        T localStructure = structure;
        var structureSpan = new Span<T>(&localStructure, 1);
        Span<byte> byteSpan = MemoryMarshal.Cast<T, byte>(structureSpan);
        stream.Write(byteSpan);
#endif
    }

    /// <summary>
    ///     Appends a managed/marshalled structure onto the given <see cref="MemoryStream" /> and advances the position.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to write.</typeparam>
    /// <param name="stream">Type of stream used to write to the output.</param>
    /// <param name="item">Item to be written to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteMarshalled<TStream, T>(this TStream stream, T item) where TStream : Stream
    {
        var size = TypeInfo.MarshalledSizeOf<T>();
        if (size < MaxStackLimit)
        {
            var stack = stackalloc byte[size];
            Marshal.StructureToPtr(item!, (IntPtr)stack, false);
            Polyfills.Write(stream, new Span<byte>(stack, size));
            return;
        }

        var array = Polyfills.AllocateUninitializedArray<byte>(size);
        fixed (byte* arrayPtr = array)
            Marshal.StructureToPtr(item!, (IntPtr)arrayPtr, false);

        stream.Write(array, 0, array.Length);
    }

    /// <summary>
    ///     Appends a managed/marshalled structure onto the given <see cref="MemoryStream" /> and advances the position.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to write.</typeparam>
    /// <param name="stream">Type of stream used to write to the output.</param>
    /// <param name="item">Array of items to be written to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteMarshalled<TStream, T>(this TStream stream, T[] item) where TStream : Stream
        => WriteMarshalled(stream, item.AsSpan());

    /// <summary>
    ///     Appends a managed/marshalled structure onto the given <see cref="MemoryStream" /> and advances the position.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to write.</typeparam>
    /// <param name="stream">Type of stream used to write to the output.</param>
    /// <param name="item">Span of items to be written to the output.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteMarshalled<TStream, T>(this TStream stream, Span<T> item) where TStream : Stream
    {
        var itemSize = TypeInfo.MarshalledSizeOf<T>();
        var totalSize = itemSize * item.Length;

        if (totalSize < MaxStackLimit)
        {
            var stack = stackalloc byte[totalSize];
            var currentStackPtr = stack;
            for (var x = 0; x < item.Length; x++)
            {
                Marshal.StructureToPtr(item[x]!, (IntPtr)currentStackPtr, false);
                currentStackPtr += itemSize;
            }

            stream.Write(new Span<byte>(stack, totalSize));
            return;
        }

        var array = Polyfills.AllocateUninitializedArray<byte>(totalSize);
        fixed (byte* arrayPtr = array)
        {
            var currentArrayPtr = arrayPtr;
            for (var x = 0; x < item.Length; x++)
            {
                Marshal.StructureToPtr(item[x]!, (IntPtr)currentArrayPtr, false);
                currentArrayPtr += itemSize;
            }
        }

        stream.Write(array, 0, array.Length);
    }

    /// <summary>
    ///     Reads a single unmanaged structure of type T from the stream.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="result">Result of the reading operation.</param>
    /// <returns>The item read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Read<TStream, T>(this TStream stream, out T result) where T : unmanaged where TStream : Stream
    {
        var size = sizeof(T);
        Span<byte> byteSpan = stackalloc byte[size];
        stream.ReadAtLeast(byteSpan);
        result = MemoryMarshal.Read<T>(byteSpan);
    }

    /// <summary>
    ///     Reads a span of unmanaged structures of type T from the stream into the provided output span.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of items to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="output">The array to store the read items.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Read<TStream, T>(this TStream stream, T[] output) where T : unmanaged where TStream : Stream
        => Read(stream, output.AsSpan());

    /// <summary>
    ///     Reads a span of unmanaged structures of type T from the stream into the provided output span.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of items to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="output">The span to store the read items.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Read<TStream, T>(this TStream stream, Span<T> output) where T : unmanaged where TStream : Stream
    {
        Span<byte> byteSpan = MemoryMarshal.Cast<T, byte>(output);
        stream.ReadAtLeast(byteSpan);
    }

    /// <summary>
    ///     Reads a single marshalled structure of type T from the stream.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of item to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="result">The result of the stream operation.</param>
    /// <returns>The item read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadMarshalled<TStream,
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TStream stream, out T result) where TStream : Stream
    {
        var size = TypeInfo.MarshalledSizeOf<T>();
        Span<byte> byteSpan = stackalloc byte[size];
        stream.ReadAtLeast(byteSpan);

        fixed (byte* bytePtr = byteSpan)
            result = Marshal.PtrToStructure<T>((nint)bytePtr)!;
    }

    /// <summary>
    ///     Reads a span of marshalled structures of type T from the stream into the provided output span.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of items to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="output">The array to store the read items.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadMarshalled<TStream,
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TStream stream, T[] output) where TStream : Stream
    {
        ReadMarshalled(stream, output.AsSpan());
    }

    /// <summary>
    ///     Reads a span of marshalled structures of type T from the stream into the provided output span.
    /// </summary>
    /// <typeparam name="TStream">Type of stream.</typeparam>
    /// <typeparam name="T">Type of items to read.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="output">The span to store the read items.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReadMarshalled<TStream,
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(this TStream stream, Span<T> output) where TStream : Stream
    {
        var itemSize = TypeInfo.MarshalledSizeOf<T>();
        var totalSize = itemSize * output.Length;
        Span<byte> byteSpan = totalSize < MaxStackLimit
            ? stackalloc byte[totalSize]
            : Polyfills.AllocateUninitializedArray<byte>(totalSize);
        stream.ReadAtLeast(byteSpan);

        fixed (byte* spanPtr = byteSpan)
        {
            var currentPtr = spanPtr;
            for (var x = 0; x < output.Length; x++)
            {
                output[x] = Marshal.PtrToStructure<T>((IntPtr)currentPtr)!;
                currentPtr += itemSize;
            }
        }
    }
}
