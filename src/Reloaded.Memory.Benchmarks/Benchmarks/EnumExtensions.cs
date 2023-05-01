using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Memory.Enums;
using Reloaded.Memory.Memory.Interfaces;
using Reloaded.Memory.Memory.Structs;
using Reloaded.Memory.Utility;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Enum Extensions", "Tests whether Memory's Enum Extensions are Zero-Cost.", Categories.ZeroOverhead)]
public unsafe class EnumExtensions
{
    public MemoryProtection[] Protections { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Protections = Enum.GetValues<MemoryProtection>();
    }

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public int ManuallyTest()
    {
        var prot = Protections;
        int numTrue = 0;
        for (int x = 0; x < prot.Length; x++)
        {
            if ((prot[x] & prot[x]) == prot[x])
                numTrue++;
        }

        return numTrue;
    }

    [Benchmark]
    public int TestFast()
    {
        var prot = Protections;
        int numTrue = 0;
        for (int x = 0; x < prot.Length; x++)
        {
            if (prot[x].HasFlagFast(prot[x]))
                numTrue++;
        }

        return numTrue;
    }
}
