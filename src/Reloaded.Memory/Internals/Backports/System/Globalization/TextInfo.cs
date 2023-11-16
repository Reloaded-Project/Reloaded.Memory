// Modified source, originally:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET7_0_OR_GREATER
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Internals.Backports.System.Text.Unicode;
// ReSharper disable UnusedType.Global

namespace Reloaded.Memory.Internals.Backports.System.Globalization;

#pragma warning disable RCS1142 // Remove redundant empty line.
#pragma warning disable RCS1141 // Add parameter to documentation comment.

[ExcludeFromCodeCoverage(Justification = "Taken [albeit slightly modified] from .NET 8 Runtime")]
internal static class TextInfo
{
    /// <summary>
    ///     Custom overload for backported vectorised change case implementations from .NET 8.
    /// </summary>
    /// <typeparam name="TConversion">Type of conversion to use.</typeparam>
    /// <param name="source">The string whose case is to be changed.</param>
    public static string ChangeCase<TConversion>(string source) where TConversion : struct =>
        string.Create(source.Length, source, (span, state) => ChangeCase<TConversion>(state, span));

    /// <summary>
    ///     Custom overload for backported vectorised change case implementations from .NET 8.
    /// </summary>
    /// <typeparam name="TConversion">Type of conversion to use.</typeparam>
    /// <param name="source">The string whose case is to be changed (as span).</param>
    public static unsafe string ChangeCase<TConversion>(ReadOnlySpan<char> source) where TConversion : struct
    {
        // Some dumb overhead here, but it's unavoidable, since Span can be sourced from Heap String or Array
        fixed (char* first = &source.GetPinnableReference())
        {
            var prms = new ChangeCaseParams(first, source.Length);
            return string.Create(source.Length, prms, (span, state) => ChangeCase<TConversion>(new ReadOnlySpan<char>(state.First, state.Length), span));
        }
    }

    /// <summary>
    ///     Backported vectorised change case implementations from .NET 8 (extended with Vector256 support!)
    /// </summary>
    /// <typeparam name="TConversion">Type of conversion to use.</typeparam>
    /// <param name="source">The string whose case is to be changed.</param>
    /// <param name="destination">The destination where the new string is to be written.</param>
    public static void ChangeCase<TConversion>(ReadOnlySpan<char> source, Span<char> destination) where TConversion : struct
    {
        if (Vector256.IsHardwareAccelerated && source.Length >= Vector256<ushort>.Count)
            ChangeCase_Vector256<TConversion>(ref MemoryMarshal.GetReference(source), ref MemoryMarshal.GetReference(destination), source.Length);
        else if (Vector128.IsHardwareAccelerated && source.Length >= Vector128<ushort>.Count)
            ChangeCase_Vector128<TConversion>(ref MemoryMarshal.GetReference(source), ref MemoryMarshal.GetReference(destination), source.Length);
        else
            ChangeCase_Under16B<TConversion>(source, destination);
    }

