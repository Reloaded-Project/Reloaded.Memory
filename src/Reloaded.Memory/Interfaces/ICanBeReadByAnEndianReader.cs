using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     This interface is a trait for items which can be read by <see cref="BigEndianReader" /> or
///     <see cref="LittleEndianReader" />.
/// </summary>
public interface ICanBeReadByAnEndianReader
{
    /// <summary>
    ///     Reads the data for this struct from Endian Reader.
    /// </summary>
    /// <param name="reader">The reader instance.</param>
    /// <typeparam name="TEndianReader">The endian reader itself.</typeparam>
    void Read<TEndianReader>(ref TEndianReader reader) where TEndianReader : IEndianReader;
}
