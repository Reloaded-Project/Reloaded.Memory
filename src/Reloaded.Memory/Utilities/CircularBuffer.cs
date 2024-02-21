using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     The <see cref="CircularBuffer" /> is a writeable buffer useful for temporary storage of data.<br /><br />
///     It's a buffer whereby once you reach the end of the buffer, it loops back over to the beginning of the stream
///     automatically.
/// </summary>
[Obsolete("This was accidentally shipped as 'class'. If you need non-heap allocating, please use: " + nameof(CircularBufferStruct))]
public unsafe class CircularBuffer
{
    private CircularBufferStruct _base;

    /// <summary>
    ///     The address of the <see cref="CircularBuffer" />.
    /// </summary>
    public nuint Start => _base.Start;

    /// <summary>
    ///     The address of the <see cref="CircularBuffer" />.
    /// </summary>
    public nuint End => _base.Current;

    /// <summary>
    ///     Address of the current item in the buffer.
    /// </summary>
    public nuint Current => _base.Current;

    /// <summary>
    ///     Remaining space in the buffer.
    /// </summary>
    public nuint Remaining => _base.Remaining;

    /// <summary>
    ///     The overall size of the buffer.
    /// </summary>
    public nuint Size => _base.Size;

    /// <summary>
    ///     Creates a <see cref="CircularBuffer" /> within the target memory source.
    /// </summary>
    /// <param name="start">The start of the buffer.</param>
    /// <param name="size">The size of the buffer in bytes.</param>
    public CircularBuffer(nuint start, int size) => _base = new CircularBufferStruct(start, size);

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
    public nuint Add(byte* data, uint length) => _base.Add(data, length);

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <param name="source">The source to write the contents to.</param>
    /// <param name="data">Address to the item to add.</param>
    /// <param name="length">Number of bytes to add.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    public nuint Add<TSource>(TSource source, byte* data, uint length) where TSource : ICanReadWriteMemory => _base.Add(source, data, length);

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="T">The type of item you wish to write to the buffer.</typeparam>
    /// <typeparam name="TSource">An implementation of <see cref="ICanReadWriteMemory" />.</typeparam>
    /// <param name="source">The source to write the contents to.</param>
    /// <param name="value">The value you wish to write to the buffer.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    /// <remarks>
    ///     This is a hot path for writing to the current process' RAM.
    ///     It is recommended to use the overload that accepts a TSource if you wish to write it somewhere else.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Add<TSource, T>(TSource source, T value) where T : unmanaged where TSource : ICanReadWriteMemory => _base.Add(source, value);

    /// <summary>
    ///     Adds a new item onto the circular buffer.
    /// </summary>
    /// <typeparam name="T">The type of item you wish to write to the buffer.</typeparam>
    /// <param name="value">The value you wish to write to the buffer.</param>
    /// <returns>Pointer to the recently added item to the buffer, or zero if the item cannot fit.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Add<T>(T value) where T : unmanaged => _base.Add(value);

    /// <summary>
    ///     Returns an enum describing if an item can fit into the buffer.
    /// </summary>
    /// <param name="itemSize">The size of the object to be appended to the buffer.</param>
    public ItemFit CanItemFit(uint itemSize) => (ItemFit)_base.CanItemFit(itemSize);

    /// <summary>
    ///     Returns an enum describing if an item can fit into the buffer.
    /// </summary>
    /// <typeparam name="T">Type of item to add to circular buffer.</typeparam>
    public ItemFit CanItemFit<T>() where T : unmanaged => (ItemFit)_base.CanItemFit<T>();

    /* Custom Types. */

    /// <summary>
    ///     Possible results for whether the item can fit.
    /// </summary>
    public enum ItemFit
    {
        /// <summary>
        ///     The item can fit into the buffer.
        /// </summary>
        Yes,

        /// <summary>
        ///     The item can fit into the buffer, but not in the remaining space (will be placed at start of buffer).
        /// </summary>
        StartOfBuffer,

        /// <summary>
        ///     The item is too large to fit into the buffer.
        /// </summary>
        No
    }
}
