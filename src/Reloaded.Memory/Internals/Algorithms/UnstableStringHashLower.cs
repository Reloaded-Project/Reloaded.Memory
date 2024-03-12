using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Utilities;
using static Reloaded.Memory.Internals.Backports.System.Text.Unicode.Utf16Utility;
#if NET7_0_OR_GREATER
using static Reloaded.Memory.Internals.Algorithms.UnstableStringHash;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Internals.Backports.System.Globalization;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Reloaded.Memory.Internals.Algorithms;

/// <summary>
///     Unstable string hash algorithm.
///     Each implementation prioritises speed, and different machines may produce different results.
/// </summary>
internal static class UnstableStringHashLower
{
    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    ///     Hashes the string in lower (invariant) case.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    /// </remarks>
    [ExcludeFromCodeCoverage] // "Cannot be accurately measured without multiple architectures. Known good impl." This is still tested tho.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe nuint GetHashCodeUnstableLower(this ReadOnlySpan<char> text)
    {
#if NET7_0_OR_GREATER
        var length = text.Length; // Span has no guarantee of null terminator.

        // Note. In these SIMD implementations we leave some (< sizeof(nuint)) data from the hash.
        // For our use of hashing file paths, this is okay, as files with different names but same extension
        // would still hash differently. If I were to PR this to runtime though, this would need fixing.

        if (Vector256.IsHardwareAccelerated && length >= sizeof(nuint) * 8)
            return text.UnstableHashVec256Lower();

        if (Vector128.IsHardwareAccelerated && length >= sizeof(nuint) * 4)
            return text.UnstableHashVec128Lower();
#endif

        return text.UnstableHashNonVectorLower();
    }

    /// <summary>
    ///     [Lowercase/Ignore Case Version]
    ///     Faster hashcode for strings; but does not randomize between application runs.
    ///     Essentially a SIMD'ed FNV-1a.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    /// </remarks>
    [SkipLocalsInit]
    internal static unsafe nuint GetHashCodeUnstableLowerSlow(this ReadOnlySpan<char> text)
    {
#if NET7_0_OR_GREATER
        // If above 1K bytes, hash on heap.
        if (text.Length <= 512) // <= hot path, don't invert branch
        {
            Span<char> asLower = stackalloc char[text.Length];
            text.ToLowerInvariantFast(asLower);
            return UnstableStringHash.GetHashCodeUnstable(asLower);
        }

        var lower = TextInfo.ChangeCase<TextInfo.ToLowerConversion>(text);
        return UnstableStringHash.GetHashCodeUnstable(lower);
#else
        // If above 1K bytes, hash on heap.
        if (text.Length <= 512) // <= hot path, don't invert branch
        {
            Span<char> asLower = stackalloc char[text.Length];
            text.ToLowerInvariant(asLower);
            return UnstableStringHash.GetHashCodeUnstable(asLower);
        }
    #if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        // Some dumb overhead here, but it's unavoidable, since Span can be sourced from Heap String or Array
        fixed (char* first = &text.GetPinnableReference())
        {
            var prms = new ChangeCaseParams(first, text.Length);
            var str = string.Create(text.Length, prms, (span, state) => new ReadOnlySpan<char>(state.First, state.Length).ToLowerInvariant(span));
            return UnstableStringHash.GetHashCodeUnstable(str);
        }
    #else
        // TODO: Improve GetHashCodeUnstableLowerSlow on Framework
        var lower = text.ToString().ToLowerInvariant();
        return lower.AsSpan().GetHashCodeUnstable();
    #endif
#endif
    }

