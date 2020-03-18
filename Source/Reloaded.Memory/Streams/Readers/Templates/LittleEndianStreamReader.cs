


using System;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Streams.Readers
{
    public partial class LittleEndianStreamReader
    {

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int16 value) => value = Reader.Read<Int16>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int16 ReadInt16() => Reader.Read<Int16>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int16 value) => value = Reader.Peek<Int16>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int16 PeekInt16() => Reader.Peek<Int16>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt16 value) => value = Reader.Read<UInt16>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt16 ReadUInt16() => Reader.Read<UInt16>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt16 value) => value = Reader.Peek<UInt16>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt16 PeekUInt16() => Reader.Peek<UInt16>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int32 value) => value = Reader.Read<Int32>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int32 ReadInt32() => Reader.Read<Int32>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int32 value) => value = Reader.Peek<Int32>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int32 PeekInt32() => Reader.Peek<Int32>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt32 value) => value = Reader.Read<UInt32>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt32 ReadUInt32() => Reader.Read<UInt32>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt32 value) => value = Reader.Peek<UInt32>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt32 PeekUInt32() => Reader.Peek<UInt32>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Int64 value) => value = Reader.Read<Int64>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int64 ReadInt64() => Reader.Read<Int64>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Int64 value) => value = Reader.Peek<Int64>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Int64 PeekInt64() => Reader.Peek<Int64>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out UInt64 value) => value = Reader.Read<UInt64>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt64 ReadUInt64() => Reader.Read<UInt64>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out UInt64 value) => value = Reader.Peek<UInt64>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe UInt64 PeekUInt64() => Reader.Peek<UInt64>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Single value) => value = Reader.Read<Single>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Single ReadSingle() => Reader.Read<Single>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Single value) => value = Reader.Peek<Single>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Single PeekSingle() => Reader.Peek<Single>();
		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Read(out Double value) => value = Reader.Read<Double>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Double ReadDouble() => Reader.Read<Double>();

		/// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe void Peek(out Double value) => value = Reader.Peek<Double>();

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override unsafe Double PeekDouble() => Reader.Peek<Double>();
	}
}