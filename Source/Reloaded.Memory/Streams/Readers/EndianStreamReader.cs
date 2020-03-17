using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Readers
{
    /// <summary>
    /// An abstract class that abstracts <see cref="BufferedStreamReader"/>, allowing for individual implementations for each endian.
    /// </summary>
    public abstract partial class EndianStreamReader : IDisposable
    {
        /// <summary>
        /// The underlying Stream Reader assigned to this reader.
        /// </summary>
        public BufferedStreamReader Reader { get; private set; }

        /* Properties (Written as methods to force inlining). */

        /// <summary>
        /// Gets the stream this class was instantiated with.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stream BaseStream() => Reader.BaseStream();

        /// <summary>
        /// Gets the remaining number of bytes that are currently buffered.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BufferBytesAvailable() => Reader.BufferBytesAvailable();

        /// <summary>
        /// Gets the total size of the current buffered data at this moment in time.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CurrentBufferSize() => Reader.CurrentBufferSize();

        /// <summary>
        /// The current position of the buffer.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Position() => Reader.Position();

        /// <summary>
        /// Constructs a <see cref="EndianStreamReader"/> given an existing stream reader.
        /// </summary>
        protected EndianStreamReader(BufferedStreamReader streamReader) => Reader = streamReader;

        /// <summary/>
        ~EndianStreamReader()
        {
            this.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Reader?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Read<T>(out T value) where T : unmanaged;

        /// <summary>
        /// Reads an unmanaged, generic type from the stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe T Read<T>() where T : unmanaged;

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        /// <param name="value">The value to output.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe void Peek<T>(out T value) where T : unmanaged;

        /// <summary>
        /// Reads an unmanaged, generic type from the stream without incrementing the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract unsafe T Peek<T>() where T : unmanaged;

        /// <summary>
        /// Seeks the underlying stream to a specified position.
        /// </summary>
        public void Seek(long offset, SeekOrigin origin) => Reader.Seek(offset, origin);

        /// <summary>
        /// Reads a specified amount of bytes at a specific offset from the underlying stream without
        /// resetting the buffers or advancing the read pointer.
        ///
        /// Note: This method is intended only for reading of large size raw data e.g. compressed file data in an archive.
        ///       No optimizations are performed for this action.
        /// </summary>
        /// <param name="offset">The offset of the data from the start of the stream.</param>
        /// <param name="count">The amount of bytes to read.</param>
        public byte[] ReadBytes(long offset, int count) => Reader.ReadBytes(offset, count);

        /// <summary>
        /// Returns true if the <see cref="BufferedStreamReader"/> has sufficient space buffered
        /// to read memory of given size.
        /// </summary>
        /// <param name="size">The size of the item to check if can be read, in bytes.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanRead(int size) => Reader.CanRead(size);
    }
}
