using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("Ptr Read Write", "Tests whether Ptr's Read/Write Functionality is Zero-Cost.", Categories.ZeroOverhead)]
public unsafe class Ptr
{
    private const int NumArrayItems = 1024;

    private MemoryAllocation _allocation;
    private int* _rawPtr;
    private Ptr<int> _ptr;

    [GlobalSetup]
    public void Setup()
    {
        _allocation = Reloaded.Memory.Memory.Instance.Allocate(NumArrayItems * 4);
        _rawPtr = (int*)_allocation.Address;
        _ptr = _rawPtr;
        for (var x = 0; x < NumArrayItems; x++)
            _rawPtr[x] = x;
    }

    [Benchmark]
    public int PtrGet()
    {
        var sum = 0;
        for (var x = 0; x < NumArrayItems; x++)
            sum += _ptr.Get(x);

        return sum;
    }

    [Benchmark]
    public int RawPtrGet()
    {
        var sum = 0;
        for (var x = 0; x < NumArrayItems; x++)
            sum += _rawPtr[x];

        return sum;
    }

    [Benchmark]
    public void PtrSet()
    {
        for (var x = 0; x < NumArrayItems; x++)
            _ptr.Set(x, x);
    }

    [Benchmark]
    public void RawPtrSet()
    {
        for (var x = 0; x < NumArrayItems; x++)
            // ReSharper disable once ConvertToCompoundAssignment
            _rawPtr[x] = x;
    }

    [GlobalCleanup]
    public void Cleanup() => Reloaded.Memory.Memory.Instance.Free(_allocation);
}
