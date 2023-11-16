using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Benchmarks.Utilities;
using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MinColumn]
[MaxColumn]
[MedianColumn]
[DisassemblyDiagnoser(printInstructionAddresses: true)]
[BenchmarkInfo("String Change Case (Unicode Only)", "Measures the overhead of a failed accelerated change case.", Categories.Performance)]
[SuppressMessage("ReSharper", "RedundantAssignment")]
public class StringChangeCaseUnicodeOnlyBenchmark
{
    private static readonly Random _random = new();
    private const int ItemCount = 10000;

    [Params(4, 12, 32, 64)] public int CharacterCount { get; set; }

    public string[] Input { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Input = new string[ItemCount];

        for (var x = 0; x < ItemCount; x++)
            Input[x] = StringGenerators.RandomStringOfProblematicCharacters(CharacterCount);
    }

    [Benchmark]
    public nuint ToLowerInvariantFast_Custom()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        Span<char> outBuf = stackalloc char[CharacterCount];

        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            Input.DangerousGetReferenceAt(x).AsSpan().ToLowerInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 1).AsSpan().ToLowerInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 2).AsSpan().ToLowerInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 3).AsSpan().ToLowerInvariantFast(outBuf);
        }

        return result;
    }

    [Benchmark]
    public nuint ToLowerInvariant_Runtime()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        Span<char> outBuf = stackalloc char[CharacterCount];

        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            Input.DangerousGetReferenceAt(x).AsSpan().ToLowerInvariant(outBuf);
            Input.DangerousGetReferenceAt(x + 1).AsSpan().ToLowerInvariant(outBuf);
            Input.DangerousGetReferenceAt(x + 2).AsSpan().ToLowerInvariant(outBuf);
            Input.DangerousGetReferenceAt(x + 3).AsSpan().ToLowerInvariant(outBuf);
        }

        return result;
    }
}

