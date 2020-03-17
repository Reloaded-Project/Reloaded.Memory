using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Reloaded.Memory.Utilities
{
    /// <summary>
    /// An extended version of the <see cref="MemoryStream"/> class that allows you to directly add generic structs to the stream.
    /// </summary>
    public partial class ExtendedMemoryStream : MemoryStream
    {
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
            var padding = RoundUp((int)Length, alignment) - Length;
            if (padding <= 0)
                return;

            Write(new byte[padding], 0, (int)padding);
        }

        /// <summary>
        /// Pads the stream with <see cref="value"/> bytes until it is aligned.
        /// </summary>
        public void AddPadding(byte value, int alignment = 2048)
        {
            var padding = RoundUp((int)Length, alignment) - Length;
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
        public void Write<T>(T[] structure) where T : unmanaged => Write(StructArray.GetBytes(structure));

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T[] structure, bool marshalStructure = true) => Write(StructArray.GetBytes(structure, marshalStructure));

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure) where T : unmanaged => Write(Struct.GetBytes(structure));

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<T>(T structure, bool marshalStructure = true) => Write(Struct.GetBytes(structure, marshalStructure));

        /// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public void Write(byte[] data) => Write(data, 0, data.Length);


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
