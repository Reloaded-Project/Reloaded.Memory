using System.Diagnostics.CodeAnalysis;
using Reloaded.Memory.Internals;
using Reloaded.Memory.Utilities;
using Reloaded.Memory.Utilities.License;
#if NET7_0_OR_GREATER
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif
#if NETSTANDARD
#endif

namespace Reloaded.Memory.Extensions;

/// <summary>
///     Helpers for working with the <see cref="string" /> type.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    ///     Returns a reference to the first element within a given <see cref="string" />, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance.</param>
    /// <returns>
    ///     A reference to the first element within <paramref name="text" />, or the location it would have used, if
    ///     <paramref name="text" /> is empty.
    /// </returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in
    ///     case the returned value is dereferenced.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static ref char DangerousGetReference(this string text)
    {
#if NET6_0_OR_GREATER
        return ref Unsafe.AsRef(text.GetPinnableReference());
#else
        return ref MemoryMarshal.GetReference(text.AsSpan());
#endif
    }

    /// <summary>
    ///     Returns a reference to an element at a specified index within a given <see cref="string" />, with no bounds checks.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance.</param>
    /// <param name="i">The index of the element to retrieve within <paramref name="text" />.</param>
    /// <returns>A reference to the element within <paramref name="text" /> at the index specified by <paramref name="i" />.</returns>
    /// <remarks>
    ///     This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the
    ///     <paramref name="i" /> parameter is valid.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static ref char DangerousGetReferenceAt(this string text, int i)
    {
#if NET6_0_OR_GREATER
        ref char r0 = ref Unsafe.AsRef(text.GetPinnableReference());
#else
        ref var r0 = ref MemoryMarshal.GetReference(text.AsSpan());
#endif
        ref var ri = ref Unsafe.Add(ref r0, (nint)(uint)i);

        return ref ri;
    }

    /// <summary>
    ///     Counts the number of occurrences of a given character into a target <see cref="string" /> instance.
    /// </summary>
    /// <param name="text">The input <see cref="string" /> instance to read.</param>
    /// <param name="c">The character to look for.</param>
    /// <returns>The number of occurrences of <paramref name="c" /> in <paramref name="text" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ExcludeFromCodeCoverage] // From CommunityToolkit.HighPerformance
    [MITLicense]
    public static int Count(this string text, char c)
    {
        ref var r0 = ref text.DangerousGetReference();
        var length = (nint)(uint)text.Length;

        return (int)SpanHelper.Count(ref r0, length, c);
    }

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    /// </remarks>
    public static nuint GetHashCodeFast(this string text) => text.AsSpan().GetHashCodeFast();

    /// <summary>
    ///     Faster hashcode for strings; but does not randomize between application runs.
    /// </summary>
    /// <param name="text">The string for which to get hash code for.</param>
    /// <remarks>
    ///     'Use this if and only if 'Denial of Service' attacks are not a concern (i.e. never used for free-form user input),
    ///     or are otherwise mitigated.
    /// </remarks>
    [ExcludeFromCodeCoverage] // "Cannot be accurately measured without multiple architectures. Known good impl." This is still tested tho.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static unsafe nuint GetHashCodeFast(this ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            var length = text.Length; // Span has no guarantee of null terminator.
