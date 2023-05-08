using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     The <see cref="CircularBuffer" /> is a writeable buffer useful for temporary storage of data.<br/><br/>
///     It's a buffer whereby once you reach the end of the buffer, it loops back over to the beginning of the stream
///     automatically.
/// </summary>
public unsafe class CircularBuffer
{
    /// <summary>
    ///     The address of the <see cref="CircularBuffer" />.
    /// </summary>
    public nuint Start => _start;

    /// <summary>
    ///     The address of the <see cref="CircularBuffer" />.
    /// </summary>
    public nuint End => _end;

    /// <summary>
    ///     Address of the current item in the buffer.
    /// </summary>
    public nuint Current
    {
        get => _current;
        private set => _current = value;
    }

    /// <summary>
    /// Remaining space in the buffer.
    /// </summary>
    public nuint Remaining => _end - _current;

    /// <summary>
    /// The overall size of the buffer.
    /// </summary>
    public nuint Size => _end - _start;

    private readonly nuint _start;
    private readonly nuint _end;
    private nuint _current; // for faster access.

    /// <summary>
    ///     Creates a <see cref="CircularBuffer" /> within the target memory source.
    /// </summary>
    /// <param name="start">The start of the buffer.</param>
    /// <param name="size">The size of the buffer in bytes.</param>
    public CircularBuffer(nuint start, int size)
    {
        _start = start;
        _end = (nuint)((byte*)start + size);
        Current = start;
    }

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <param name="data">Address to the item to add.</param>
    /// <param name="length">Number of bytes to add.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    /// <remarks>
    ///     This is a hot path for writing to the current process' RAM.
    ///     It is recommended to use the overload that accepts a TSource if you wish to write it somewhere else.
    /// </remarks>
    public nuint Add(byte* data, uint length)
    {
        var remaining = (uint)(_end - _current);
        if (length <= remaining)
        {
            // Hot path, item can be written to buffer.
            Buffer.MemoryCopy(data, (void*)_current, remaining, length);
            var result = _current;
            _current += length;
            return result;
        }

        var size = _end - _start;
        if (size >= length)
        {
            // Item fits but we need to write to buffer start.
            Buffer.MemoryCopy(data, (void*)_start, size, length);
            var result = _start;
            _current = _start + length;
            return result;
        }

        return UIntPtr.Zero;
    }

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory"/>.</typeparam>
    /// <param name="source">The source to write the contents to.</param>
    /// <param name="data">Address to the item to add.</param>
    /// <param name="length">Number of bytes to add.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    public nuint Add<TSource>(TSource source, byte* data, uint length) where TSource : ICanReadWriteMemory
    {
        if (length <= Remaining)
        {
            // Hot path, item can be written to buffer.
            source.WriteRaw(_current, new Span<byte>(data, (int)length));
            var result = _current;
            _current += length;
            return result;
        }

        var size = _end - _start;
        if (size >= length)
        {
            // Item fits but we need to write to buffer start.
            source.WriteRaw(_start, new Span<byte>(data, (int)length));
            var result = _start;
            _current = _start + length;
            return result;
        }

        return UIntPtr.Zero;
    }

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="T">The type of item you wish to write to the buffer.</typeparam>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory"/>.</typeparam>
    /// <param name="source">The source to write the contents to.</param>
    /// <param name="value">The value you wish to write to the buffer.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    /// <remarks>
    ///     This is a hot path for writing to the current process' RAM.
    ///     It is recommended to use the overload that accepts a TSource if you wish to write it somewhere else.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Add<TSource, T>(TSource source, T value) where T : unmanaged where TSource : ICanReadWriteMemory
    {
        // These will use SIMD copy on most architecture & runtimes.
        if (sizeof(T) <= 32)
            return AddFast(source, value);

        return Add(source, (byte*)&value, (uint)sizeof(T));
    }

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="T">The type of item you wish to write to the buffer.</typeparam>
    /// <param name="value">The value you wish to write to the buffer.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Add<T>(T value) where T : unmanaged
    {
        // These will use SIMD copy on most architecture & runtimes.
        if (sizeof(T) <= 32)
            return AddFast(value);

        return Add((byte*)&value, (uint)sizeof(T));
    }

    #region Optimized Methods for Common Types
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal nuint AddFast<T>(T value) where T : unmanaged
    {
        if ((nuint)sizeof(T) <= Remaining)
        {
            // Hot path, item can be written to buffer.
            *(T*)_current = value;
            var result = _current;
            _current += (nuint)sizeof(T);
            return result;
        }

        if (Size >= (nuint)sizeof(T))
        {
            // Item fits but we need to write to buffer start.
            *(T*)_start = value;
            var result = _start;
            _current = result + (nuint)sizeof(T);
            return result;
        }

        // Rare case.
        return UIntPtr.Zero;
    }

    internal nuint AddFast<TSource, T>(TSource source, T value) where T : unmanaged where TSource : ICanReadWriteMemory
    {
        if ((nuint)sizeof(T) <= Remaining)
        {
            // Hot path, item can be written to buffer.
            source.Write(_current, value);
            var result = _current;
            _current += (nuint)sizeof(T);
            return result;
        }

        if (Size >= (nuint)sizeof(T))
        {
            // Item fits but we need to write to buffer start.
            source.Write(_start, value);
            var result = _start;
            _current = result + (nuint)sizeof(T);
            return result;
        }

        // Rare case.
        return UIntPtr.Zero;
    }
    #endregion

    /// <summary>
    ///     Returns an enumerable describing if an item can fit into the buffer.
    /// </summary>
    /// <param name="itemSize">The size of the object to be appended to the buffer.</param>
    public ItemFit CanItemFit(uint itemSize)
    {
        if (itemSize <= Remaining)
            return ItemFit.Yes;

        if (Size >= itemSize)
            return ItemFit.StartOfBuffer;

        return ItemFit.No;
    }

    /// <summary>
    ///     Returns an enumerable describing if an item can fit into the buffer.
    /// </summary>
    /// <typeparam name="T">Type of item to add to circular buffer.</typeparam>
    public ItemFit CanItemFit<T>() where T : unmanaged
    {
        if (sizeof(T) <= (int)Remaining)
            return ItemFit.Yes;

        if (Size >= (nuint)sizeof(T))
            return ItemFit.StartOfBuffer;

        return ItemFit.No;
    }

    /* Custom Types. */

    /// <summary>
    /// Possible results for whether the item can fit.
    /// </summary>
    public enum ItemFit
    {
        /// <summary>
        /// The item can fit into the buffer.
        /// </summary>
        Yes,

        /// <summary>
        /// The item can fit into the buffer, but not in the remaining space (will be placed at start of buffer).
        /// </summary>
        StartOfBuffer,

        /// <summary>
        /// The item is too large to fit into the buffer.
        /// </summary>
        No,
    }
}
