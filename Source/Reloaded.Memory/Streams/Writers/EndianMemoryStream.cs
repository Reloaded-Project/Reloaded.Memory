﻿using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory.Streams.Writers
{
    /// <summary>
    /// An abstract class that abstracts <see cref="EndianMemoryStream"/>, allowing for individual implementations for each endian.
    /// </summary>
    public abstract partial class EndianMemoryStream : IDisposable
    {
        /// <summary>
        /// The underlying Stream.
        /// </summary>
        public ExtendedMemoryStream Stream { get; private set; }

        /// <summary>
        /// If this is set, the underlying stream will be disposed.
        /// </summary>
        public bool DisposeUnderlyingStream { get; private set; } = true;

        /// <summary>
        /// Constructs a <see cref="EndianMemoryStream"/> given an existing stream.
        /// </summary>
        protected EndianMemoryStream(ExtendedMemoryStream stream) => Stream = stream;

        /// <summary>
        /// Constructs a <see cref="EndianMemoryStream"/> given an existing stream.
        /// </summary>
        protected EndianMemoryStream(ExtendedMemoryStream stream, bool disposeUnderlyingStream)
        {
            Stream = stream;
            DisposeUnderlyingStream = disposeUnderlyingStream;
        }

        /// <summary/>
        ~EndianMemoryStream()
        {
            this.Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (DisposeUnderlyingStream)
                Stream?.Dispose();
            
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Pads the stream with 0x00 bytes until it is aligned.
        /// </summary>
        public void AddPadding(int alignment = 2048) => Stream.AddPadding(alignment);

        /// <summary>
        /// Pads the stream with <see paramref="value"/> bytes until it is aligned.
        /// </summary>
        public void AddPadding(byte value, int alignment = 2048) => Stream.AddPadding(value, alignment);

        /// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public void Write(byte[] data) => Stream.Write(data);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Write<T>(T[] structure) where T : unmanaged;

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Write<T>(T structure) where T : unmanaged;

        /// <summary>
        /// Converts the underlying stream to an array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToArray() => Stream.ToArray();
    }
}
