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
        public void AddPadding(int alignment = 2048)
        {
            var padding = RoundUp((int)Position, alignment) - Position;
            if (padding <= 0)
                return;

            Write(new byte[padding], 0, (int)padding);
        }

        /// <summary>
        /// Pads the stream with <see cref="value"/> bytes until it is aligned.
        /// </summary>
        public void AddPadding(byte value, int alignment = 2048)
        {
            var padding = RoundUp((int)Position, alignment) - Position;
            if (padding <= 0)
                return;

            var bytes = new byte[padding];
            for (int x = 0; x < bytes.Length; x++) 
                bytes[x] = value;

            Write(bytes, 0, (int)padding);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure) where T : unmanaged
        {
            for (int x = 0; x < structure.Length; x++)
                Write(ref structure[x]);
        }

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure, bool marshalStructure = true)
        {
            for (int x = 0; x < structure.Length; x++)
                Write(ref structure[x], marshalStructure);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure) where T : unmanaged => Write(ref structure);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(ref T structure) where T : unmanaged
        {
#if FEATURE_NATIVE_SPAN
            if (sizeof(T) < MaxStackLimit)
            {
                Span<byte> stack = stackalloc byte[sizeof(T)];
                Struct.GetBytes(ref structure, stack);
                Write(stack);
            }
            else
            {
                Write(Struct.GetBytes(structure));
            }
#else
            Write(Struct.GetBytes(structure));
#endif
        }


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
        public void Write<T>(T structure, bool marshalStructure = true) => Write(ref structure, marshalStructure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(ref T structure, bool marshalStructure = true)
        {
#if FEATURE_NATIVE_SPAN
            var size = Struct.GetSize<T>(true);
            if (size < MaxStackLimit)
            {
                Span<byte> stack = stackalloc byte[size];
                Struct.GetBytes(ref structure, marshalStructure, stack);
                base.Write(stack);
            }
            else
            {
                Write(Struct.GetBytes(structure, marshalStructure));
            }
#else
            Write(Struct.GetBytes(structure, marshalStructure));
#endif
        }

        /// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public void Write(byte[] data) => Write(data, 0, data.Length);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianPrimitive<T>(T[] structures) where T : unmanaged
        {
            for (var x = 0; x < structures.Length; x++)
                WriteBigEndianPrimitive(structures[x]);
        }

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianStruct<T>(T[] structures) where T : unmanaged, IEndianReversible
        {
            for (var x = 0; x < structures.Length; x++)
                WriteBigEndianStruct(structures[x]);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianPrimitive<T>(T structure) where T : unmanaged
        {
            Endian.Reverse(ref structure);
            Write(ref structure);
        }

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBigEndianStruct<T>(T structure) where T : unmanaged, IEndianReversible
        {
            structure.SwapEndian();
            Write(ref structure);
        }

        private static int RoundUp(int number, int multiple)
        {
            if (multiple == 0)
                return number;

            int remainder = number % multiple;
            if (remainder == 0)
                return number;

            return number + multiple - remainder;
        }
    }
}