    /// <summary>
    ///     Note: Modified to assume ASCII casing same as invariant.
    /// </summary>
    public static void ChangeCase_Vector256<TConversion>(ref char source, ref char destination, int charCount) where TConversion : struct
    {
        Debug.Assert(charCount >= Vector256<ushort>.Count);
        Debug.Assert(Vector256.IsHardwareAccelerated);

        // JIT will treat this as a constant in release builds
        var toUpper = typeof(TConversion) == typeof(ToUpperConversion);
        nuint i = 0;

        ref var src = ref Unsafe.As<char, ushort>(ref source);
        ref var dst = ref Unsafe.As<char, ushort>(ref destination);

        var lengthU = (nuint)charCount;
        var lengthToExamine = lengthU - (nuint)Vector256<ushort>.Count;
        do
        {
            var vec = Vector256.LoadUnsafe(ref src, i);
            if (!Utf16Utility.AllCharsInVector256AreAscii(vec))
            {
                goto NON_ASCII;
            }

            vec = toUpper ? Utf16Utility.Vector256AsciiToUppercase(vec) : Utf16Utility.Vector256AsciiToLowercase(vec);
            vec.StoreUnsafe(ref dst, i);

            i += (nuint)Vector256<ushort>.Count;
        } while (i <= lengthToExamine);

        Debug.Assert(i <= lengthU);

        // Handle trailing elements
        if (i < lengthU)
        {
            var trailingElements = lengthU - (nuint)Vector256<ushort>.Count;
            var vec = Vector256.LoadUnsafe(ref src, trailingElements);
            if (!Utf16Utility.AllCharsInVector256AreAscii(vec))
            {
                goto NON_ASCII;
            }

            vec = toUpper ? Utf16Utility.Vector256AsciiToUppercase(vec) : Utf16Utility.Vector256AsciiToLowercase(vec);
            vec.StoreUnsafe(ref dst, trailingElements);
        }

        return;

    NON_ASCII:
        // We encountered non-ASCII data and therefore can't perform invariant case conversion;
        // Fallback to ICU/NLS.
        // We modify this to fall back to current runtime impl.
        var length = charCount - (int)i;
        var srcSpan = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref source, i), length);
        var dstSpan = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref destination, i), length);
        ChangeCase_Fallback<TConversion>(srcSpan, dstSpan);
    }

    /// <summary>
    /// Modified to assume ASCII casing same as invariant.
    /// </summary>
    public static void ChangeCase_Vector128<TConversion>(ref char source, ref char destination, int charCount) where TConversion : struct
    {
        Debug.Assert(charCount >= Vector128<ushort>.Count);
        Debug.Assert(Vector128.IsHardwareAccelerated);

        // JIT will treat this as a constant in release builds
        var toUpper = typeof(TConversion) == typeof(ToUpperConversion);
        nuint i = 0;

        ref var src = ref Unsafe.As<char, ushort>(ref source);
        ref var dst = ref Unsafe.As<char, ushort>(ref destination);

        var lengthU = (nuint)charCount;
        var lengthToExamine = lengthU - (nuint)Vector128<ushort>.Count;
        do
        {
            var vec = Vector128.LoadUnsafe(ref src, i);
            if (!Utf16Utility.AllCharsInVector128AreAscii(vec))
            {
                goto NON_ASCII;
            }

            vec = toUpper ? Utf16Utility.Vector128AsciiToUppercase(vec) : Utf16Utility.Vector128AsciiToLowercase(vec);
            vec.StoreUnsafe(ref dst, i);

            i += (nuint)Vector128<ushort>.Count;
        } while (i <= lengthToExamine);

        Debug.Assert(i <= lengthU);

        // Handle trailing elements
        if (i < lengthU)
        {
            var trailingElements = lengthU - (nuint)Vector128<ushort>.Count;
            var vec = Vector128.LoadUnsafe(ref src, trailingElements);
            if (!Utf16Utility.AllCharsInVector128AreAscii(vec))
            {
                goto NON_ASCII;
            }

            vec = toUpper ? Utf16Utility.Vector128AsciiToUppercase(vec) : Utf16Utility.Vector128AsciiToLowercase(vec);
            vec.StoreUnsafe(ref dst, trailingElements);
        }

        return;

    NON_ASCII:
        // We encountered non-ASCII data and therefore can't perform invariant case conversion;
        // Fallback to ICU/NLS.
        // We modify this to fall back to current runtime impl.
        var length = charCount - (int)i;
        var srcSpan = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref source, i), length);
        var dstSpan = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref destination, i), length);
        ChangeCase_Fallback<TConversion>(srcSpan, dstSpan);
    }

    // A dummy struct that is used for 'ToUpper' in generic parameters
    public readonly struct ToUpperConversion;

    // A dummy struct that is used for 'ToLower' in generic parameters
    public readonly struct ToLowerConversion;

    private unsafe struct ChangeCaseParams(char* first, int length)
    {
        public readonly char* First = first;
        public readonly int Length = length;
    };

    /// <summary>
    ///     An implementation of Change Case for inputs up to 16 bytes.
    ///     Custom, not taken from runtime.
    /// </summary>
    /// <param name="source">Source span.</param>
    /// <param name="destination">Destination span.</param>
    public static unsafe void ChangeCase_Under16B<TConversion>(ReadOnlySpan<char> source, Span<char> destination) where TConversion : struct
    {
        var length = source.Length;

        // JIT will treat this as a constant in release builds
        var toUpper = typeof(TConversion) == typeof(ToUpperConversion);

        // 32 bit implementation
        if (sizeof(nuint) == 4)
        {
            ref uint srcNuintPtr = ref Unsafe.As<char, uint>(ref MemoryMarshal.GetReference(source));
            ref uint dstNuintPtr = ref Unsafe.As<char, uint>(ref MemoryMarshal.GetReference(destination));

            // 32 bit implementation
            // range: 0-7 chars (0-14 bytes)
            // keep converting 4 bytes at once until we are left with 0-2
            while (length >= 2)
            {
                length -= 2;
                if (!Utf16Utility.AllCharsInUIntAreAscii(srcNuintPtr))
                    goto NotAscii;

                dstNuintPtr = toUpper
                    ? Utf8Utility.ConvertAllAsciiBytesInUInt32ToUppercase(srcNuintPtr)
                    : Utf8Utility.ConvertAllAsciiBytesInUInt32ToLowercase(srcNuintPtr);

                srcNuintPtr = ref Unsafe.Add(ref srcNuintPtr, 1);
                dstNuintPtr = ref Unsafe.Add(ref dstNuintPtr, 1);
            }

            ref char srcCharPtr = ref Unsafe.As<uint, char>(ref srcNuintPtr);
            ref char dstCharPtr = ref Unsafe.As<uint, char>(ref dstNuintPtr);
            if (length > 0)
            {
                if (toUpper)
                {
                    if (UnicodeUtility.IsInRangeInclusive(srcCharPtr, 'a', 'z'))
                    {
                        dstCharPtr = (char)(srcCharPtr - (char)0x20u);
                        return;
                    }

                    goto NotAscii;
                }
                else
                {
                    if (UnicodeUtility.IsInRangeInclusive(srcCharPtr, 'A', 'Z'))
                    {
                        dstCharPtr = (char)(srcCharPtr + (char)0x20u);
                        return;
                    }

                    goto NotAscii;
                }
            }

            return;
        }

        // 64 bit implementation
        if (sizeof(nuint) == 8)
        {
            ref nuint srcNuintPtr = ref Unsafe.As<char, nuint>(ref MemoryMarshal.GetReference(source));
            ref nuint dstNuintPtr = ref Unsafe.As<char, nuint>(ref MemoryMarshal.GetReference(destination));

            // range: 0-7 chars (0-14 bytes)
            // -4 chars
            if (length >= 4)
            {
                length -= sizeof(nuint) / sizeof(char);
                if (!Utf16Utility.AllCharsInNuintAreAscii(srcNuintPtr))
                    goto NotAscii;

                dstNuintPtr = toUpper
                    ? (nuint)Utf8Utility.ConvertAllAsciiBytesInUInt64ToUppercase(srcNuintPtr)
                    : (nuint)Utf8Utility.ConvertAllAsciiBytesInUInt64ToLowercase(srcNuintPtr);

                srcNuintPtr = ref Unsafe.Add(ref srcNuintPtr, 1);
                dstNuintPtr = ref Unsafe.Add(ref dstNuintPtr, 1);
            }

            // -2 chars
            ref uint srcUIntPtr = ref Unsafe.As<nuint, uint>(ref srcNuintPtr);
            ref uint dstUIntPtr = ref Unsafe.As<nuint, uint>(ref dstNuintPtr);
            if (length >= 2)
            {
                length -= 2;
                if (!Utf16Utility.AllCharsInUIntAreAscii(srcUIntPtr))
                    goto NotAscii;

                dstUIntPtr = toUpper
                    ? Utf8Utility.ConvertAllAsciiBytesInUInt32ToUppercase(srcUIntPtr)
                    : Utf8Utility.ConvertAllAsciiBytesInUInt32ToLowercase(srcUIntPtr);

                srcUIntPtr = ref Unsafe.Add(ref srcUIntPtr, 1);
                dstUIntPtr = ref Unsafe.Add(ref dstUIntPtr, 1);
            }

            // -1 char
            ref char srcCharPtr = ref Unsafe.As<uint, char>(ref srcUIntPtr);
            ref char dstCharPtr = ref Unsafe.As<uint, char>(ref dstUIntPtr);
            if (length >= 1)
            {
                if (toUpper)
                {
                    if (UnicodeUtility.IsInRangeInclusive(srcCharPtr, 'a', 'z'))
                    {
                        dstCharPtr = (char)(srcCharPtr - (char)0x20u);
                        return;
                    }

                    goto NotAscii;
                }
                else
                {
                    if (UnicodeUtility.IsInRangeInclusive(srcCharPtr, 'A', 'Z'))
                    {
                        dstCharPtr = (char)(srcCharPtr + (char)0x20u);
                        return;
                    }

                    goto NotAscii;
                }
            }

            return;
        }

        ThrowHelpers.ThrowArchitectureNotSupportedException();
        return;

        NotAscii:
            ChangeCase_Fallback<TConversion>(source, destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ChangeCase_Fallback<TConversion>(ReadOnlySpan<char> source, Span<char> destination)
    {
        // JIT will treat this as a constant in release builds
        var toUpper = typeof(TConversion) == typeof(ToUpperConversion);
        try
        {
            if (toUpper)
                source.ToUpperInvariant(destination);
            else
                source.ToLowerInvariant(destination);
        }
        catch (InvalidOperationException)
        {
            // Overlapping buffers
            if (toUpper)
                source.ToString().AsSpan().ToUpperInvariant(destination);
            else
                source.ToString().AsSpan().ToLowerInvariant(destination);
        }
    }
}
#endif
