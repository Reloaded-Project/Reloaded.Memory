using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Utilities;
#if NET7_0_OR_GREATER
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Reloaded.Memory.Internals.Algorithms;

/// <summary>
///     Unstable string hash algorithm.
///     Each implementation prioritises speed, and different machines may produce different results.
/// </summary>
internal static class UnstableStringHash
{
    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    ///     Essentially a SIMD'ed FNV-1a.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    /// </remarks>
    [ExcludeFromCodeCoverage] // "Cannot be accurately measured without multiple architectures. Known good impl." This is still tested tho.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe nuint GetHashCodeUnstable(this ReadOnlySpan<char> text)
    {
#if NET7_0_OR_GREATER
        var length = text.Length; // Span has no guarantee of null terminator.
        // For short strings below size of nuint, we need separate approach; so we use legacy runtime approach
        // for said cold case.

        // Note: The `/ sizeof(char)` accounts that length is measured in 2-byte chars, not bytes.

        // Note. In these SIMD implementations we leave some (< sizeof(nuint)) data from the hash.
        // For our use of hashing file paths, this is okay, as files with different names but same extension
        // would still hash differently. If I were to PR this to runtime though, this would need fixing.

        if (Vector256.IsHardwareAccelerated && length >= sizeof(nuint) * 8)
            return text.UnstableHashVec256();

        if (Vector128.IsHardwareAccelerated && length >= sizeof(nuint) * 4)
            return text.UnstableHashVec128();
#endif

        return text.UnstableHashNonVector();
    }

    #if NET7_0_OR_GREATER
#if NET8_0 // Bug in .NET 8 seems to cause this to not re-jit to tier1 till like 200k calls on Linux x64
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    internal static unsafe UIntPtr UnstableHashVec128(this ReadOnlySpan<char> text)
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

            while (length >= sizeof(Vector128<ulong>) / sizeof(char) * 4) // 64 byte chunks.
            {
                length -= (sizeof(Vector128<ulong>) / sizeof(char)) * 4;
                hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr));
                hash1_128 = HashMultiply128(hash1_128, prime);

                hash2_128 = Vector128.Xor(hash2_128, Vector128.Load((ulong*)ptr + 2));
                hash2_128 = HashMultiply128(hash2_128, prime);

                hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr + 4));
                hash1_128 = HashMultiply128(hash1_128, prime);

                hash2_128 = Vector128.Xor(hash2_128, Vector128.Load((ulong*)ptr + 6));
                hash2_128 = HashMultiply128(hash2_128, prime);
                ptr += (sizeof(Vector128<ulong>) / sizeof(nuint)) * 4;
            }

            while (length >= sizeof(Vector128<ulong>) / sizeof(char)) // 16 byte chunks.
            {
                length -= sizeof(Vector128<ulong>) / sizeof(char);
                hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr));
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
                var hash1Uint = hash1_128.AsUInt32();
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[0] * hash1Uint[1]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[2] * hash1Uint[3]);
            }

            // 4/8 byte remainders
            while (length >= (sizeof(nuint) / sizeof(char)))
            {
                length -= (sizeof(nuint) / sizeof(char));
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                ptr += 1;
            }

            return hash1 + (hash2 * 0x5D588B65);
        }
    }