    #if NET7_0_OR_GREATER
    #if NET8_0 // Bug in .NET 8 seems to cause this to not re-jit to tier1 till like 200k calls on Linux x64
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    #endif
    internal static unsafe UIntPtr UnstableHashVec128Lower(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            nuint hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;
            var ptr = (nuint*)(src);

            var prime = Vector128.Create((ulong)0x100000001b3);
            var hash1_128 = Vector128.Create(0xcbf29ce484222325);
            var hash2_128 = Vector128.Create(0xcbf29ce484222325);

            // We "normalize to lowercase" every char by ORing with 0x0020. This casts
            // a very wide net because it will change, e.g., '^' to '~'. But that should
            // be ok because we expect this to be very rare in practice.
            var toLower = Vector128.Create<short>(0x0020).AsUInt64();

            while (length >= sizeof(Vector128<ulong>) / sizeof(char) * 4) // 64 byte chunks.
            {
                length -= (sizeof(Vector128<ulong>) / sizeof(char)) * 4;

                var v0 = Vector128.Load((ulong*)ptr);
                if (!AllCharsInVector128AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_128 = Vector128.Xor(hash1_128, Vector128.BitwiseOr(v0, toLower));
                hash1_128 = HashMultiply128(hash1_128, prime);

                v0 = Vector128.Load((ulong*)ptr + 2);
                if (!AllCharsInVector128AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash2_128 = Vector128.Xor(hash2_128, Vector128.BitwiseOr(v0, toLower));
                hash2_128 = HashMultiply128(hash2_128, prime);

                v0 = Vector128.Load((ulong*)ptr + 4);
                if (!AllCharsInVector128AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_128 = Vector128.Xor(hash1_128, Vector128.BitwiseOr(v0, toLower));
                hash1_128 = HashMultiply128(hash1_128, prime);

                v0 = Vector128.Load((ulong*)ptr + 6);
                if (!AllCharsInVector128AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash2_128 = Vector128.Xor(hash2_128, Vector128.BitwiseOr(v0, toLower));
                hash2_128 = HashMultiply128(hash2_128, prime);
                ptr += (sizeof(Vector128<ulong>) / sizeof(nuint)) * 4;
            }

            while (length >= sizeof(Vector128<ulong>) / sizeof(char)) // 16 byte chunks.
            {
                length -= sizeof(Vector128<ulong>) / sizeof(char);

                var v0 = Vector128.Load((ulong*)ptr);
                if (!AllCharsInVector128AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_128 = Vector128.Xor(hash1_128, Vector128.BitwiseOr(v0, toLower));
                hash1_128 = HashMultiply128(hash1_128, prime);
                ptr += (sizeof(Vector128<ulong>) / sizeof(nuint));
            }

            // Flatten
            hash1_128 ^= hash2_128;
            if (sizeof(nuint) == 8)
            {
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)hash1_128[0];
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (nuint)hash1_128[1];
            }
            else
            {
                Vector128<uint> hash1Uint = hash1_128.AsUInt32();
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[0] * hash1Uint[1]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[2] * hash1Uint[3]);
            }

            // 4/8 byte remainders
            if (sizeof(nuint) == 8)
            {
                while (length >= (sizeof(nuint) / sizeof(char)))
                {
                    length -= (sizeof(nuint) / sizeof(char));

                    var p0 = (ulong)ptr[0];
                    if (!AllCharsInULongAreAscii(p0))
                        goto NotAscii;

                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)(p0 | 0x0020_0020_0020_0020u);
                    ptr += 1;
                }
            }
            else if (sizeof(nuint) == 4)
            {
                while (length >= (sizeof(nuint) / sizeof(char)))
                {
                    length -= (sizeof(nuint) / sizeof(char));

                    var p0 = ptr[0];
                    if (!AllCharsInULongAreAscii(p0))
                        goto NotAscii;

                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (p0 | 0x0020_0020);
                    ptr += 1;
                }
            }
            else
            {
                ThrowHelpers.ThrowArchitectureNotSupportedException();
            }

            return hash1 + (hash2 * 1566083941);
        }

        NotAscii:
            return GetHashCodeUnstableLowerSlow(text);
    }

#if NET8_0 // Bug in .NET 8 seems to cause this to not re-jit to tier1 till like 200k calls on Linux x64
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    internal static unsafe UIntPtr UnstableHashVec256Lower(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            nuint hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;
            var ptr = (nuint*)(src);

            var prime = Vector256.Create((ulong)0x100000001b3);
            var hash1_256 = Vector256.Create(0xcbf29ce484222325);
            var hash2_256 = Vector256.Create(0xcbf29ce484222325);

            // We "normalize to lowercase" every char by ORing with 0x0020. This casts
            // a very wide net because it will change, e.g., '^' to '~'. But that should
            // be ok because we expect this to be very rare in practice.
            var toLower = Vector256.Create<short>(0x0020).AsUInt64();

            while (length >= sizeof(Vector256<ulong>) / sizeof(char) * 4) // 128 byte chunks.
            {
                length -= (sizeof(Vector256<ulong>) / sizeof(char)) * 4;

                var v0 = Vector256.Load((ulong*)ptr);
                if (!AllCharsInVector256AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_256 = Vector256.Xor(hash1_256, Vector256.BitwiseOr(v0, toLower));
                hash1_256 = HashMultiply256(hash1_256, prime);

                v0 = Vector256.Load((ulong*)ptr + 4);
                if (!AllCharsInVector256AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash2_256 = Vector256.Xor(hash2_256, Vector256.BitwiseOr(v0, toLower));
                hash2_256 = HashMultiply256(hash2_256, prime);

                v0 = Vector256.Load((ulong*)ptr + 8);
                if (!AllCharsInVector256AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_256 = Vector256.Xor(hash1_256, Vector256.BitwiseOr(v0, toLower));
                hash1_256 = HashMultiply256(hash1_256, prime);

                v0 = Vector256.Load((ulong*)ptr + 12);
                if (!AllCharsInVector256AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash2_256 = Vector256.Xor(hash2_256, Vector256.BitwiseOr(v0, toLower));
                hash2_256 = HashMultiply256(hash2_256, prime);
                ptr += (sizeof(Vector256<ulong>) / sizeof(nuint)) * 4;
            }

            while (length >= sizeof(Vector256<ulong>) / sizeof(char)) // 32 byte chunks.
            {
                length -= sizeof(Vector256<ulong>) / sizeof(char);

                var v0 = Vector256.Load((ulong*)ptr);
                if (!AllCharsInVector256AreAscii(v0.AsUInt16()))
                    goto NotAscii;

                hash1_256 = Vector256.Xor(hash1_256, Vector256.BitwiseOr(v0, toLower));
                hash1_256 = HashMultiply256(hash1_256, prime);
                ptr += (sizeof(Vector256<ulong>) / sizeof(nuint));
            }

            // Flatten
            hash1_256 = Vector256.Xor(hash1_256, hash2_256);
            if (sizeof(nuint) == 8)
            {
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)hash1_256[0];
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (nuint)hash1_256[1];
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)hash1_256[2];
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (nuint)hash1_256[3];
            }
            else
            {
                Vector256<uint> hash1Uint = hash1_256.AsUInt32();
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[0] * hash1Uint[1]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[2] * hash1Uint[3]);
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[3] * hash1Uint[4]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[5] * hash1Uint[6]);
            }

            // 4/8 byte remainders
            if (sizeof(nuint) == 8)
            {
                while (length >= (sizeof(nuint) / sizeof(char)))
                {
                    length -= (sizeof(nuint) / sizeof(char));

                    var p0 = (ulong)ptr[0];
                    if (!AllCharsInULongAreAscii(p0))
                        goto NotAscii;

                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)(p0 | 0x0020_0020_0020_0020u);
                    ptr += 1;
                }
            }
            else if (sizeof(nuint) == 4)
            {
                while (length >= (sizeof(nuint) / sizeof(char)))
                {
                    length -= (sizeof(nuint) / sizeof(char));

                    var p0 = ptr[0];
                    if (!AllCharsInULongAreAscii(p0))
                        goto NotAscii;

                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (p0 | 0x0020_0020);
                    ptr += 1;
                }
            }
            else
            {
                ThrowHelpers.ThrowArchitectureNotSupportedException();
            }

            return hash1 + (hash2 * 1566083941);
        }