#if NET7_0_OR_GREATER
            // For short strings below size of nuint, we need separate approach; so we use legacy runtime approach
            // for said cold case.
            if (length >= sizeof(nuint) / sizeof(char))
            {
                nuint hash1 = (5381 << 16) + 5381;
                nuint hash2 = hash1;

                // I tried aligning the data here; but it didn't help much perf wise
                // despite being 3-4 instructions. I do not know why.
                nuint* ptr = (nuint*)(src);

                // Note. In this implementations we leave some (< sizeof(nuint)) data from the hash.

                // For our use of hashing file paths, this is okay, as files with different names but same extension
                // would still hash differently. If I were to PR this to runtime though, this would need fixing.

                if (Avx2.IsSupported || Vector128.IsHardwareAccelerated)
                {
                    // AVX Version
                    // Ideally I could rewrite this in full Vector256 but I don't know how to get it to emit VPMULUDQ for the multiply operation.
                    if (Avx2.IsSupported &&
                        length >= sizeof(Vector256<ulong>) / sizeof(char) * 4) // over 128 bytes + AVX
                    {
                        var prime = Vector256.Create((ulong)0x100000001b3);
                        var hash1Avx = Vector256.Create(0xcbf29ce484222325);
                        var hash2Avx = Vector256.Create(0xcbf29ce484222325);

                        while (length >= sizeof(Vector256<ulong>) / sizeof(char) * 4) // 128 byte chunks.
                        {
                            length -= (sizeof(Vector256<ulong>) / sizeof(char)) * 4;
                            hash1Avx = Avx2.Xor(hash1Avx, Avx.LoadVector256((ulong*)ptr));
                            hash1Avx = Avx2.Multiply(hash1Avx.AsUInt32(), prime.AsUInt32());

                            hash2Avx = Avx2.Xor(hash2Avx, Avx.LoadVector256((ulong*)ptr + 4));
                            hash2Avx = Avx2.Multiply(hash2Avx.AsUInt32(), prime.AsUInt32());

                            hash1Avx = Avx2.Xor(hash1Avx, Avx.LoadVector256((ulong*)ptr + 8));
                            hash1Avx = Avx2.Multiply(hash1Avx.AsUInt32(), prime.AsUInt32());

                            hash2Avx = Avx2.Xor(hash2Avx, Avx.LoadVector256((ulong*)ptr + 12));
                            hash2Avx = Avx2.Multiply(hash2Avx.AsUInt32(), prime.AsUInt32());
                            ptr += (sizeof(Vector256<ulong>) / sizeof(nuint)) * 4;
                        }

                        while (length >= sizeof(Vector256<ulong>) / sizeof(char)) // 32 byte chunks.
                        {
                            length -= sizeof(Vector256<ulong>) / sizeof(char);
                            hash1Avx = Avx2.Xor(hash1Avx, Avx.LoadVector256((ulong*)ptr));
                            hash1Avx = Avx2.Multiply(hash1Avx.AsUInt32(), prime.AsUInt32());
                            ptr += (sizeof(Vector256<ulong>) / sizeof(nuint));
                        }

                        // Flatten
                        hash1Avx = Avx2.Xor(hash1Avx, hash2Avx);
                        if (sizeof(nuint) == 8)
                        {
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)hash1Avx[0];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (nuint)hash1Avx[1];
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (nuint)hash1Avx[2];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (nuint)hash1Avx[3];
                        }
                        else
                        {
                            var hash1Uint = hash1Avx.AsUInt32();
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[0] * hash1Uint[1]);
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[2] * hash1Uint[3]);
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (hash1Uint[3] * hash1Uint[4]);
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (hash1Uint[5] * hash1Uint[6]);
                        }

                        // 4/8 byte remainders
                        while (length >= (sizeof(nuint) / sizeof(char)))
                        {
                            length -= (sizeof(nuint) / sizeof(char));
                            hash1 = (nuint)((BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0]);
                            ptr += 1;
                        }

                        return hash1 + (hash2 * 1566083941);
                    }

                    // Over 64 bytes + SSE. Supported on all x64 processors
                    if (Vector128.IsHardwareAccelerated && length >= sizeof(Vector128<ulong>) / sizeof(char) * 4)
                    {
                        var prime = Vector128.Create((ulong)0x100000001b3);
                        var hash1_128 = Vector128.Create(0xcbf29ce484222325);
                        var hash2_128 = Vector128.Create(0xcbf29ce484222325);

                        while (length >= sizeof(Vector128<ulong>) / sizeof(char) * 4) // 64 byte chunks.
                        {
                            length -= (sizeof(Vector128<ulong>) / sizeof(char)) * 4;
                            hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr));
                            hash1_128 = Vector128.Multiply(hash1_128.AsUInt32(), prime.AsUInt32()).AsUInt64();

                            hash2_128 = Vector128.Xor(hash2_128, Vector128.Load((ulong*)ptr + 2));
                            hash2_128 = Vector128.Multiply(hash2_128.AsUInt32(), prime.AsUInt32()).AsUInt64();

                            hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr + 4));
                            hash1_128 = Vector128.Multiply(hash1_128.AsUInt32(), prime.AsUInt32()).AsUInt64();

                            hash2_128 = Vector128.Xor(hash2_128, Vector128.Load((ulong*)ptr + 6));
                            hash2_128 = Vector128.Multiply(hash2_128.AsUInt32(), prime.AsUInt32()).AsUInt64();
                            ptr += (sizeof(Vector128<ulong>) / sizeof(nuint)) * 4;
                        }

                        while (length >= sizeof(Vector128<ulong>) / sizeof(char)) // 16 byte chunks.
                        {
                            length -= sizeof(Vector128<ulong>) / sizeof(char);
                            hash1_128 = Vector128.Xor(hash1_128, Vector128.Load((ulong*)ptr));
                            hash1_128 = Vector128.Multiply(hash1_128.AsUInt32(), prime.AsUInt32()).AsUInt64();
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

                        return hash1 + (hash2 * 1566083941);
                    }

                    if (sizeof(nuint) == 8) // 64-bit. Max 8 operations. (8 * 8 = 64bytes)
                    {
                        // 16 byte loop
                        while (length >= (sizeof(nuint) / sizeof(char)) * 2)
                        {
                            length -= (sizeof(nuint) / sizeof(char)) * 2;
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                            ptr += 2;
                        }

                        if (length >= sizeof(nuint) / sizeof(char))
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];

                        return hash1 + (hash2 * 1566083941);
                    }
                    else if (sizeof(nuint) == 4) // 32-bit. Max 16 operations (16 * 4 = 64 bytes)
                    {
                        // 16 byte loop
                        while (length >= (sizeof(nuint) / sizeof(char)) * 4)
                        {
                            length -= (sizeof(nuint) / sizeof(char)) * 4;
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[2];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[3];
                            ptr += 4;
                        }

                        // 8 byte
                        if (length >= (sizeof(nuint) / sizeof(char)) * 2)
                        {
                            length -= (sizeof(nuint) / sizeof(char)) * 2;
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                            hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                            ptr += 2;
                        }

                        // 4 byte
                        if (length >= (sizeof(nuint) / sizeof(char)))
                            hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];

                        return hash1 + (hash2 * 1566083941);
                    }

                    // The future is now.
                    return NonRandomizedHashCode_Fallback(src, length);
                }

                // Non-vector accelerated version here.
                // 32/64 byte loop
                while (length >= (sizeof(nuint) / sizeof(char)) * 8)
                {
                    length -= (sizeof(nuint) / sizeof(char)) * 8;
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[2];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[3];
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[4];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[5];
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[6];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[7];
                    ptr += 8;
                }

                // 16/32 byte
                if (length >= (sizeof(nuint) / sizeof(char)) * 4)
                {
                    length -= (sizeof(nuint) / sizeof(char)) * 4;
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[2];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[3];
                    ptr += 4;
                }

                // 8/16 byte
                if (length >= (sizeof(nuint) / sizeof(char)) * 2)
                {
                    length -= (sizeof(nuint) / sizeof(char)) * 2;
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                    hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                    ptr += 2;
                }

                // 4/8 byte
                if (length >= (sizeof(nuint) / sizeof(char)))
                    hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];

                return hash1 + (hash2 * 1566083941);
            }
#endif
            return NonRandomizedHashCode_Fallback(src, length);
        }
    }

    [ExcludeFromCodeCoverage]
    private static unsafe nuint NonRandomizedHashCode_Fallback(char* src, int length)
    {
        // -1 because we cannot assume string has null terminator at end unlike runtime.
        length -= 1;

        // Version for when input data is smaller than native int. This one is taken from the runtime.
        // For tiny strings like 'C:'
        uint hash1 = (5381 << 16) + 5381;
        var hash2 = hash1;
        var ptr = (uint*)src;

        while (length > 2)
        {
            length -= 4;
            // Where length is 4n-1 (e.g. 3,7,11,15,19) this additionally consumes the null terminator
            hash1 = (Polyfills.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
            hash2 = (Polyfills.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
            ptr += 2;
        }

        if (length > 0)
        {
            // Where length is 4n-3 (e.g. 1,5,9,13,17) this additionally consumes the null terminator
            hash2 = (Polyfills.RotateLeft(hash2, 5) + hash2) ^ ptr[0];
        }

        return hash1 + hash2 * 1566083941;
    }
}
