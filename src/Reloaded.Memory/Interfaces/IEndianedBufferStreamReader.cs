using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Interfaces;

/// <summary>
///     An implementation of <see cref="BufferedStreamReader{TStream}" /> tied to a specific endian.
/// </summary>
public interface IEndianedBufferStreamReader
{
    /// <summary>
    ///     Gets the remaining number of bytes that are currently buffered.
    /// </summary>
    [ExcludeFromCodeCoverage]
    int BufferedBytesAvailable { get; }

    /// <summary>
    ///     Gets the total size of the current buffered data at this moment in time.
    /// </summary>
    [ExcludeFromCodeCoverage]
    int CurrentBufferSize { get; }

    /// <summary>
    ///     This is true if end of stream was reached while refilling the internal buffer.
    /// </summary>
    [ExcludeFromCodeCoverage]
    bool IsEndOfStream { get; }

    /// <summary>
    ///     Gets the current position of the buffer.
    /// </summary>
    [ExcludeFromCodeCoverage]
    long Position { get; }

    /// <summary>
    ///     Seeks the underlying stream to a specified position.
    /// </summary>
    /// <param name="offset">Offset of the origin.</param>
    /// <param name="origin">Where to seek the stream from.</param>
    /// <remarks>
    ///     For performance reasons, prefer <see cref="BufferedStreamReader{TStream}.Advance" /> if you intend to seek from
    ///     current position.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    void Seek(long offset, SeekOrigin origin);

    /// <summary>
    ///     Advances the underlying stream by a specified number of bytes.
    /// </summary>
    /// <param name="offset">Offset of the origin.</param>
    [ExcludeFromCodeCoverage]
    void Advance(long offset);

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
    [ExcludeFromCodeCoverage]
    int ReadBytesUnbuffered(long offset, Span<byte> data);

    /// <summary>
    ///     Reads a generic struct <typeparamref name="T" /> from the stream.
    /// </summary>
    /// <typeparam name="T">Type of struct to read which inherits from <see cref="ICanReverseEndian" />.</typeparam>
    /// <returns>The struct from the stream.</returns>
    T ReadStruct<T>() where T : unmanaged, ICanReverseEndian;

    /// <summary>
    ///     Reads a generic struct <typeparamref name="T" /> from the stream.
    /// </summary>
    /// <typeparam name="T">Type of struct to read which inherits from <see cref="ICanReverseEndian" />.</typeparam>
    /// <param name="result">The result of the read operation.</param>
    /// <returns>The struct from the stream.</returns>
    void ReadStruct<T>(out T result) where T : unmanaged, ICanReverseEndian;

    /// <summary>
    ///     Reads a generic struct <typeparamref name="T" /> from the stream without advancing the stream.
    /// </summary>
    /// <typeparam name="T">Type of struct to read which inherits from <see cref="ICanReverseEndian" />.</typeparam>
    /// <returns>The struct from the stream.</returns>
    T PeekStruct<T>() where T : unmanaged, ICanReverseEndian;

    /// <summary>
    ///     Reads a generic struct <typeparamref name="T" /> from the stream without advancing the stream.
    /// </summary>
    /// <typeparam name="T">Type of struct to read which inherits from <see cref="ICanReverseEndian" />.</typeparam>
    /// <param name="result">The result of the read operation.</param>
    /// <returns>The struct from the stream.</returns>
    void PeekStruct<T>(out T result) where T : unmanaged, ICanReverseEndian;

    /// <summary>
    ///     Reads a byte from the stream.
    /// </summary>
    /// <returns>A single byte from the stream.</returns>
    byte ReadByte();

    /// <summary>
    ///     Reads a signed byte from the stream.
    /// </summary>
    /// <returns>A single byte from the stream.</returns>
    sbyte ReadSByte();

    /// <summary>
    ///     Reads an int16 from the stream.
    /// </summary>
    /// <returns>A single int16 from the stream.</returns>
    short ReadInt16();

    /// <summary>
    ///     Reads an uint16 from the stream.
    /// </summary>
    /// <returns>A single uint16 from the stream.</returns>
    ushort ReadUInt16();

    /// <summary>
    ///     Reads an int32 from the stream.
    /// </summary>
    /// <returns>A single int32 from the stream.</returns>
    int ReadInt32();

    /// <summary>
    ///     Reads an uint32 from the stream.
    /// </summary>
    /// <returns>A single uint32 from the stream.</returns>
    uint ReadUInt32();

    /// <summary>
    ///     Reads an int64 from the stream.
    /// </summary>
    /// <returns>A single int64 from the stream.</returns>
    long ReadInt64();

