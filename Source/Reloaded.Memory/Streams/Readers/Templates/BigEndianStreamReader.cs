


using System;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Readers
{
    public partial class BigEndianStreamReader
    {

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int16 value) => value = Reader.ReadBigEndianPrimitiveInt16();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int16 ReadInt16() => Reader.ReadBigEndianPrimitiveInt16();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int16 value) => value = Reader.PeekBigEndianPrimitiveInt16();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int16 PeekInt16() => Reader.PeekBigEndianPrimitiveInt16();
		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt16 value) => value = Reader.ReadBigEndianPrimitiveUInt16();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt16 ReadUInt16() => Reader.ReadBigEndianPrimitiveUInt16();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt16 value) => value = Reader.PeekBigEndianPrimitiveUInt16();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt16 PeekUInt16() => Reader.PeekBigEndianPrimitiveUInt16();
		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int32 value) => value = Reader.ReadBigEndianPrimitiveInt32();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int32 ReadInt32() => Reader.ReadBigEndianPrimitiveInt32();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int32 value) => value = Reader.PeekBigEndianPrimitiveInt32();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int32 PeekInt32() => Reader.PeekBigEndianPrimitiveInt32();
		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt32 value) => value = Reader.ReadBigEndianPrimitiveUInt32();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt32 ReadUInt32() => Reader.ReadBigEndianPrimitiveUInt32();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt32 value) => value = Reader.PeekBigEndianPrimitiveUInt32();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt32 PeekUInt32() => Reader.PeekBigEndianPrimitiveUInt32();
		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int64 value) => value = Reader.ReadBigEndianPrimitiveInt64();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int64 ReadInt64() => Reader.ReadBigEndianPrimitiveInt64();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int64 value) => value = Reader.PeekBigEndianPrimitiveInt64();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int64 PeekInt64() => Reader.PeekBigEndianPrimitiveInt64();
		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt64 value) => value = Reader.ReadBigEndianPrimitiveUInt64();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt64 ReadUInt64() => Reader.ReadBigEndianPrimitiveUInt64();

		/// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt64 value) => value = Reader.PeekBigEndianPrimitiveUInt64();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt64 PeekUInt64() => Reader.PeekBigEndianPrimitiveUInt64();
	}
}