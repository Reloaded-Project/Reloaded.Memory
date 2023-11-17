// Modified source, originally:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;

#if NET7_0_OR_GREATER
using System.Diagnostics;
using System.Runtime.Intrinsics;
#endif

namespace Reloaded.Memory.Internals.Backports.System.Text.Unicode;

#pragma warning disable RCS1141 // Add parameter to documentation comment.
[ExcludeFromCodeCoverage] // "Taken from .NET Runtime"
internal static class Utf16Utility
{
    /// <summary>
    ///     Returns true iff the 64-bit nuint represents all ASCII UTF-16 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to assert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe bool AllCharsInNuintAreAscii(nuint value)
    {
        switch (sizeof(nuint))
        {
            // Replaced with concrete implementation by JIT.
            case 4:
                return (value & ~0x007F_007Fu) == 0;
            case 8:
                return (value & ~0x007F_007F_007F_007Fu) == 0;
            default:
                ThrowHelpers.ThrowArchitectureNotSupportedException();
                return false;
        }
    }

    /// <summary>
    ///     Returns true iff the 64-bit nuint represents all ASCII UTF-16 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to assert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInULongAreAscii(ulong value) => (value & ~0x007F_007F_007F_007Fu) == 0;

    /// <summary>
    ///     Returns true iff the 32-bit nuint represents all ASCII UTF-16 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to assert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInUIntAreAscii(uint value) => (value & ~0x007F_007F) == 0;

#if NET7_0_OR_GREATER
    /// <summary>
    ///     Returns true iff the Vector128 represents 8 ASCII UTF-16 characters in machine endianness.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInVector128AreAscii(Vector128<uint> vec) =>
        AllCharsInVector128AreAscii(vec.AsUInt16());

    /// <summary>
    ///     Returns true iff the Vector256 represents 16 ASCII UTF-16 characters in machine endianness.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInVector256AreAscii(Vector256<uint> vec) =>
        AllCharsInVector256AreAscii(vec.AsUInt16());

    /// <summary>
    ///     Returns true iff the Vector128 represents 8 ASCII UTF-16 characters in machine endianness.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInVector128AreAscii(Vector128<ushort> vec) =>
        (vec & Vector128.Create(unchecked((ushort)~0x007F))) == Vector128<ushort>.Zero;

    /// <summary>
    ///     Returns true iff the Vector256 represents 16 ASCII UTF-16 characters in machine endianness.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool AllCharsInVector256AreAscii(Vector256<ushort> vec) =>
        (vec & Vector256.Create(unchecked((ushort)~0x007F))) == Vector256<ushort>.Zero;

    /// <summary>
    ///     Convert Vector128 that represent 8 ASCII UTF-16 characters to lowercase
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<ushort> Vector128AsciiToLowercase(Vector128<ushort> vec)
    {
        // ASSUMPTION: Caller has validated that input values are ASCII.
        Debug.Assert(AllCharsInVector128AreAscii(vec));

        // the 0x80 bit of each word of 'lowerIndicator' will be set iff the word has value >= 'A'
        Vector128<sbyte> lowIndicator1 = Vector128.Create((sbyte)(0x80 - 'A')) + vec.AsSByte();

        // the 0x80 bit of each word of 'combinedIndicator' will be set iff the word has value >= 'A' and <= 'Z'
        Vector128<sbyte> combIndicator1 = Vector128.LessThan(
            Vector128.Create(unchecked((sbyte)('Z' - 'A' - 0x80))), lowIndicator1);

        // Add the lowercase indicator (0x20 bit) to all A-Z letters
        return Vector128.AndNot(Vector128.Create((sbyte)0x20), combIndicator1).AsUInt16() + vec;
    }

    /// <summary>
    ///     Convert Vector256 that represent 16 ASCII UTF-16 characters to lowercase
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<ushort> Vector256AsciiToLowercase(Vector256<ushort> vec)
    {
        // the 0x80 bit of each word of 'lowerIndicator' will be set iff the word has value >= 'A'
        Vector256<sbyte> lowIndicator1 = Vector256.Create((sbyte)(0x80 - 'A')) + vec.AsSByte();

        // the 0x80 bit of each word of 'combinedIndicator' will be set iff the word has value >= 'A' and <= 'Z'
        Vector256<sbyte> combIndicator1 = Vector256.LessThan(
            Vector256.Create(unchecked((sbyte)('Z' - 'A' - 0x80))), lowIndicator1);

        // Add the lowercase indicator (0x20 bit) to all A-Z letters
        return Vector256.AndNot(Vector256.Create((sbyte)0x20), combIndicator1).AsUInt16() + vec;
    }

    /// <summary>
    ///     Convert Vector128 that represent 8 ASCII UTF-16 characters to uppercase
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<ushort> Vector128AsciiToUppercase(Vector128<ushort> vec)
    {
        // ASSUMPTION: Caller has validated that input values are ASCII.
        Debug.Assert(AllCharsInVector128AreAscii(vec));

        // the 0x80 bit of each word of 'lowerIndicator' will be set iff the word has value >= 'a'
        Vector128<sbyte> lowIndicator1 = Vector128.Create((sbyte)(0x80 - 'a')) + vec.AsSByte();

        // the 0x80 bit of each word of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
        Vector128<sbyte> combIndicator1 = Vector128.LessThan(
            Vector128.Create(unchecked((sbyte)('z' - 'a' - 0x80))), lowIndicator1);

        // Drop the lowercase indicator (0x20 bit) from all a-z letters
        return vec - Vector128.AndNot(Vector128.Create((sbyte)0x20), combIndicator1).AsUInt16();
    }

    /// <summary>
    ///     Convert Vector256 that represent 16 ASCII UTF-16 characters to uppercase
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<ushort> Vector256AsciiToUppercase(Vector256<ushort> vec)
    {
        // the 0x80 bit of each word of 'lowerIndicator' will be set iff the word has value >= 'a'
        Vector256<sbyte> lowIndicator1 = Vector256.Create((sbyte)(0x80 - 'a')) + vec.AsSByte();

        // the 0x80 bit of each word of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
        Vector256<sbyte> combIndicator1 = Vector256.LessThan(
            Vector256.Create(unchecked((sbyte)('z' - 'a' - 0x80))), lowIndicator1);

        // Drop the lowercase indicator (0x20 bit) from all a-z letters
        return vec - Vector256.AndNot(Vector256.Create((sbyte)0x20), combIndicator1).AsUInt16();
    }
#endif
}
