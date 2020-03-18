

using System;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Streams
{
    public partial class BufferedStreamReader : IDisposable
    {
        
        /// <summary>
        /// Reads an unmanaged Int16 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out Int16 value)
        {
            int size = sizeof(Int16);
            ReFillIfNecessary(size);

            value = *(Int16*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged Int16 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int16 ReadBigEndianPrimitiveInt16()
        {
            ReadBigEndianPrimitive(out Int16 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  Int16 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out Int16 value)
        {
            ReFillIfNecessary(sizeof(Int16));

            value = *(Int16*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  Int16 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int16 PeekBigEndianPrimitiveInt16()
        {
            PeekBigEndianPrimitive(out Int16 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged UInt16 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out UInt16 value)
        {
            int size = sizeof(UInt16);
            ReFillIfNecessary(size);

            value = *(UInt16*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged UInt16 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt16 ReadBigEndianPrimitiveUInt16()
        {
            ReadBigEndianPrimitive(out UInt16 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  UInt16 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out UInt16 value)
        {
            ReFillIfNecessary(sizeof(UInt16));

            value = *(UInt16*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  UInt16 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt16 PeekBigEndianPrimitiveUInt16()
        {
            PeekBigEndianPrimitive(out UInt16 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged Int32 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out Int32 value)
        {
            int size = sizeof(Int32);
            ReFillIfNecessary(size);

            value = *(Int32*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged Int32 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int32 ReadBigEndianPrimitiveInt32()
        {
            ReadBigEndianPrimitive(out Int32 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  Int32 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out Int32 value)
        {
            ReFillIfNecessary(sizeof(Int32));

            value = *(Int32*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  Int32 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int32 PeekBigEndianPrimitiveInt32()
        {
            PeekBigEndianPrimitive(out Int32 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged UInt32 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out UInt32 value)
        {
            int size = sizeof(UInt32);
            ReFillIfNecessary(size);

            value = *(UInt32*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged UInt32 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt32 ReadBigEndianPrimitiveUInt32()
        {
            ReadBigEndianPrimitive(out UInt32 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  UInt32 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out UInt32 value)
        {
            ReFillIfNecessary(sizeof(UInt32));

            value = *(UInt32*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  UInt32 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt32 PeekBigEndianPrimitiveUInt32()
        {
            PeekBigEndianPrimitive(out UInt32 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged Int64 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out Int64 value)
        {
            int size = sizeof(Int64);
            ReFillIfNecessary(size);

            value = *(Int64*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged Int64 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int64 ReadBigEndianPrimitiveInt64()
        {
            ReadBigEndianPrimitive(out Int64 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  Int64 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out Int64 value)
        {
            ReFillIfNecessary(sizeof(Int64));

            value = *(Int64*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  Int64 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Int64 PeekBigEndianPrimitiveInt64()
        {
            PeekBigEndianPrimitive(out Int64 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged UInt64 from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out UInt64 value)
        {
            int size = sizeof(UInt64);
            ReFillIfNecessary(size);

            value = *(UInt64*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged UInt64 from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt64 ReadBigEndianPrimitiveUInt64()
        {
            ReadBigEndianPrimitive(out UInt64 value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  UInt64 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out UInt64 value)
        {
            ReFillIfNecessary(sizeof(UInt64));

            value = *(UInt64*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  UInt64 from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe UInt64 PeekBigEndianPrimitiveUInt64()
        {
            PeekBigEndianPrimitive(out UInt64 value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged Single from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out Single value)
        {
            int size = sizeof(Single);
            ReFillIfNecessary(size);

            value = *(Single*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged Single from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Single ReadBigEndianPrimitiveSingle()
        {
            ReadBigEndianPrimitive(out Single value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  Single from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out Single value)
        {
            ReFillIfNecessary(sizeof(Single));

            value = *(Single*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  Single from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Single PeekBigEndianPrimitiveSingle()
        {
            PeekBigEndianPrimitive(out Single value);
            return value;
        }

        
        /// <summary>
        /// Reads an unmanaged Double from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void ReadBigEndianPrimitive(out Double value)
        {
            int size = sizeof(Double);
            ReFillIfNecessary(size);

            value = *(Double*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
		/// Reads an unmanaged Double from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Double ReadBigEndianPrimitiveDouble()
        {
            ReadBigEndianPrimitive(out Double value);
            return value;
        }

		/// <summary>
        /// Reads an unmanaged  Double from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe void PeekBigEndianPrimitive(out Double value)
        {
            ReFillIfNecessary(sizeof(Double));

            value = *(Double*)(_gcHandlePtr + _bufferOffset);
            value = Memory.Endian.Reverse(value);
        }

        /// <summary>
        /// Reads an unmanaged  Double from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		[ExcludeFromCodeCoverage]
        public unsafe Double PeekBigEndianPrimitiveDouble()
        {
            PeekBigEndianPrimitive(out Double value);
            return value;
        }

    }
}
