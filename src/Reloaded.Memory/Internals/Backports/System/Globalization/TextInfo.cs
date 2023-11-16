// Modified source, originally:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET7_0_OR_GREATER
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;
using System.Text;
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
        {
            var toUpper = typeof(TConversion) == typeof(ToUpperConversion);
            ChangeCase_Fallback(source, destination, toUpper);
        }
    }

    private static void ChangeCase_Fallback(ReadOnlySpan<char> source, Span<char> destination, bool toUpper)
    {
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
        ChangeCase_Fallback(srcSpan, dstSpan, toUpper);
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
        ChangeCase_Fallback(srcSpan, dstSpan, toUpper);
    }

    // A dummy struct that is used for 'ToUpper' in generic parameters
    public readonly struct ToUpperConversion;

    // A dummy struct that is used for 'ToLower' in generic parameters
    public readonly struct ToLowerConversion;
}
#endif
