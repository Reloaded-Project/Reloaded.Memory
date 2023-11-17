using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Benchmarks.Utilities;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Internals.Algorithms;
using Reloaded.Memory.Internals.Backports.System.Text.Unicode;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[ShortRunJob]
[MinColumn]
[MaxColumn]
[MedianColumn]
[DisassemblyDiagnoser(printInstructionAddresses: true)]
[BenchmarkInfo("String Hashing (Ignore Case)", "Measures the performance of string hashing using invariant case insensitive.", Categories.Performance)]
[SuppressMessage("ReSharper", "RedundantAssignment")]
public class StringHashIgnoreCaseBenchmark
{
    private const int ItemCount = 10000;

    [Params(4, 8, 16, 32, 64, 128, 256)] public int CharacterCount { get; set; }

    public string[] Input { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Input = new string[ItemCount];

        for (var x = 0; x < ItemCount; x++)
            Input[x] = StringGenerators.RandomStringAsciiMixedCase(CharacterCount);
    }

    [Benchmark]
    public nuint Custom_UnstableLower()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = UnstableStringHashLower.GetHashCodeUnstableLower(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHashLower.GetHashCodeUnstableLower(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHashLower.GetHashCodeUnstableLower(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHashLower.GetHashCodeUnstableLower(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_Avx2Lower()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = UnstableStringHashLower.UnstableHashVec256Lower(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHashLower.UnstableHashVec256Lower(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHashLower.UnstableHashVec256Lower(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHashLower.UnstableHashVec256Lower(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_Vec128Lower()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {

            result = UnstableStringHashLower.UnstableHashVec128Lower(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHashLower.UnstableHashVec128Lower(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHashLower.UnstableHashVec128Lower(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHashLower.UnstableHashVec128Lower(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_NonVecLower()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {

            result = UnstableStringHashLower.UnstableHashNonVectorLower(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHashLower.UnstableHashNonVectorLower(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHashLower.UnstableHashNonVectorLower(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHashLower.UnstableHashNonVectorLower(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public int Runtime_NonRandom_NET8()
    {
        var result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = Runtime_80_Impl(Input.DangerousGetReferenceAt(x));
            result = Runtime_80_Impl(Input.DangerousGetReferenceAt(x + 1));
            result = Runtime_80_Impl(Input.DangerousGetReferenceAt(x + 2));
            result = Runtime_80_Impl(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public int Runtime_Current()
    {
        var result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = Input.DangerousGetReferenceAt(x).GetHashCode(StringComparison.OrdinalIgnoreCase);
            result = Input.DangerousGetReferenceAt(x + 1).GetHashCode(StringComparison.OrdinalIgnoreCase);
            result = Input.DangerousGetReferenceAt(x + 2).GetHashCode(StringComparison.OrdinalIgnoreCase);
            result = Input.DangerousGetReferenceAt(x + 3).GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        return result;
    }

    /// <summary>
    /// Non-Random Runtime Implementation from .NET 8.0, from `src/libraries/System.Private.CoreLib/src/System/String.Comparison.cs`
    /// </summary>
    internal unsafe int Runtime_80_Impl(ReadOnlySpan<char> text)
    {
        uint hash1 = (5381 << 16) + 5381;
        var hash2 = hash1;

        fixed (char* src = &text.GetPinnableReference())
        {
            // Debug.Assert(src[this.Length] == '\0', "src[this.Length] == '\\0'");
            // Debug.Assert(((int)src) % 4 == 0, "Managed string should start at 4 bytes boundary");

            var ptr = (uint*)src;
            var length = text.Length;

            // We "normalize to lowercase" every char by ORing with 0x0020. This casts
            // a very wide net because it will change, e.g., '^' to '~'. But that should
            // be ok because we expect this to be very rare in practice.
            const uint normalizeToLowercase = 0x0020_0020u; // valid both for big-endian and for little-endian

            while (length > 2)
            {
                uint p0 = ptr[0];
                uint p1 = ptr[1];
                if (!Utf16Utility.AllCharsInUInt32AreAscii(p0 | p1))
                    goto NotAscii;

                length -= 4;
                // Where length is 4n-1 (e.g. 3,7,11,15,19) this additionally consumes the null terminator
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ (p0 | normalizeToLowercase);
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (p1 | normalizeToLowercase);
                ptr += 2;
            }

            if (length > 0)
            {
                var p0 = ptr[0];
                if (!Utf16Utility.AllCharsInUInt32AreAscii(p0))
                    goto NotAscii;

                // Where length is 4n-3 (e.g. 1,5,9,13,17) this additionally consumes the null terminator
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ (p0 | normalizeToLowercase);
            }
        }

        return (int)(hash1 + (hash2 * 1566083941));

    NotAscii:
        ThrowHelpers.ThrowPlatformNotSupportedException();
        return 0;
    }

    /// <summary>
    ///     Port from Runtime (.NET 8)
    /// </summary>
    internal static class Utf16Utility
    {
        /// <summary>
        /// Returns true iff the UInt32 represents two ASCII UTF-16 characters in machine endianness.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AllCharsInUInt32AreAscii(uint value) => (value & ~0x007F_007Fu) == 0;
    }
}
