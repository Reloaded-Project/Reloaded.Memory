using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Enums;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Memory Protection Extensions", "Checks if memory protection change extensions are minimal cost.",
    Categories.ZeroOverhead)]
public class MemoryChangeProtectionExtension
{
    // Must be divisible by 2
    public const int DataSize = 4096;

    public MemoryAllocation Alloc { get; set; }

    [GlobalSetup]
    public void Setup() => Alloc = new Reloaded.Memory.Memory().Allocate(DataSize);

    [GlobalCleanup]
    public bool Cleanup() => new Reloaded.Memory.Memory().Free(Alloc);

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public nuint ChangePermission_Direct()
    {
        var memory = new Reloaded.Memory.Memory();
        var oldProtection = memory.ChangeProtection(Alloc.Address, (int)Alloc.Length, MemoryProtection.Read);
        memory.ChangeProtectionRaw(Alloc.Address, (int)Alloc.Length, oldProtection);
        return oldProtection;
    }

    [Benchmark]
    public nuint ChangePermission_Disposable()
    {
        var memory = new Reloaded.Memory.Memory();
        DisposableMemoryProtection<Reloaded.Memory.Memory> oldProtection =
            memory.ChangeProtectionDisposable(Alloc.Address, (int)Alloc.Length, MemoryProtection.Read);
        oldProtection.Dispose();
        return oldProtection.OriginalProtection;
    }

    [Benchmark]
    public nuint ChangePermission_Disposable_Using()
    {
        var memory = new Reloaded.Memory.Memory();
        using DisposableMemoryProtection<Reloaded.Memory.Memory> oldProtection =
            memory.ChangeProtectionDisposable(Alloc.Address, (int)Alloc.Length, MemoryProtection.Read);
        return oldProtection.OriginalProtection;
    }
}
