using System.Buffers;
using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     Instance of a rented array. Don't forget to dispose me please!
/// </summary>
public readonly struct ArrayRental : IDisposable
{
    /// <summary>
    ///     The underlying array for this rental.
    /// </summary>
    public byte[] Array { get; }

    /// <summary>
    ///     Returns the span for given rented array.
    /// </summary>
    public Span<byte> Span => Array.AsSpanFast();

    /// <summary>
    ///     Rents a provided number of bytes.
    /// </summary>
    /// <param name="numBytes">Minimum number of bytes to rent.</param>
    public ArrayRental(int numBytes) => Array = ArrayPool<byte>.Shared.Rent(numBytes);

    /// <inheritdoc />
    public void Dispose() => ArrayPool<byte>.Shared.Return(Array);
}

/// <summary>
///     Instance of a rented array of type T. Don't forget to dispose me please!
/// </summary>
/// <typeparam name="T">Type of item stored in this rental.</typeparam>
public readonly struct ArrayRental<T> : IDisposable
{
    /// <summary>
    ///     The underlying array for this rental.
    /// </summary>
    public T[] Array { get; }

    /// <summary>
    ///     Returns the span for given rented array.
    /// </summary>
    public Span<T> Span => Array.AsSpanFast();

    /// <summary>
    ///     Rents a provided number of bytes.
    /// </summary>
    /// <param name="numBytes">Minimum number of items to rent.</param>
    public ArrayRental(int numBytes) => Array = ArrayPool<T>.Shared.Rent(numBytes);

    /// <inheritdoc />
    public void Dispose() => ArrayPool<T>.Shared.Return(Array);
}
