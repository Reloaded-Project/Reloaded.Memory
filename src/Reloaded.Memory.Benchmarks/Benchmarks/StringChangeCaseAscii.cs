using System.Diagnostics.CodeAnalysis;
using System.Text;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Benchmarks.Utilities;
using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MinColumn]
[MaxColumn]
[MedianColumn]
[DisassemblyDiagnoser(printInstructionAddresses: true)]
[BenchmarkInfo("String Change Case (ASCII Only)", "Measures the performance of changing the case of an ASCII string using invariant rules.", Categories.Performance)]
[SuppressMessage("ReSharper", "RedundantAssignment")]
public class StringChangeCaseAsciiBenchmark
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
            Input[x] = StringGenerators.RandomStringAsciiMixedCase(CharacterCount);
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
            Ascii.ToLower(Input.DangerousGetReferenceAt(x).AsSpan(), outBuf, out _);
            Ascii.ToLower(Input.DangerousGetReferenceAt(x + 1).AsSpan(), outBuf, out _);
            Ascii.ToLower(Input.DangerousGetReferenceAt(x + 2).AsSpan(), outBuf, out _);
            Ascii.ToLower(Input.DangerousGetReferenceAt(x + 3).AsSpan(), outBuf, out _);
        }

        return result;
    }

    [Benchmark]
    public nuint ToUpperInvariantFast_Custom()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        Span<char> outBuf = stackalloc char[CharacterCount];

        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            Input.DangerousGetReferenceAt(x).AsSpan().ToUpperInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 1).AsSpan().ToUpperInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 2).AsSpan().ToUpperInvariantFast(outBuf);
            Input.DangerousGetReferenceAt(x + 3).AsSpan().ToUpperInvariantFast(outBuf);
        }

        return result;
    }

    [Benchmark]
    public nuint ToUpperInvariant_Runtime()
    {
        nuint result = 0;
        var maxLen = Input.Length / 4;
        Span<char> outBuf = stackalloc char[CharacterCount];

        // unroll
        for (var x = 0; x < maxLen; x += 4)
        {
            Ascii.ToUpper(Input.DangerousGetReferenceAt(x).AsSpan(), outBuf, out _);
            Ascii.ToUpper(Input.DangerousGetReferenceAt(x + 1).AsSpan(), outBuf, out _);
            Ascii.ToUpper(Input.DangerousGetReferenceAt(x + 2).AsSpan(), outBuf, out _);
            Ascii.ToUpper(Input.DangerousGetReferenceAt(x + 3).AsSpan(), outBuf, out _);
        }

        return result;
    }
}

