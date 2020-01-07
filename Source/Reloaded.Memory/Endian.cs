using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace Reloaded.Memory
{
    /// <summary>
    /// Provides various utilities for converting primitives and structures between endians.
    /// </summary>
    public static class Endian
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Reverse(byte value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Reverse(sbyte value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Reverse(short value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Reverse(ushort value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Reverse(int value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Reverse(uint value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Reverse(long value) => BinaryPrimitives.ReverseEndianness(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Reverse(ulong value) => BinaryPrimitives.ReverseEndianness(value);

        /// <summary>
        /// Reverses the endian of a primitive value such as int, short, float, double etc. (Not including structs).
        /// </summary>
        /// <param name="type">The individual value to be byte reversed.</param>
        /// <param name="swapped">The output variable to receive the swapped out value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Reverse<T>(ref T type, out T swapped) where T : unmanaged
        {
            swapped = type;
            Reverse(ref swapped);
        }

        /// <summary>
        /// Reverses the endian of a primitive value such as int, short, float, double etc. (Not including structs).
        /// </summary>
        /// <param name="type">The individual value to be byte reversed. The value will be modified directly.</param>
        public static unsafe void Reverse<T>(ref T type) where T : unmanaged
        {
            byte* bytes = (byte*)Unsafe.AsPointer(ref type);
            var byteSpan = new Span<byte>(bytes, sizeof(T));
            byteSpan.Reverse();
        }
    }
}
