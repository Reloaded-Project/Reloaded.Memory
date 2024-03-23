using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Memory Read Write", "Tests whether Memory's Read/Write Functionality is Zero-Cost.",
    Categories.ZeroOverhead)]
public unsafe class Memory
{
    // Must be divisible by 2
    public const int DataSize = 4096;

    public MemoryAllocation Alloc { get; set; }

    [GlobalSetup]
    public void Setup() => Alloc = new Reloaded.Memory.Memory().Allocate(DataSize);

    [GlobalCleanup]
    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
    public void Cleanup() => new Reloaded.Memory.Memory().Free(Alloc);

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public nuint ReadViaPointer()
    {
        var ptr = (nuint*)Alloc.Address;
        var maxAddress = (nuint*)(Alloc.Address + Alloc.Length);
        nuint result = 0;

        while (ptr < maxAddress)
        {
            result += *ptr;
            ptr += 1;
        }

        return (nuint)(result + ptr);
    }

    [Benchmark]
    public nuint ReadViaMemory()
    {
        var memory = Reloaded.Memory.Memory.Instance;
        var ptr = (nuint*)Alloc.Address;
        var maxAddress = (nuint*)(Alloc.Address + Alloc.Length);
        nuint result = 0;

        while (ptr < maxAddress)
        {
            result += memory.Read<nuint>((nuint)ptr);
            ptr += 1;
        }

        return (nuint)(result + ptr);
    }

    [Benchmark]
    public nuint ReadViaMemory_ViaOutParameter()
    {
        var memory = new Reloaded.Memory.Memory();
        var ptr = (nuint*)Alloc.Address;
        var maxAddress = (nuint*)(Alloc.Address + Alloc.Length);
        nuint result = 0;

        while (ptr < maxAddress)
        {
            memory.Read<nuint>((nuint)ptr, out var res);
            result += res;
            ptr += 1;
        }

        return (nuint)(result + ptr);
    }

    [Benchmark]
    public nuint WriteViaPointer()
    {
        var ptr = (nuint*)Alloc.Address;
        var maxAddress = (nuint*)(Alloc.Address + Alloc.Length);
        nuint result = 0;

        while (ptr < maxAddress)
        {
            *ptr = (nuint)ptr;
            ptr += 1;
        }

        return (nuint)(result + ptr);
    }

    [Benchmark]
    public nuint WriteViaMemory()
    {
        var memory = new Reloaded.Memory.Memory();
        var ptr = (nuint*)Alloc.Address;
        var maxAddress = (nuint*)(Alloc.Address + Alloc.Length);
        nuint result = 0;

        while (ptr < maxAddress)
        {
            memory.Write((nuint)ptr, (nuint)ptr);
            ptr += 1;
        }

        return (nuint)(result + ptr);
    }
}
