using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     An interface shared by <see cref="LittleEndianReader" /> and <see cref="BigEndianReader" />.
/// </summary>
public interface IEndianReader
{
    /// <summary>
    ///     Reads a unsigned 8-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A unsigned 8-bit integer value.</returns>
    /// <remarks>This API exists simply for consistency.</remarks>
    byte ReadByte();

    /// <summary>
    ///     Reads a signed 8-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A signed 8-bit integer value.</returns>
    /// <remarks>This API exists simply for consistency.</remarks>
    sbyte ReadSByte();

    /// <summary>
    ///     Reads a signed 16-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A signed 16-bit integer value.</returns>
    short ReadShort();

    /// <summary>
    ///     Reads an unsigned 16-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>An unsigned 16-bit integer value.</returns>
    ushort ReadUShort();

    /// <summary>
    ///     Reads an unsigned 32-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>An unsigned 32-bit integer value.</returns>
    uint ReadUInt();

    /// <summary>
    ///     Reads a signed 32-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A signed 32-bit integer value.</returns>
    int ReadInt();

    /// <summary>
    ///     Reads an unsigned 64-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>An unsigned 64-bit integer value.</returns>
    ulong ReadULong();

    /// <summary>
    ///     Reads a signed 64-bit integer from the current pointer in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A signed 64-bit integer value.</returns>
    long ReadLong();

    /// <summary>
    ///     Reads a float in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A float.</returns>
    float ReadFloat();

    /// <summary>
    ///     Reads a double in Little Endian format and advances the pointer.
    /// </summary>
    /// <returns>A double.</returns>
    double ReadDouble();

    /// <summary>
    ///     Reads a unsigned 8-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A unsigned 8-bit integer value.</returns>
    byte ReadByteAtOffset(int offset);

    /// <summary>
    ///     Reads a signed 8-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A signed 8-bit integer value.</returns>
    sbyte ReadSByteAtOffset(int offset);

    /// <summary>
    ///     Reads a signed 16-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A signed 16-bit integer value.</returns>
    short ReadShortAtOffset(int offset);

    /// <summary>
    ///     Reads a unsigned 16-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A unsigned 16-bit integer value.</returns>
    ushort ReadUShortAtOffset(int offset);

    /// <summary>
    ///     Reads a signed 32-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A signed 32-bit integer value.</returns>
    int ReadIntAtOffset(int offset);

    /// <summary>
    ///     Reads a unsigned 32-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A unsigned 32-bit integer value.</returns>
    uint ReadUIntAtOffset(int offset);

    /// <summary>
    ///     Reads a signed 64-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A signed 64-bit integer value.</returns>
    long ReadLongAtOffset(int offset);

    /// <summary>
    ///     Reads an unsigned 64-bit integer from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>An unsigned 64-bit integer value.</returns>
    ulong ReadULongAtOffset(int offset);

    /// <summary>
    ///     Reads a float from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A float value.</returns>
    float ReadFloatAtOffset(int offset);

    /// <summary>
    ///     Reads a double from the specified offset in Little Endian format without advancing the pointer.
    /// </summary>
    /// <param name="offset">The offset in bytes from the current pointer.</param>
    /// <returns>A double value.</returns>
    double ReadDoubleAtOffset(int offset);

    /// <summary>
    ///     Advances the pointer by a specified number of bytes.
    /// </summary>
    /// <param name="offset">The number of bytes to advance the pointer by.</param>
    void Seek(int offset);

    /// <summary>
    ///     Reads into a supported structure.
    /// </summary>
    /// <param name="item">The item to read the values into.</param>
    /// <typeparam name="T">Type of item to read into.</typeparam>
    public void Read<T>(ref T item) where T : ICanBeReadByAnEndianReader;

    /// <summary>
    ///     Deserializes into provided structure.
    /// </summary>
    /// <typeparam name="T">Type of item to create.</typeparam>
    /// <returns>New instance of given structure.</returns>
    public T Read<T>() where T : unmanaged, ICanBeReadByAnEndianReader;
}
