using Reloaded.Memory.Utilities;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Reloaded.Memory.Streams;

/// <summary>
///     This class adds a buffering mechanism for streams, allowing for fast reading of data behind streams.
///     This aims to speed up read/writes while preserving stream-like API/semantics.
/// </summary>
/// <typeparam name="TStream">Type of underlying stream.</typeparam>
/// <remarks>
///     Using this class is slower than reading from stream directly and parsing received buffers; as each
///     read here requires a bounds check. But it is faster than any of the common solutions such as
///     <see cref="BinaryReader" />
///     or <see cref="BinaryWriter" /> as minimal error checking is done.
/// </remarks>
[PublicAPI]
public unsafe partial class BufferedStreamReader<TStream> where TStream : Stream
{
    private readonly byte[] _buffer;

    private int _bufferOffset; // Read offset.

    /* Pointers */
    private GCHandle _gcHandle;
    private readonly nint _gcHandlePtr;
    private ArrayRental _rental;
    private bool _disposed;

    /* Properties (Written as methods to force inlining). */

    /// <summary>
    ///     Gets the stream this class was instantiated with.
    /// </summary>
    public TStream BaseStream { get; }

    /// <summary>
    ///     Gets the remaining number of bytes that are currently buffered.
    /// </summary>
    public int BufferedBytesAvailable { get; private set; }

    /// <summary>
    ///     Gets the total size of the current buffered data at this moment in time.
    /// </summary>
    public int CurrentBufferSize => _bufferOffset + BufferedBytesAvailable;

    /// <summary>
    ///     This is true if end of stream was reached while refilling the internal buffer.
    /// </summary>
    public bool IsEndOfStream { get; private set; }

    /// <summary>
    ///     This method is executed if a buffer refill does not fill the whole buffer, indicating end of stream was reached.
    /// </summary>
    /// <remarks>
    ///     This is provided as it can help reduce redundant checks for end of stream in user code.
    /// </remarks>
    public Action? OnEndOfStream;

    /// <summary>
    ///     The current position of the buffer.
    /// </summary>
    public long Position => BaseStream.Position - BufferedBytesAvailable;

    /// <summary>
    ///     Constructs a <see cref="BufferedStreamReader{TStream}" />.
    /// </summary>
    /// <param name="stream">The stream to add buffering capabilities to.</param>
    /// <param name="bufferSize">
    ///     The size of the buffer.<br /><br />
    ///     Benchmarking suggests 65536 to be an optimal value for <see cref="FileStream" /> and 512/2048 optimal for
    ///     <see cref="MemoryStream" />.<br /><br />
    /// </param>
    public BufferedStreamReader(TStream stream, int bufferSize = 65536)
    {
        BaseStream = stream;
        _rental = new ArrayRental(bufferSize);
        _buffer = _rental.Array;
        _gcHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
        _gcHandlePtr = _gcHandle.AddrOfPinnedObject();
    }

    /// <inheritdoc />
    ~BufferedStreamReader() => Dispose();

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _rental.Dispose();
        if (_gcHandle.IsAllocated)
            _gcHandle.Free();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Seeks the underlying stream to a specified position.
    /// </summary>
    /// <param name="offset">Offset of the origin.</param>
    /// <param name="origin">Where to seek the stream from.</param>
    /// <remarks>
    ///     For performance reasons, prefer <see cref="Advance" /> if you intend to seek from current position.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Seek(long offset, SeekOrigin origin)
    {
        if (origin == SeekOrigin.Begin)
            offset -= Position;

        // seekTarget == _stream.Length - offset
        else if (origin == SeekOrigin.End)
            offset = BaseStream.Length - offset - Position;

        RelativeSeek(offset);
    }

    /// <summary>
    ///     Advances the underlying stream by a specified number of bytes.
    /// </summary>
    /// <param name="offset">Offset of the origin.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Advance(long offset) => RelativeSeek(offset);

    /// <summary>
    ///     Reads a specified amount of bytes at a specific offset from the underlying stream without
    ///     resetting the buffers or advancing the read pointer.
    ///     Note: This method is intended only for reading of large size raw data e.g. compressed file data in an archive.
    ///     No optimizations are performed for this action.
    /// </summary>
    /// <param name="offset">The offset of the data from the start of the stream.</param>
    /// <param name="data">The span to read the data into.</param>
    /// <returns>
    ///     Number of bytes read.
    ///     This might be less than the amount requested.
    ///     If it is less, end of stream was reached.
    /// </returns>
    public int ReadBytesUnbuffered(long offset, Span<byte> data)
    {
        var originalPosition = BaseStream.Position;

        try
        {
            BaseStream.Position = offset;
            var result = BaseStream.ReadAtLeast(data);
            return result;
        }
        finally
        {
            BaseStream.Position = originalPosition;
        }
    }

