using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Readers
{
    /// <summary>
    /// A version of <see cref="EndianStreamReader"/> that reads data in Big Endian mode.
    /// </summary>
    public partial class BigEndianStreamReader : EndianStreamReader
    {
        /// <summary>
        /// Constructs a <see cref="EndianStreamReader"/> given an existing stream reader.
        /// </summary>
        public BigEndianStreamReader(BufferedStreamReader streamReader) : base(streamReader) { }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read<T>(out T value) => Reader.ReadBigEndianPrimitive(out value);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe T Read<T>() => Reader.ReadBigEndianPrimitive<T>();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek<T>(out T value) => Reader.PeekBigEndianPrimitive(out value);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe T Peek<T>() => Reader.PeekBigEndianPrimitive<T>();

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ReadStruct<T>(out T value) where T : unmanaged, IEndianReversible => Reader.ReadBigEndianStruct(out value);

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T ReadStruct<T>() where T : unmanaged, IEndianReversible => Reader.ReadBigEndianStruct<T>();

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output without incrementing the position.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void PeekStruct<T>(out T value) where T : unmanaged, IEndianReversible => Reader.PeekBigEndianStruct(out value);

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output without incrementing the position.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T PeekStruct<T>() where T : unmanaged, IEndianReversible => Reader.PeekBigEndianStruct<T>();
    }
}
