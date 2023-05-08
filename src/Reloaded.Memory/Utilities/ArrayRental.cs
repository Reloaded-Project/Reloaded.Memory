using System.Buffers;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     Instance of a rented array. Don't forget to dispose me please!
/// </summary>
public struct ArrayRental : IDisposable
{
    /// <summary>
    ///     The underlying array for this rental.
    /// </summary>
    public byte[] Array { get; }

    /// <summary>
    ///     Returns the span for given rented array.
    /// </summary>
    public Span<byte> Span => Array.AsSpan();

    /// <summary>
    ///     Rents a provided number of bytes.
    /// </summary>
    /// <param name="numBytes">Minimum number of bytes to rent.</param>
    public ArrayRental(int numBytes) => Array = ArrayPool<byte>.Shared.Rent(numBytes);

    /// <inheritdoc />
    public void Dispose() => ArrayPool<byte>.Shared.Return(Array);
}
