using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Memory.Streams
{
    /// <summary>
    /// Provides various extensions to streams.
    /// </summary>
    public static unsafe class StreamExtensions
    {
        private const int MaxStackLimit = 1024;

        /// <summary>
        /// Pads the stream with 0x00 bytes until it is aligned.
        /// </summary>
        public static void AddPadding(this Stream stream, int alignment = 2048)
        {
            var padding = Internal.Utilities.RoundUp((int)stream.Position, alignment) - stream.Position;
            if (padding <= 0)
                return;

            stream.Write(new byte[padding], 0, (int)padding);
        }

        /// <summary>
        /// Pads the stream with <see paramref="value"/> bytes until it is aligned.
        /// </summary>
        public static void AddPadding(this Stream stream, byte value, int alignment = 2048)
        {
            var padding = Internal.Utilities.RoundUp((int)stream.Position, alignment) - stream.Position;
            if (padding <= 0)
                return;

            var bytes = new byte[padding];
            for (int x = 0; x < bytes.Length; x++)
                bytes[x] = value;

            stream.Write(bytes, 0, (int)padding);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T[] structure) where T : unmanaged
        {
            for (int x = 0; x < structure.Length; x++)
                stream.Write(ref structure[x]);
        }

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T[] structure, bool marshalStructure = true)
        {
            for (int x = 0; x < structure.Length; x++)
                stream.Write(ref structure[x], marshalStructure);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T structure) where T : unmanaged => stream.Write(ref structure);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, ref T structure) where T : unmanaged
        {
#if FEATURE_NATIVE_SPAN
            if (sizeof(T) < MaxStackLimit)
            {
                Span<byte> stack = stackalloc byte[sizeof(T)];
                MemoryMarshal.Write(stack, ref structure);
                stream.Write(stack);
            }
            else
            {
                stream.Write(Struct.GetBytes(structure));
            }
#else
            stream.Write(Struct.GetBytes(structure));
#endif
        }

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T structure, bool marshalStructure = true) => stream.Write(ref structure, marshalStructure);

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, ref T structure, bool marshalStructure = true)
        {
#if FEATURE_NATIVE_SPAN
            var size = Struct.GetSize<T>(true);
            if (size < MaxStackLimit)
            {
                Span<byte> stack = stackalloc byte[size];
                Struct.GetBytes(ref structure, marshalStructure, stack);
                stream.Write(stack);
            }
            else
            {
                stream.Write(Struct.GetBytes(structure, marshalStructure));
            }
#else
            stream.Write(Struct.GetBytes(structure, marshalStructure));
#endif
        }

        /// <summary>
        /// Appends bytes onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        public static void Write(this Stream stream, byte[] data) => stream.Write(data, 0, data.Length);

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianPrimitive<T>(this Stream stream, T[] structures) where T : unmanaged
        {
            for (var x = 0; x < structures.Length; x++)
                stream.WriteBigEndianPrimitive(structures[x]);
        }

        /// <summary>
        /// Appends an managed/marshalled structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianStruct<T>(this Stream stream, T[] structures) where T : unmanaged, IEndianReversible
        {
            for (var x = 0; x < structures.Length; x++)
                stream.WriteBigEndianStruct(structures[x]);
        }

        /// <summary>
        /// Appends an unmanaged structure onto the <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianPrimitive<T>(this Stream stream, T structure) where T : unmanaged
        {
            Endian.Reverse(ref structure);
            stream.Write(ref structure);
        }

        /// <summary>
        /// Appends a managed/marshalled structure onto the given <see cref="MemoryStream"/> and advances the position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBigEndianStruct<T>(this Stream stream, T structure) where T : unmanaged, IEndianReversible
        {
            structure.SwapEndian();
            stream.Write(ref structure);
        }
    }
}
