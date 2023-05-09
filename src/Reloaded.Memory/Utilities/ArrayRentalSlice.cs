using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     Represents a slice of an <see cref="ArrayRental" />.
///     This API is meant to be used as a return value from methods, and transfers control of the rental from the internal
///     <see cref="ArrayRental"/>.
/// </summary>
public struct ArrayRentalSlice : IDisposable
{
    /// <summary>
    ///     The underlying rental.
    /// </summary>
    public ArrayRental Rental { get; }

    /// <summary>
    ///     Returns the span for the rented slice.
    /// </summary>
    public Span<byte> Span => Rental.Array.AsSpanFast(0, Length);

    /// <summary>
    ///     Length of this slice.
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     Represents a slice of the array rental.
    /// </summary>
    /// <param name="rental">The underlying rental.</param>
    /// <param name="length">Length of the underlying rental.</param>
    public ArrayRentalSlice(ArrayRental rental, int length)
    {
        Rental = rental;
        Length = length;
    }

    /// <inheritdoc />
    public void Dispose() => Rental.Dispose();
}