    /// <summary>
    ///     Reads a value that requires marshalling from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    /// <returns>The value read, after marshalling.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ReadMarshalled<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>()
    {
        var size = TypeInfo.MarshalledSizeOf<T>();
        if (CanRead(size))
        {
            var result = Marshal.PtrToStructure<T>((nint)(void*)_gcHandlePtr + _bufferOffset)!;
            _bufferOffset += size;
            BufferedBytesAvailable -= size;
            return result;
        }

        ReFillBuffer();
        var result2 = Marshal.PtrToStructure<T>((nint)(void*)_gcHandlePtr + _bufferOffset)!;
        _bufferOffset += size;
        BufferedBytesAvailable -= size;
        return result2;
    }

    /// <summary>
    ///     Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Read<T>() where T : unmanaged
    {
        var size = sizeof(T);
        if (CanRead(size))
        {
            T result = *(T*)((byte*)_gcHandlePtr + _bufferOffset);
            _bufferOffset += size;
            BufferedBytesAvailable -= size;
            return result;
        }

        ReFillBuffer();
        T result2 = *(T*)((byte*)_gcHandlePtr + _bufferOffset);
        _bufferOffset += size;
        BufferedBytesAvailable -= size;
        return result2;
    }

    /// <summary>
    ///     Reads raw data from the stream, without conversion.
    /// </summary>
    /// <param name="length">The length of bytes requested.</param>
    /// <param name="available">
    ///     Total available length. This can be less than requested if reached end of file or not all items
    ///     can fit in buffer.
    /// </param>
    /// <returns>
    ///     Pointer to the raw read data.
    ///     DO NOT MODIFY THE DATA IN THIS BUFFER. THIS IS A DIRECT POINTER TO INTERNAL BUFFER.
    /// </returns>
    /// <remarks>
    ///     Prefer this method over any of the other methods if you intend on reading large amount of data.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte* ReadRaw(int length, out int available)
    {
        if (CanRead(length))
        {
            var result = (byte*)_gcHandlePtr + _bufferOffset;
            available = length;
            _bufferOffset += length;
            BufferedBytesAvailable -= length;
            return result;
        }

        ReFillBuffer();
        var result2 = (byte*)_gcHandlePtr + _bufferOffset;
        available = Math.Min(BufferedBytesAvailable, length);
        _bufferOffset += available;
        BufferedBytesAvailable -= available;
        return result2;
    }

    /// <summary>
    ///     Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    /// <param name="numItems">Number of items requested.</param>
    /// <param name="available">
    ///     Total available items. This can be less than requested if reached end of file or buffer cannot
    ///     hold more items.
    /// </param>
    /// <returns>
    ///     Pointer to the raw read data.
    ///     DO NOT MODIFY THE DATA IN THIS BUFFER. THIS IS A DIRECT POINTER TO INTERNAL BUFFER.
    /// </returns>
    /// <remarks>
    ///     Prefer this method over any of the other methods if you intend on reading large amount of data.
    ///     This method is slow if sizeof(<typeparamref name="T" />) is not a power of 2.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T* ReadRaw<T>(int numItems, out int available) where T : unmanaged
    {
        var result = ReadRaw(sizeof(T) * numItems, out var availableBytes);
        available = availableBytes / sizeof(T);
        Advance(available * sizeof(T) - availableBytes);
        return (T*)result;
    }

    /// <summary>
    ///     Reads a number of items into a span.
    /// </summary>
    /// <typeparam name="T">Type of value. sizeof(<typeparamref name="T" />) must be lesser than buffer size of reader.</typeparam>
    /// <param name="buffer">
    ///     The buffer to read the items into.
    /// </param>
    /// <returns>
    ///     Number of items read into the buffer.
    /// </returns>
    /// <remarks>
    ///     Prefer this method over any of the other methods if you intend on reading large amount of data.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadRaw<T>(Span<T> buffer) where T : unmanaged
    {
        fixed (T* bufferPtr = buffer)
            return ReadRaw(bufferPtr, buffer.Length);
    }

    /// <summary>
    ///     Reads a number of items into a span.
    /// </summary>
    /// <typeparam name="T">Type of value. sizeof(<typeparamref name="T" />) must be lesser than buffer size of reader.</typeparam>
    /// <param name="buffer">The buffer to read the items into.</param>
    /// <param name="numItems">Number of items in the buffer.</param>
    /// <returns>
    ///     Number of items read into the buffer.
    /// </returns>
    /// <remarks>
    ///     Prefer this method over any of the other methods if you intend on reading large amount of data.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadRaw<T>(T* buffer, int numItems) where T : unmanaged
    {
        T* result = ReadRaw<T>(numItems, out var available);
        var numBytes = sizeof(T) * numItems;
        Buffer.MemoryCopy(result, buffer, numBytes, numBytes);
        return available;
    }

