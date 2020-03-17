

using System;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Readers
{
    /// <summary>
    /// An abstract class that abstracts <see cref="BufferedStreamReader"/>, allowing for individual implementations for each endian.
    /// </summary>
    public abstract partial class EndianStreamReader
    {

        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out Int16 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int16 ReadInt16();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out Int16 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int16 PeekInt16();
        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out UInt16 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt16 ReadUInt16();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out UInt16 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt16 PeekUInt16();
        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out Int32 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int32 ReadInt32();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out Int32 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int32 PeekInt32();
        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out UInt32 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt32 ReadUInt32();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out UInt32 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt32 PeekUInt32();
        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out Int64 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int64 ReadInt64();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out Int64 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe Int64 PeekInt64();
        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read(out UInt64 value);

		/// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt64 ReadUInt64();

		/// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek(out UInt64 value);

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe UInt64 PeekUInt64();
        






    }
}