        NotAscii:
            return GetHashCodeUnstableLowerSlow(text);
    }
#endif

    internal static unsafe nuint UnstableHashNonVectorLower(this ReadOnlySpan<char> text)
    {
        // JIT will convert this to direct branch.
        switch (sizeof(nuint))
        {
            case 4:
                return UnstableHashNonVectorLower32(text);
            case 8:
                return UnstableHashNonVectorLower64(text);
            default:
                ThrowHelpers.ThrowArchitectureNotSupportedException();
                return 0;
        }
    }

    internal static unsafe UIntPtr UnstableHashNonVectorLower32(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            const ulong prime = 0x01000193;
            ulong hash1 = 0x811c9dc5;
            var hash2 = hash1;
            var ptr = (uint*)(src);

            // We "normalize to lowercase" every char by ORing with 0x0020. This casts
            // a very wide net because it will change, e.g., '^' to '~'. But that should
            // be ok because we expect this to be very rare in practice.
            const uint normalizeToLowercase = 0x0020_0020; // valid both for big-endian and for little-endian

            // 32 byte
            while (length >= (sizeof(uint) / sizeof(char)) * 8)
            {
                length -= (sizeof(uint) / sizeof(char)) * 8;

                var p0 = ptr[0];
                var p1 = ptr[1];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;

                p0 = ptr[2];
                p1 = ptr[3];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;

                p0 = ptr[4];
                p1 = ptr[5];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;

                p0 = ptr[6];
                p1 = ptr[7];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                ptr += 8;
            }

            // 16 byte
            if (length >= (sizeof(uint) / sizeof(char)) * 4)
            {
                length -= (sizeof(uint) / sizeof(char)) * 4;

                var p0 = ptr[0];
                var p1 = ptr[1];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;

                p0 = ptr[2];
                p1 = ptr[3];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                ptr += 4;
            }

            // 8 byte
            if (length >= (sizeof(uint) / sizeof(char)) * 2)
            {
                length -= (sizeof(uint) / sizeof(char)) * 2;
                uint p0 = ptr[0];
                uint p1 = ptr[1];
                if (!AllCharsInUIntAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                ptr += 2;
            }

            // 4 byte
            if (length >= (sizeof(uint) / sizeof(char)))
            {
                uint p0 = ptr[0];
                if (!AllCharsInUIntAreAscii(p0))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
            }

            // 2 bytes potentially left
            var remainingPtr = (char*)ptr;
            if (length >= 1)
            {
                uint p0 = remainingPtr[0];
                if (!AllCharsInUIntAreAscii(p0))
                    goto NotAscii;

                hash2 = (hash2 ^ (p0 | normalizeToLowercase)) * prime;
            }

            // ReSharper disable once RedundantCast
            return (nuint)(hash1 + (hash2 * 1566083941));
        }

        NotAscii:
            return GetHashCodeUnstableLowerSlow(text);
    }

    internal static unsafe UIntPtr UnstableHashNonVectorLower64(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            const ulong prime = 0x00000100000001B3;
            ulong hash1 = 0xcbf29ce484222325;
            var hash2 = hash1;
            var ptr = (ulong*)(src);

            // We "normalize to lowercase" every char by ORing with 0x0020. This casts
            // a very wide net because it will change, e.g., '^' to '~'. But that should
            // be ok because we expect this to be very rare in practice.
            const ulong normalizeToLowercase = 0x0020_0020_0020_0020; // valid both for big-endian and for little-endian

            // 64 byte
            while (length >= (sizeof(ulong) / sizeof(char)) * 8)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 8;

                var p0 = ptr[0];
                var p1 = ptr[1];
                var p2 = ptr[2];
                var p3 = ptr[3];
                if (!AllCharsInULongAreAscii(p0 | p1 | p2 | p3))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                hash1 = (hash1 ^ (p2 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p3 | normalizeToLowercase)) * prime;

                p0 = ptr[4];
                p1 = ptr[5];
                p2 = ptr[6];
                p3 = ptr[7];
                if (!AllCharsInULongAreAscii(p0 | p1 | p2 | p3))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                hash1 = (hash1 ^ (p2 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p3 | normalizeToLowercase)) * prime;
                ptr += 8;
            }

            // 32 byte
            if (length >= (sizeof(ulong) / sizeof(char)) * 4)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 4;

                var p0 = ptr[0];
                var p1 = ptr[1];
                var p2 = ptr[2];
                var p3 = ptr[3];
                if (!AllCharsInULongAreAscii(p0 | p1 | p2 | p3))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                hash1 = (hash1 ^ (p2 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p3 | normalizeToLowercase)) * prime;
                ptr += 4;
            }

            // 16 byte
            if (length >= (sizeof(ulong) / sizeof(char)) * 2)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 2;

                var p0 = ptr[0];
                var p1 = ptr[1];
                if (!AllCharsInULongAreAscii(p0 | p1))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                ptr += 2;
            }

            // 8 byte
            if (length >= (sizeof(ulong) / sizeof(char)))
            {
                var p0 = ptr[0];
                if (!AllCharsInULongAreAscii(p0))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
            }

            // 2/4/6 bytes left
            var remainingPtr = (char*)ptr;
            if (length >= 2)
            {
                length -= 2;

                var p0 = remainingPtr[0];
                var p1 = remainingPtr[1];
                if (!AllCharsInULongAreAscii((ulong)(p0 | p1)))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
                hash2 = (hash2 ^ (p1 | normalizeToLowercase)) * prime;
                remainingPtr += 2;
            }

            if (length >= 1)
            {
                var p0 = remainingPtr[0];
                if (!AllCharsInULongAreAscii(p0))
                    goto NotAscii;

                hash1 = (hash1 ^ (p0 | normalizeToLowercase)) * prime;
            }

            // ReSharper disable once RedundantCast
            return (nuint)(hash1 + (hash2 * 1566083941));
        }

        NotAscii:
            return GetHashCodeUnstableLowerSlow(text);
    }

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER) && !NET7_0_OR_GREATER
    private unsafe struct ChangeCaseParams(char* first, int length)
    {
        public readonly char* First = first;
        public readonly int Length = length;
    };
#endif
}