    /// <summary>
    ///     Reads an unmanaged, generic type from the stream.
    /// </summary>
    /// <typeparam name="T">Type of value to read.</typeparam>
    /// <param name="value">The read in value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Read<T>(out T value) where T : unmanaged => value = Read<T>();

    /// <summary>
    ///     Reads a value that requires marshalling stream without incrementing the position.
    ///     Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
    /// </summary>
    /// <typeparam name="T">The type to read with marshalling.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T PeekMarshalled<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>()
    {
        var size = TypeInfo.MarshalledSizeOf<T>();
        if (CanRead(size))
            return Marshal.PtrToStructure<T>((nint)(void*)_gcHandlePtr + _bufferOffset)!;

        ReFillBuffer();
        var result2 = Marshal.PtrToStructure<T>((nint)(void*)_gcHandlePtr + _bufferOffset)!;
        return result2;
    }

    /// <summary>
    ///     Reads a managed or unmanaged generic type from the stream without incrementing the position.
    ///     Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
    /// </summary>
    /// <typeparam name="T">The type to read with marshalling.</typeparam>
    /// <param name="value">The read in value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PeekMarshalled<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
                                    DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>(out T value)
    {
        value = PeekMarshalled<T>();
    }

    /// <summary>
    ///     Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Peek<T>() where T : unmanaged
    {
        if (CanRead(sizeof(T)))
            return *(T*)(_gcHandlePtr + _bufferOffset);

        ReFillBuffer();
        return *(T*)(_gcHandlePtr + _bufferOffset);
    }

    /// <summary>
    ///     Reads an unmanaged, generic type from the stream without incrementing the position.
    /// </summary>
    /// <typeparam name="T">The value to peek from top of stream.</typeparam>
    /// <param name="value">The value to return.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Peek<T>(out T value) where T : unmanaged => value = Peek<T>();

    /// <summary>
    ///     Converts the stream to big endian.
    /// </summary>
    public BigEndianBufferedStreamReader<TStream> AsBigEndian() => this;

    /// <summary>
    ///     Converts the stream to little endian.
    /// </summary>
    public LittleEndianBufferedStreamReader<TStream> AsLittleEndian() => this;

    /// <summary>
    ///     Returns true if the <see cref="BufferedStreamReader{TStream}" /> has sufficient space buffered
    ///     to read memory of given size.
    /// </summary>
    /// <param name="size">The size of the item to check if can be read, in bytes.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CanRead(int size) => size <= BufferedBytesAvailable;

    /// <summary>
    ///     Refills the remainder of the buffer.
    ///     i.e. Preserves data to still be read (<see cref="BufferedBytesAvailable" />) and reads enough data to fill rest of
    ///     buffer.
    /// </summary>
    private void ReFillBuffer()
    {
        Buffer.MemoryCopy((void*)(_gcHandlePtr + _bufferOffset), (void*)_gcHandlePtr, BufferedBytesAvailable,
            BufferedBytesAvailable);
        FillBuffer(BufferedBytesAvailable);
    }

    /// <summary>
    ///     Fills the buffer with new data.
    /// </summary>
    /// <param name="bufferOffset">The offset in the buffer to start filling from.</param>
    private void FillBuffer(int bufferOffset)
    {
        BufferedBytesAvailable = BaseStream.ReadAtLeast(_buffer, bufferOffset, _buffer.Length - bufferOffset, false) +
                                 bufferOffset;
        _bufferOffset = 0;

        if (BufferedBytesAvailable >= _buffer.Length)
            return;

        IsEndOfStream = true;
        OnEndOfStream?.Invoke();
    }

    /// <summary>
    ///     Internal method that seeks the stream relatively to current position.
    /// </summary>
    /// <param name="relativeOffset">Offset which by to seek.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RelativeSeek(long relativeOffset)
    {
        // Fast Seek Manually inlined for perf.
        // Check if target is in current buffer, if not grab new.
        // Do not invert braces, optimised for hot paths.
        if (relativeOffset >= 0)
        {
            if (relativeOffset <= BufferedBytesAvailable)
            {
                _bufferOffset += (int)relativeOffset;
                BufferedBytesAvailable -= (int)relativeOffset;
                return;
            }
        }
        else
        {
            // Convert negative to positive: offset * -1
            if (relativeOffset * -1 <= _bufferOffset)
            {
                _bufferOffset += (int)relativeOffset;
                BufferedBytesAvailable -= (int)relativeOffset;
                return;
            }
        }

        // Slow path, entire buffer needs re-filling.
        SeekSlow(relativeOffset);
    }

    private void SeekSlow(long relativeOffset)
    {
        var targetPosition = Position + relativeOffset;
        var targetOffset = targetPosition - BaseStream.Position;
        BaseStream.Seek(targetOffset, SeekOrigin.Current);
        FillBuffer(0);
    }
}
