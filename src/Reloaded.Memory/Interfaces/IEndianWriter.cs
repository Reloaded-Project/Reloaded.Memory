using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     An interface shared by <see cref="LittleEndianWriter" /> and <see cref="BigEndianWriter" />.
/// </summary>
public interface IEndianWriter
{
    /// <summary>
    ///     Writes a signed 8-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The signed 16-bit integer value to write.</param>
    void Write(sbyte value);

    /// <summary>
    ///     Writes an unsigned 8-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The unsigned 8-bit integer value to write.</param>
    void Write(byte value);

    /// <summary>
    ///     Writes a signed 16-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The signed 16-bit integer value to write.</param>
    void Write(short value);

    /// <summary>
    ///     Writes an unsigned 16-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The unsigned 16-bit integer value to write.</param>
    void Write(ushort value);

    /// <summary>
    ///     Writes an unsigned 32-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The unsigned 32-bit integer value to write.</param>
    void Write(uint value);

    /// <summary>
    ///     Writes a signed 32-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The signed 32-bit integer value to write.</param>
    void Write(int value);

    /// <summary>
    ///     Writes a signed 64-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The signed 64-bit integer value to write.</param>
    void Write(long value);

    /// <summary>
    ///     Writes an unsigned 64-bit integer value to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The unsigned 64-bit integer value to write.</param>
    void Write(ulong value);

    /// <summary>
    ///     Writes a float to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The float value to write.</param>
    void Write(float value);

    /// <summary>
    ///     Writes a double to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="value">The double value to write.</param>
    void Write(double value);

    /// <summary>
    ///     Writes a byte array to the current pointer and advances the pointer.
    /// </summary>
    /// <param name="data">The byte array to write.</param>
    void Write(Span<byte> data);

    /// <summary>
    ///     Writes a signed 8-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The signed 8-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(sbyte value, int offset);

    /// <summary>
    ///     Writes a unsigned 8-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The unsigned 8-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(byte value, int offset);

    /// <summary>
    ///     Writes a signed 16-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The signed 16-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(short value, int offset);

    /// <summary>
    ///     Writes a unsigned 16-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The unsigned 16-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(ushort value, int offset);

    /// <summary>
    ///     Writes a signed 32-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The signed 32-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(int value, int offset);

    /// <summary>
    ///     Writes a unsigned 32-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The unsigned 32-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(uint value, int offset);

    /// <summary>
    ///     Writes a signed 64-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The signed 64-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(long value, int offset);

    /// <summary>
    ///     Writes an unsigned 64-bit integer value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The unsigned 64-bit integer value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(ulong value, int offset);

    /// <summary>
    ///     Writes a float value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The float value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(float value, int offset);

    /// <summary>
    ///     Writes a double value to the specified offset without advancing the pointer.
    /// </summary>
    /// <param name="value">The double value to write.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    void WriteAtOffset(double value, int offset);

    /// <summary>
    ///     Advances the stream by a specified number of bytes.
    /// </summary>
    /// <param name="offset">The number of bytes to advance by.</param>
    void Seek(int offset);

    /// <summary>
    ///     Writes the item to the given endian writer.
    /// </summary>
    /// <typeparam name="T">The item to write.</typeparam>
    /// <param name="item">The item to write to the writer.</param>
    void Write<T>(in T item) where T : unmanaged, ICanWriteToAnEndianWriter;
}