    /// <summary>
    ///     Reads an uint64 from the stream.
    /// </summary>
    /// <returns>A single uint64 from the stream.</returns>
    ulong ReadUInt64();

    /// <summary>
    ///     Reads a float from the stream.
    /// </summary>
    /// <returns>A single float from the stream.</returns>
    float ReadSingle();

    /// <summary>
    ///     Reads a double from the stream.
    /// </summary>
    /// <returns>A single double from the stream.</returns>
    double ReadDouble();

    /// <summary>
    ///     Reads a byte from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out byte value);

    /// <summary>
    ///     Reads a signed byte from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out sbyte value);

    /// <summary>
    ///     Reads an int16 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out short value);

    /// <summary>
    ///     Reads an uint16 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out ushort value);

    /// <summary>
    ///     Reads an int32 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out int value);

    /// <summary>
    ///     Reads an uint32 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out uint value);

    /// <summary>
    ///     Reads an int64 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out long value);

    /// <summary>
    ///     Reads an uint64 from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out ulong value);

    /// <summary>
    ///     Reads a float from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out float value);

    /// <summary>
    ///     Reads a double from the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Read(out double value);

    /// <summary>
    ///     Reads a byte from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single byte from the stream.</returns>
    byte PeekByte();

    /// <summary>
    ///     Reads a signed byte from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single byte from the stream.</returns>
    sbyte PeekSByte();

    /// <summary>
    ///     Reads an int16 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single int16 from the stream.</returns>
    short PeekInt16();

    /// <summary>
    ///     Reads an uint16 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single uint16 from the stream.</returns>
    ushort PeekUInt16();

    /// <summary>
    ///     Reads an int32 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single int32 from the stream.</returns>
    int PeekInt32();

    /// <summary>
    ///     Reads an uint32 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single uint32 from the stream.</returns>
    uint PeekUInt32();

    /// <summary>
    ///     Reads an int64 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single int64 from the stream.</returns>
    long PeekInt64();

    /// <summary>
    ///     Reads an uint64 from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single uint64 from the stream.</returns>
    ulong PeekUInt64();

    /// <summary>
    ///     Reads a float from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single float from the stream.</returns>
    float PeekSingle();

    /// <summary>
    ///     Reads a double from the stream without advancing the stream.
    /// </summary>
    /// <returns>A single double from the stream.</returns>
    double PeekDouble();

    /// <summary>
    ///     Reads a byte from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out byte value);

    /// <summary>
    ///     Reads a signed byte from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out sbyte value);

    /// <summary>
    ///     Reads an int16 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out short value);

    /// <summary>
    ///     Reads an uint16 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out ushort value);

    /// <summary>
    ///     Reads an int32 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out int value);

    /// <summary>
    ///     Reads an uint32 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out uint value);

    /// <summary>
    ///     Reads an int64 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out long value);

    /// <summary>
    ///     Reads an uint64 from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out ulong value);

    /// <summary>
    ///     Reads a float from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out float value);

    /// <summary>
    ///     Reads a double from the stream without advancing the stream.
    /// </summary>
    /// <param name="value">The parameter to receive the value.</param>
    void Peek(out double value);

    /// <summary>
    ///     Reads raw data from the stream, without conversion to target endian.
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
    [ExcludeFromCodeCoverage]
    unsafe byte* ReadRaw(int length, out int available);

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
    [ExcludeFromCodeCoverage]
    unsafe T* ReadRaw<T>(int numItems, out int available) where T : unmanaged;

    /// <summary>
    ///     Reads a number of items into a span of bytes, without endian conversion.
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
    [ExcludeFromCodeCoverage]
    int ReadRaw<T>(Span<T> buffer) where T : unmanaged;

    /// <summary>
    ///     Reads a number of items into a span of bytes, without endian conversion.
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
    [ExcludeFromCodeCoverage]
    unsafe int ReadRaw<T>(T* buffer, int numItems) where T : unmanaged;
}

/// <summary>
///     An implementation of <see cref="BufferedStreamReader{TStream}" /> tied to a specific endian.
/// </summary>
/// <typeparam name="TStream">Type of stream underlying the <see cref="BufferedStreamReader{TStream}" />.</typeparam>
public interface IEndianedBufferStreamReader<out TStream> : IEndianedBufferStreamReader where TStream : Stream
{
    /// <summary>
    ///     Gets the stream this class was instantiated with.
    /// </summary>
    [ExcludeFromCodeCoverage]
    TStream BaseStream { get; }
}
