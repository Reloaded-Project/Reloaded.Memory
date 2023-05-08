using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Enums;
using Reloaded.Memory.Extensions;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Enum Extensions", "Tests whether Memory's Enum Extensions are Zero-Cost.", Categories.ZeroOverhead)]
public class EnumExtensions
{
    public MemoryProtection[] Protections { get; set; } = null!;

    [GlobalSetup]
    public void Setup() => Protections = Enum.GetValues<MemoryProtection>();

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public int ManuallyTest()
    {
        MemoryProtection[] prot = Protections;
        var numTrue = 0;
        for (var x = 0; x < prot.Length; x++)
        {
            if ((prot[x] & prot[x]) == prot[x])
                numTrue++;
        }

        return numTrue;
    }

    [Benchmark]
    public int TestFast()
    {
        MemoryProtection[] prot = Protections;
        var numTrue = 0;
        for (var x = 0; x < prot.Length; x++)
        {
            if (prot[x].HasFlagFast(prot[x]))
                numTrue++;
        }

        return numTrue;
    }
}
