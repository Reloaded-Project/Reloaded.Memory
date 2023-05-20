using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;

namespace Reloaded.Memory.Utilities;

/// <summary>
///     Utility methods for dealing with endian conversions.
/// </summary>
/// <summary>
///     Provides various utilities for converting primitives and structures between endians.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Endian
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Reverse(float value)
    {
        var integer = Unsafe.As<float, int>(ref value);
        integer = BinaryPrimitives.ReverseEndianness(integer);
        return Unsafe.As<int, float>(ref integer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Reverse(double value)
    {
        var integer = Unsafe.As<double, long>(ref value);
        integer = BinaryPrimitives.ReverseEndianness(integer);
        return Unsafe.As<long, double>(ref integer);
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    ///     Reverses the endian of a primitive value such as int, short, float, double etc. (Not including structs).
    /// </summary>
    /// <typeparam name="T">Type of element whose endian is to be reversed.</typeparam>
    /// <param name="value">The individual value to be byte reversed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T Reverse<T>(T value) where T : unmanaged
    {
        if (sizeof(T) == 1)
            return value;

        if (sizeof(T) == 2)
        {
            var integer = Unsafe.As<T, ushort>(ref value);
            integer = BinaryPrimitives.ReverseEndianness(integer);
            return Unsafe.As<ushort, T>(ref integer);
        }

        if (sizeof(T) == 4)
        {
            var integer = Unsafe.As<T, int>(ref value);
            integer = BinaryPrimitives.ReverseEndianness(integer);
            return Unsafe.As<int, T>(ref integer);
        }

        if (sizeof(T) == 8)
        {
            var integer = Unsafe.As<T, long>(ref value);
            integer = BinaryPrimitives.ReverseEndianness(integer);
            return Unsafe.As<long, T>(ref integer);
        }

        ThrowHelpers.ThrowTypeNotSupportedException<T>();
        return value;
    }
}
