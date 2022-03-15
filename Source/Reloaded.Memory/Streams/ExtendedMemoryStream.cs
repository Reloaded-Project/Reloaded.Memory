using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Reloaded.Memory.Streams
{
    /// <summary>
    /// An extended version of the <see cref="MemoryStream"/> class that allows you to directly add generic structs to the stream.
    /// </summary>
    public unsafe partial class ExtendedMemoryStream : MemoryStream
    {
        private const int MaxStackLimit = 1024;

        /// <inheritdoc />
        public ExtendedMemoryStream() { }

        /// <inheritdoc />
        public ExtendedMemoryStream(byte[] buffer) : base(buffer) { }

        /// <inheritdoc />
        public ExtendedMemoryStream(byte[] buffer, bool writable) : base(buffer, writable) { }

        /// <inheritdoc />
        public ExtendedMemoryStream(byte[] buffer, int index, int count) : base(buffer, index, count) { }

        /// <inheritdoc />
        public ExtendedMemoryStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable) { }

        /// <inheritdoc />
        public ExtendedMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible) : base(buffer, index, count, writable, publiclyVisible) { }

        /// <inheritdoc />
        public ExtendedMemoryStream(int capacity) : base(capacity) { }

        /// <summary>
        /// Pads the stream with 0x00 bytes until it is aligned.
        /// </summary>
        public void AddPadding(int alignment = 2048) => StreamExtensions.AddPadding(this, alignment);

        /// <summary>
        /// Pads the stream with <see paramref="value"/> bytes until it is aligned.
        /// </summary>
        public void AddPadding(byte value, int alignment = 2048) => StreamExtensions.AddPadding(this, value, alignment);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure) where T : unmanaged => StreamExtensions.Write(this, structure);

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure, bool marshalStructure = true) => StreamExtensions.Write(this, structure, marshalStructure);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure) where T : unmanaged => StreamExtensions.Write(this, ref structure);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(ref T structure) where T : unmanaged => StreamExtensions.Write(this, ref structure);


#if FEATURE_NATIVE_SPAN
        /// <summary>
        /// Writes the sequence of bytes contained in source into the current memory stream and advances the current position within this memory stream by the number of bytes written.
        /// </summary>
        /// <param name="source">A region of memory. This method copies the contents of this region to the current memory stream.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new void Write(ReadOnlySpan<byte> source) => base.Write(source);
#endif

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure, bool marshalStructure = true) => StreamExtensions.Write(this, ref structure, marshalStructure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(ref T structure, bool marshalStructure = true) => StreamExtensions.Write(this, ref structure, marshalStructure);

        /// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public void Write(byte[] data) => Write(data, 0, data.Length);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianPrimitive<T>(T[] structures) where T : unmanaged => StreamExtensions.WriteBigEndianPrimitive(this, structures);

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianStruct<T>(T[] structures) where T : unmanaged, IEndianReversible => StreamExtensions.WriteBigEndianStruct(this, structures);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianPrimitive<T>(T structure) where T : unmanaged => StreamExtensions.WriteBigEndianPrimitive(this, structure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianStruct<T>(T structure) where T : unmanaged, IEndianReversible => StreamExtensions.WriteBigEndianStruct(this, structure);
    }
}
