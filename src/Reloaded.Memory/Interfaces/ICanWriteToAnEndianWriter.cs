using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     This interface is a trait for items which can be read by <see cref="BigEndianReader" /> or
///     <see cref="LittleEndianReader" />.
/// </summary>
public interface ICanWriteToAnEndianWriter
{
    /// <summary>
    ///     Writes data from this struct to an endian writer.
    /// </summary>
    /// <param name="reader">The writer instance.</param>
    /// <typeparam name="TEndianWriter">The endian writer itself.</typeparam>
    void Write<TEndianWriter>(ref TEndianWriter reader) where TEndianWriter : IEndianWriter;
}
