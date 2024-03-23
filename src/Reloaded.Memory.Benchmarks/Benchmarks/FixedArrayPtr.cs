using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("FixedArrayPtr Read Write", "Tests whether FixedArrayPtr's Read/Write Functionality is Zero-Cost.",
    Categories.ZeroOverhead)]
public unsafe class FixedArrayPtr
{
    private const int ArrayLength = 1000;

    private FixedArrayPtr<int> _fixedArrayPtr;
    private int[] _sourceArray = null!;
    private int[] _destinationArray = null!;
    private MemoryAllocation _allocation;

    [GlobalSetup]
    public void Setup()
    {
        _allocation = new Reloaded.Memory.Memory().Allocate(sizeof(int) * ArrayLength);
        _fixedArrayPtr = new FixedArrayPtr<int>((int*)_allocation.Address, ArrayLength);

        _sourceArray = new int[ArrayLength];
        _destinationArray = new int[ArrayLength];

        var random = new Random();
        for (var x = 0; x < ArrayLength; x++)
            _sourceArray[x] = random.Next();
    }

    [GlobalCleanup]
    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
    public void Cleanup() => new Reloaded.Memory.Memory().Free(_allocation);

    [Benchmark]
    public void FixedArrayPtr_CopyFrom() => _fixedArrayPtr.CopyFrom(_sourceArray, ArrayLength);

    [Benchmark]
    public void FixedArrayPtr_CopyTo() => _fixedArrayPtr.CopyTo(_destinationArray, ArrayLength);

    [Benchmark]
    public int FixedArrayPtr_Get()
    {
        FixedArrayPtr<int> ptr = _fixedArrayPtr;
        var value = 0;
        for (var x = 0; x < ArrayLength; x++)
            value = ptr.Get(x);

        return value;
    }

    [Benchmark]
    public int RawPointer_Get()
    {
        int* ptr = _fixedArrayPtr.Pointer;
        var value = 0;
        for (var x = 0; x < ArrayLength; x++)
            value = ptr[x];

        return value;
    }

    [Benchmark]
    public void FixedArrayPtr_Set()
    {
        FixedArrayPtr<int> ptr = _fixedArrayPtr;
        for (var x = 0; x < ArrayLength; x++)
            ptr.Set(x, _sourceArray[x]);
    }

    [Benchmark]
    public void RawPointer_Set()
    {
        int* ptr = _fixedArrayPtr.Pointer;
        for (var x = 0; x < ArrayLength; x++)
            ptr[x] = _sourceArray[x];
    }
}
