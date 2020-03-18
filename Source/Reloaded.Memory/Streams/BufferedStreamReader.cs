using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Reloaded.Memory.Streams
{
    /// <summary>
    /// A custom <see cref="BinaryReader"/> tuned for performance supporting the read of generics from another stream without unnecessary
    /// sanitization checks. Maintains a simple buffering layer to speed up read and write operations over slow streams.
    ///
    /// Limitation: Class cannot read structs larger than buffer size. This is not checked for!
    /// </summary>
    public partial class BufferedStreamReader : IDisposable
    {
        private readonly Stream _stream;
        private readonly byte[] _buffer;
        private readonly int _bufferSize;

        private int _bufferOffset;            // Read offset.
        private int _bufferedBytesRemaining;  // Number of bytes unread.

        /* Pointers */
        private GCHandle _gcHandle;
        private IntPtr _gcHandlePtr;
        private Sources.Memory _memory = new Sources.Memory();

        /* Properties (Written as methods to force inlining). */

        /// <summary>
        /// Gets the stream this class was instantiated with.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stream BaseStream() { return _stream; }

        /// <summary>
        /// Gets the remaining number of bytes that are currently buffered.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BufferBytesAvailable() { return _bufferedBytesRemaining; }

        /// <summary>
        /// Gets the total size of the current buffered data at this moment in time.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CurrentBufferSize() { return _bufferOffset + _bufferedBytesRemaining; } 

        /// <summary>
        /// The current position of the buffer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Position() { return _stream.Position - _bufferedBytesRemaining; }

        private BufferedStreamReader() { }

        /// <summary>
        /// Constructs a <see cref="BufferedStreamReader"/>.
        /// </summary>
        /// <param name="stream">The stream to add buffering capabilities to.</param>
        /// <param name="bufferSize">
        ///     The size of the buffer.
        ///     Benchmarking suggests 65536 to be an optimal value for <see cref="FileStream"/> and 512/2048 optimal for <see cref="MemoryStream"/>.
        ///     Note: With the exception of <see cref="ReadBytes"/>, you may only use <see cref="Read{T}(out T)"/> for structs smaller than the buffer size.
        /// </param>
        public BufferedStreamReader(Stream stream, int bufferSize)
        {
            _stream = stream;
            _bufferSize = bufferSize;
            _buffer = new byte[bufferSize];
            _bufferOffset = bufferSize;

            _gcHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            _gcHandlePtr = _gcHandle.AddrOfPinnedObject();
        }

        /// <summary/>
        ~BufferedStreamReader()
        {
            this.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _gcHandle.Free();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Seeks the underlying stream to a specified position.
        /// </summary>
        public void Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Current)
            {
                RelativeSeek(offset);
            }
            else if (origin == SeekOrigin.Begin)
            {
                long relativeOffset = offset - Position();
                RelativeSeek(relativeOffset);
            }
            else if (origin == SeekOrigin.End)
            {
                long seekTarget = (_stream.Length - offset);
                long relativeOffset = seekTarget - Position();
                RelativeSeek(relativeOffset);
            }
        }

        /// <summary>
        /// Reads a specified amount of bytes at a specific offset from the underlying stream without
        /// resetting the buffers or advancing the read pointer.
        ///
        /// Note: This method is intended only for reading of large size raw data e.g. compressed file data in an archive.
        ///       No optimizations are performed for this action.
        /// </summary>
        /// <param name="offset">The offset of the data from the start of the stream.</param>
        /// <param name="count">The amount of bytes to read.</param>
        public byte[] ReadBytes(long offset, int count)
        {
            long originalPosition = _stream.Position;

            _stream.Position = offset;
            byte[] output = new byte[count];
            _stream.Read(output, 0, count);

            _stream.Position = originalPosition;

            return output;
        }

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="value">The value to output.</param>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Read<T>(out T value, bool marshal)
        {
            int size = Struct.GetSize<T>(marshal);
            ReFillIfNecessary(size);

            _memory.Read(_gcHandlePtr + _bufferOffset, out value, marshal);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Read<T>(out T value) where T : unmanaged
        {
            int size = sizeof(T);
            ReFillIfNecessary(size);

            value = *(T*)(_gcHandlePtr + _bufferOffset);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>(bool marshal)
        {
            Read(out T value, marshal);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T Read<T>() where T : unmanaged
        {
            Read(out T value);
            return value;
        }

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream without incrementing the position.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="value">The value to output.</param>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Peek<T>(out T value, bool marshal)
        {
            ReFillIfNecessary(Struct.GetSize<T>(marshal));
            _memory.Read(_gcHandlePtr + _bufferOffset, out value, marshal);
        }

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Peek<T>(out T value) where T : unmanaged
        {
            ReFillIfNecessary(sizeof(T));
            value = *(T*)(_gcHandlePtr + _bufferOffset);
        }

        /// <summary>
        /// Reads a managed or unmanaged generic type from the stream without incrementing the position.
        /// Note: For performance recommend using other overload if reading unmanaged type (i.e. marshal = false)
        /// </summary>
        /// <param name="marshal">Set to true to perform marshalling on the value being read, else false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek<T>(bool marshal)
        {
            Peek(out T value, marshal);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T Peek<T>() where T : unmanaged
        {
            Peek(out T value);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged primitive from the stream, swapping the endian of the output.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ReadBigEndianPrimitive<T>(out T value) where T : unmanaged
        {
            int size = sizeof(T);
            ReFillIfNecessary(size);

            value = *(T*)(_gcHandlePtr + _bufferOffset);
            Memory.Endian.Reverse(ref value);

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void ReadBigEndianStruct<T>(out T value) where T : unmanaged, IEndianReversible
        {
            int size = sizeof(T);
            ReFillIfNecessary(size);

            value = *(T*)(_gcHandlePtr + _bufferOffset);
            value.SwapEndian();

            _bufferOffset += size;
            _bufferedBytesRemaining -= size;
        }

        /// <summary>
        /// Reads an unmanaged primitive from the stream, swapping the endian of the output.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T ReadBigEndianPrimitive<T>() where T : unmanaged
        {
            ReadBigEndianPrimitive(out T value);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T ReadBigEndianStruct<T>() where T : unmanaged, IEndianReversible
        {
            ReadBigEndianStruct<T>(out T value);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged primitive from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void PeekBigEndianPrimitive<T>(out T value) where T : unmanaged
        {
            ReFillIfNecessary(sizeof(T));

            value = *(T*)(_gcHandlePtr + _bufferOffset);
            Memory.Endian.Reverse(ref value);
        }

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output without incrementing the position.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void PeekBigEndianStruct<T>(out T value) where T : unmanaged, IEndianReversible
        {
            ReFillIfNecessary(sizeof(T));

            value = *(T*)(_gcHandlePtr + _bufferOffset);
            value.SwapEndian();
        }

        /// <summary>
        /// Reads an unmanaged primitive from the stream, swapping the endian of the output without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T PeekBigEndianPrimitive<T>() where T : unmanaged
        {
            PeekBigEndianPrimitive(out T value);
            return value;
        }

        /// <summary>
        /// Reads an unmanaged struct from the stream, swapping the endian of the output without incrementing the position.
        /// The structure read should implement the <see cref="IEndianReversible"/> interface.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T PeekBigEndianStruct<T>() where T : unmanaged, IEndianReversible
        {
            PeekBigEndianStruct(out T value);
            return value;
        }

        /// <summary>
        /// Returns true if the <see cref="BufferedStreamReader"/> has sufficient space buffered
        /// to read memory of given size.
        /// </summary>
        /// <param name="size">The size of the item to check if can be read, in bytes.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanRead(int size)
        {
            return size <= _bufferedBytesRemaining;
        }

        /// <summary>
        /// Refills the remainder of the buffer if the current buffer cannot handle the given amount of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReFillIfNecessary(int size)
        {
            if (!CanRead(size))
                ReFillBuffer();
        }

        /// <summary>
        /// Refills the remainder of the buffer.
        /// i.e. Preserves data to still be read (<see cref="_bufferedBytesRemaining"/>) and reads enough data to fill rest of buffer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReFillBuffer()
        {
            // Rewind stream by amount of bytes remaining and read new data.
            _stream.Seek(_bufferedBytesRemaining * -1, SeekOrigin.Current);
            FillBuffer();
        }

        /// <summary>
        /// Fills the buffer with new data.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FillBuffer()
        {
            _bufferedBytesRemaining = _stream.Read(_buffer, 0, _bufferSize);
            _bufferOffset = 0;
        }

        /// <summary>
        /// Internal method that seeks the stream relatively to current position.
        /// </summary>
        private void RelativeSeek(long relativeOffset)
        {
            // Check if target is in current buffer, if not grab new.
            if (!FastSeekRelativeToCurrent(relativeOffset))
            {
                long targetPosition = Position() + relativeOffset;
                long targetOffset = targetPosition - _stream.Position;
                _stream.Seek(targetOffset, SeekOrigin.Current);
                FillBuffer();
            }
        }

        /// <summary>
        /// Tries to seek by a given offset in the existing buffer.
        /// Operation succeeds if offset fits in existing buffer, fails otherwise.
        /// </summary>
        /// <param name="offset">The offset relative to the current position to seek to.</param>
        /// <returns>Returns true if the seek was successful, else false.</returns>
        private bool FastSeekRelativeToCurrent(long offset)
        {
            if (offset >= 0)
            {
                if (offset <= _bufferedBytesRemaining)
                {
                    _bufferOffset += (int)offset;
                    _bufferedBytesRemaining -= (int)offset;
                    return true;
                }
            }
            else
            {
                // Convert negative to positive: offset * -1
                if (offset * -1 <= _bufferOffset)
                {
                    _bufferOffset += (int)offset;
                    _bufferedBytesRemaining -= (int)offset;
                    return true;
                }
            }

            return false;
        }
    }
}