#if NET8_0 // Bug in .NET 8 seems to cause this to not re-jit to tier1 till like 200k calls on Linux x64
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    internal static unsafe UIntPtr UnstableHashVec256(this ReadOnlySpan<char> text)
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

            while (length >= sizeof(Vector256<ulong>) / sizeof(char) * 4) // 128 byte chunks.
            {
                length -= (sizeof(Vector256<ulong>) / sizeof(char)) * 4;
                hash1_256 = Vector256.Xor(hash1_256, Vector256.Load((ulong*)ptr));
                hash1_256 = HashMultiply256(hash1_256, prime);

                hash2_256 = Vector256.Xor(hash2_256, Vector256.Load((ulong*)ptr + 4));
                hash2_256 = HashMultiply256(hash2_256, prime);

                hash1_256 = Vector256.Xor(hash1_256, Vector256.Load((ulong*)ptr + 8));
                hash1_256 = HashMultiply256(hash1_256, prime);

                hash2_256 = Vector256.Xor(hash2_256, Vector256.Load((ulong*)ptr + 12));
                hash2_256 = HashMultiply256(hash2_256, prime);
                ptr += (sizeof(Vector256<ulong>) / sizeof(nuint)) * 4;
            }

            while (length >= sizeof(Vector256<ulong>) / sizeof(char)) // 32 byte chunks.
            {
                length -= sizeof(Vector256<ulong>) / sizeof(char);
                hash1_256 = Vector256.Xor(hash1_256, Vector256.Load((ulong*)ptr));
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
                var hash1Uint = hash1_256.AsUInt32();
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[0] * hash1Uint[1]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[2] * hash1Uint[3]);
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[3] * hash1Uint[4]);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[5] * hash1Uint[6]);
            }

            // 4/8 byte remainders
            while (length >= (sizeof(nuint) / sizeof(char)))
            {
                length -= (sizeof(nuint) / sizeof(char));
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                ptr += 1;
            }

            return hash1 + (hash2 * 1566083941);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector128<ulong> HashMultiply128(Vector128<ulong> a, Vector128<ulong> b)
    {
        // See comment in HashMultiply256
        if (Sse2.IsSupported)
            return Sse2.Multiply(a.AsUInt32(), b.AsUInt32()).AsUInt64();

        return Vector128.Multiply(a.AsUInt32(), b.AsUInt32()).AsUInt64();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector256<ulong> HashMultiply256(Vector256<ulong> a, Vector256<ulong> b)
    {
        // On AVX2, we want VPMULUDQ.
        // Unfortunately the Vector256 fallback can't produce this,
        // so we fallback to multiplying 32-bit ints, which isn't as good, but still not terrible.
        if (Avx2.IsSupported)
            return Avx2.Multiply(a.AsUInt32(), b.AsUInt32()).AsUInt64();

        return Vector256.Multiply(a.AsUInt32(), b.AsUInt32()).AsUInt64();
    }
    #endif

    internal static unsafe UIntPtr UnstableHashNonVector(this ReadOnlySpan<char> text)
    {
        // JIT will convert this to direct branch.
        switch (sizeof(nuint))
        {
            case 4:
                return UnstableHashNonVector32(text);
            case 8:
                return UnstableHashNonVector64(text);
            default:
                ThrowHelpers.ThrowArchitectureNotSupportedException();
                return (nuint) 0;
        }
    }

    internal static unsafe UIntPtr UnstableHashNonVector32(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            const ulong prime = 0x01000193;
            ulong hash1 = 0x811c9dc5;
            var hash2 = hash1;
            var ptr = (uint*)(src);

            // Non-vector accelerated version here.
            // 32 byte loop
            while (length >= (sizeof(uint) / sizeof(char)) * 8)
            {
                length -= (sizeof(uint) / sizeof(char)) * 8;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                hash1 = (hash1 ^ ptr[2]) * prime;
                hash2 = (hash2 ^ ptr[3]) * prime;
                hash1 = (hash1 ^ ptr[4]) * prime;
                hash2 = (hash2 ^ ptr[5]) * prime;
                hash1 = (hash1 ^ ptr[6]) * prime;
                hash2 = (hash2 ^ ptr[7]) * prime;
                ptr += 8;
            }

            // 16 byte
            if (length >= (sizeof(uint) / sizeof(char)) * 4)
            {
                length -= (sizeof(uint) / sizeof(char)) * 4;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                hash1 = (hash1 ^ ptr[2]) * prime;
                hash2 = (hash2 ^ ptr[3]) * prime;
                ptr += 4;
            }

            // 8 byte
            if (length >= (sizeof(uint) / sizeof(char)) * 2)
            {
                length -= (sizeof(uint) / sizeof(char)) * 2;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                ptr += 2;
            }

            // 4 byte
            if (length >= (sizeof(uint) / sizeof(char)))
            {
                length -= (sizeof(uint) / sizeof(char));
                hash1 = (hash1 ^ ptr[0]) * prime;
                ptr += 1;
            }

            // 2 bytes potentially left
            var remainingPtr = (char*)ptr;
            if (length >= 1)
                hash2 = (hash2 ^ remainingPtr[0]) * prime;

            return (nuint)(hash1 + (hash2 * 1566083941));
        }
    }

    internal static unsafe UIntPtr UnstableHashNonVector64(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
            const ulong prime = 0x00000100000001B3;
            ulong hash1 = 0xcbf29ce484222325;
            var hash2 = hash1;
            var ptr = (ulong*)(src);

            // Non-vector accelerated version here.
            // 64 byte loop
            while (length >= (sizeof(ulong) / sizeof(char)) * 8)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 8;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                hash1 = (hash1 ^ ptr[2]) * prime;
                hash2 = (hash2 ^ ptr[3]) * prime;
                hash1 = (hash1 ^ ptr[4]) * prime;
                hash2 = (hash2 ^ ptr[5]) * prime;
                hash1 = (hash1 ^ ptr[6]) * prime;
                hash2 = (hash2 ^ ptr[7]) * prime;
                ptr += 8;
            }

            // 32 byte
            if (length >= (sizeof(ulong) / sizeof(char)) * 4)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 4;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                hash1 = (hash1 ^ ptr[2]) * prime;
                hash2 = (hash2 ^ ptr[3]) * prime;
                ptr += 4;
            }

            // 16 byte
            if (length >= (sizeof(ulong) / sizeof(char)) * 2)
            {
                length -= (sizeof(ulong) / sizeof(char)) * 2;
                hash1 = (hash1 ^ ptr[0]) * prime;
                hash2 = (hash2 ^ ptr[1]) * prime;
                ptr += 2;
            }

            // 8 byte
            if (length >= (sizeof(ulong) / sizeof(char)))
            {
                length -= (sizeof(ulong) / sizeof(char));
                hash1 = (hash1 ^ ptr[0]) * prime;
                ptr += 1;
            }

            // 2/4/6 bytes left
            var remainingPtr = (char*)ptr;
            if (length >= 2)
            {
                length -= 2;
                hash1 = (hash1 ^ remainingPtr[0]) * prime;
                hash2 = (hash2 ^ remainingPtr[1]) * prime;
                remainingPtr += 2;
            }

            if (length >= 1)
                hash2 = (hash2 ^ remainingPtr[0]) * prime;

            return (nuint)(hash1 + (hash2 * 1566083941));
        }
    }
}
