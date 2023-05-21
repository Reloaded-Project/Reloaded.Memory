using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Memory Allocation Extensions", "Checks if memory allocation extensions are minimal cost.",
    Categories.ZeroOverhead)]
public class MemoryAllocateExtension
{
    // Must be divisible by 2
    public const int DataSize = 4096;

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public nuint Allocate_Direct()
    {
        var memory = new Reloaded.Memory.Memory();
        MemoryAllocation alloc = memory.Allocate(DataSize);
        _ = memory.Free(alloc);
        return alloc.Address;
    }

    [Benchmark]
    public nuint Allocate_Disposable()
    {
        var memory = new Reloaded.Memory.Memory();
        DisposableMemoryAllocation<Reloaded.Memory.Memory> alloc = memory.AllocateDisposable(DataSize);
        alloc.Dispose();
        return alloc.Allocation.Address;
    }

    [Benchmark]
    public nuint Allocate_Disposable_Using()
    {
        var memory = new Reloaded.Memory.Memory();
        using DisposableMemoryAllocation<Reloaded.Memory.Memory> alloc = memory.AllocateDisposable(DataSize);
        return alloc.Allocation.Address;
    }
}
