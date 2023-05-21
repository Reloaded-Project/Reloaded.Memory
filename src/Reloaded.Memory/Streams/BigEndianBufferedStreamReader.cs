﻿using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Streams;

/// <summary>
///     Implementation of <see cref="BufferedStreamReader{TStream}" /> that delegates the underlying method calls as Big
///     Endian.
/// </summary>
/// <typeparam name="TStream">Type of underlying stream.</typeparam>
public struct BigEndianBufferedStreamReader<TStream> : IEndianedBufferStreamReader<TStream> where TStream : Stream
{
    private BufferedStreamReader<TStream> _impl;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public TStream BaseStream => _impl.BaseStream;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public int BufferedBytesAvailable => _impl.BufferedBytesAvailable;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public int CurrentBufferSize => _impl.CurrentBufferSize;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public bool IsEndOfStream => _impl.IsEndOfStream;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public long Position => _impl.Position;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public void Seek(long offset, SeekOrigin origin) => _impl.Seek(offset, origin);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public void Advance(long offset) => _impl.Advance(offset);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public int ReadBytesUnbuffered(long offset, Span<byte> data) => _impl.ReadBytesUnbuffered(offset, data);

    /// <inheritdoc />
    public T ReadStruct<T>() where T : unmanaged, ICanReverseEndian => _impl.ReadBigEndianStruct<T>();

    /// <inheritdoc />
    public void ReadStruct<T>(out T result) where T : unmanaged, ICanReverseEndian => result = ReadStruct<T>();

    /// <inheritdoc />
    public T PeekStruct<T>() where T : unmanaged, ICanReverseEndian => _impl.PeekBigEndianStruct<T>();

    /// <inheritdoc />
    public void PeekStruct<T>(out T result) where T : unmanaged, ICanReverseEndian => result = PeekStruct<T>();

    /// <inheritdoc />
    public byte ReadByte() => _impl.Read<byte>();

    /// <inheritdoc />
    public sbyte ReadSByte() => _impl.Read<sbyte>();

    /// <inheritdoc />
    public short ReadInt16() => _impl.ReadBigEndianInt16();

    /// <inheritdoc />
    public ushort ReadUInt16() => _impl.ReadBigEndianUInt16();

    /// <inheritdoc />
    public int ReadInt32() => _impl.ReadBigEndianInt32();

    /// <inheritdoc />
    public uint ReadUInt32() => _impl.ReadBigEndianUInt32();

    /// <inheritdoc />
    public long ReadInt64() => _impl.ReadBigEndianInt64();

    /// <inheritdoc />
    public ulong ReadUInt64() => _impl.ReadBigEndianUInt64();

    /// <inheritdoc />
    public float ReadSingle() => _impl.ReadBigEndianSingle();

    /// <inheritdoc />
    public double ReadDouble() => _impl.ReadBigEndianDouble();

    /// <inheritdoc />
    public void Read(out byte value) => value = ReadByte();

    /// <inheritdoc />
    public void Read(out sbyte value) => value = ReadSByte();

    /// <inheritdoc />
    public void Read(out short value) => value = ReadInt16();

    /// <inheritdoc />
    public void Read(out ushort value) => value = ReadUInt16();

    /// <inheritdoc />
    public void Read(out int value) => value = ReadInt32();

    /// <inheritdoc />
    public void Read(out uint value) => value = ReadUInt32();

    /// <inheritdoc />
    public void Read(out long value) => value = ReadInt64();

    /// <inheritdoc />
    public void Read(out ulong value) => value = ReadUInt64();

    /// <inheritdoc />
    public void Read(out float value) => value = ReadSingle();

    /// <inheritdoc />
    public void Read(out double value) => value = ReadDouble();

    /// <inheritdoc />
    public byte PeekByte() => _impl.Peek<byte>();

    /// <inheritdoc />
    public sbyte PeekSByte() => _impl.Peek<sbyte>();

    /// <inheritdoc />
    public short PeekInt16() => _impl.PeekBigEndianInt16();

    /// <inheritdoc />
    public ushort PeekUInt16() => _impl.PeekBigEndianUInt16();

    /// <inheritdoc />
    public int PeekInt32() => _impl.PeekBigEndianInt32();

    /// <inheritdoc />
    public uint PeekUInt32() => _impl.PeekBigEndianUInt32();

    /// <inheritdoc />
    public long PeekInt64() => _impl.PeekBigEndianInt64();

    /// <inheritdoc />
    public ulong PeekUInt64() => _impl.PeekBigEndianUInt64();

    /// <inheritdoc />
    public float PeekSingle() => _impl.PeekBigEndianSingle();

    /// <inheritdoc />
    public double PeekDouble() => _impl.PeekBigEndianDouble();

    /// <inheritdoc />
    public void Peek(out byte value) => value = PeekByte();

    /// <inheritdoc />
    public void Peek(out sbyte value) => value = PeekSByte();

    /// <inheritdoc />
    public void Peek(out short value) => value = PeekInt16();

    /// <inheritdoc />
    public void Peek(out ushort value) => value = PeekUInt16();

    /// <inheritdoc />
    public void Peek(out int value) => value = PeekInt32();

    /// <inheritdoc />
    public void Peek(out uint value) => value = PeekUInt32();

    /// <inheritdoc />
    public void Peek(out long value) => value = PeekInt64();

    /// <inheritdoc />
    public void Peek(out ulong value) => value = PeekUInt64();

    /// <inheritdoc />
    public void Peek(out float value) => value = PeekSingle();

    /// <inheritdoc />
    public void Peek(out double value) => value = PeekDouble();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public unsafe byte* ReadRaw(int length, out int available) => _impl.ReadRaw(length, out available);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public unsafe T* ReadRaw<T>(int numItems, out int available) where T : unmanaged
        => _impl.ReadRaw<T>(numItems, out available);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public int ReadRaw<T>(Span<T> buffer) where T : unmanaged => _impl.ReadRaw(buffer);

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public unsafe int ReadRaw<T>(T* buffer, int numItems) where T : unmanaged => _impl.ReadRaw(buffer, numItems);

    /// <summary>
    ///     Implicitly converts a <see cref="BigEndianBufferedStreamReader{TStream}" /> to a
    ///     <see cref="BufferedStreamReader{TStream}" />.
    /// </summary>
    /// <param name="reader">The <see cref="BigEndianBufferedStreamReader{TStream}" /> to convert.</param>
    public static implicit operator BufferedStreamReader<TStream>(BigEndianBufferedStreamReader<TStream> reader)
        => reader._impl;

    /// <summary>
    ///     Implicitly converts a <see cref="BufferedStreamReader{TStream}" /> to a
    ///     <see cref="BigEndianBufferedStreamReader{TStream}" />.
    /// </summary>
    /// <param name="reader">The <see cref="BufferedStreamReader{TStream}" /> to convert.</param>
    public static implicit operator BigEndianBufferedStreamReader<TStream>(BufferedStreamReader<TStream> reader)
        => new() { _impl = reader };
}
