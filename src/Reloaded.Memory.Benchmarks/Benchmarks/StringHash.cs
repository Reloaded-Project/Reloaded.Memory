using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Benchmarks.Utilities;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Internals.Algorithms;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MinColumn]
[MaxColumn]
[MedianColumn]
[DisassemblyDiagnoser(printInstructionAddresses: true)]
[BenchmarkInfo("String Hashing", "Measures the performance of string hashing.", Categories.Performance)]
[SuppressMessage("ReSharper", "RedundantAssignment")]
public class StringHashBenchmark
{
    private const int ItemCount = 10000;

    [Params(4, 8, 12, 16, 64, 96, 128, 256, 1024)] public int CharacterCount { get; set; }

    public string[] Input { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Input = new string[ItemCount];

        for (var x = 0; x < ItemCount; x++)
            Input[x] = StringGenerators.RandomStringUpperWithEmoji(CharacterCount);
    }

    [Benchmark]
    public nuint Custom_Unstable()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = UnstableStringHash.GetHashCodeUnstable(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHash.GetHashCodeUnstable(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHash.GetHashCodeUnstable(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHash.GetHashCodeUnstable(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_Vec256()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = UnstableStringHash.UnstableHashVec256(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHash.UnstableHashVec256(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHash.UnstableHashVec256(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHash.UnstableHashVec256(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_Vec128()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {

            result = UnstableStringHash.UnstableHashVec128(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHash.UnstableHashVec128(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHash.UnstableHashVec128(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHash.UnstableHashVec128(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public nuint Custom_NonVec()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {

            result = UnstableStringHash.UnstableHashNonVector(Input.DangerousGetReferenceAt(x));
            result = UnstableStringHash.UnstableHashNonVector(Input.DangerousGetReferenceAt(x + 1));
            result = UnstableStringHash.UnstableHashNonVector(Input.DangerousGetReferenceAt(x + 2));
            result = UnstableStringHash.UnstableHashNonVector(Input.DangerousGetReferenceAt(x + 3));
        }

        return result;
    }

    [Benchmark]
    public int Runtime_NonRandom_702()
    {
        var result = 0;
        var maxLen = Input.Length / 4;
        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            result = Runtime_70_Impl(Input.DangerousGetReferenceAt(x));
            result = Runtime_70_Impl(Input.DangerousGetReferenceAt(x + 1));
            result = Runtime_70_Impl(Input.DangerousGetReferenceAt(x + 2));
            result = Runtime_70_Impl(Input.DangerousGetReferenceAt(x + 3));
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
            result = Input.DangerousGetReferenceAt(x).GetHashCode();
            result = Input.DangerousGetReferenceAt(x + 1).GetHashCode();
            result = Input.DangerousGetReferenceAt(x + 2).GetHashCode();
            result = Input.DangerousGetReferenceAt(x + 3).GetHashCode();
        }

        return result;
    }

    public unsafe int Runtime_70_Impl(ReadOnlySpan<char> text)
    {
        fixed (char* src = &text.GetPinnableReference())
        {
            // Asserts here for alignment etc. are no longer valid as we are operating on a slice, so memory alignment is not guaranteed.
            uint hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            var ptr = (uint*)src;
            var length = text.Length;

            while (length > 2)
            {
                length -= 4;
                // Where length is 4n-1 (e.g. 3,7,11,15,19) this additionally consumes the null terminator
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr[0];
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[1];
                ptr += 2;
            }

            if (length > 0)
            {
                // Where length is 4n-3 (e.g. 1,5,9,13,17) this additionally consumes the null terminator
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ ptr[0];
            }

            return (int)(hash1 + hash2 * 1566083941);
        }
    }
}
