﻿using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Enums;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Utility;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Memory Safe Read/Write Extensions", "Checks if memory safe read/write are minimal cost.", Categories.ZeroOverhead)]
public class MemorySafeReadWriteExtensions
{
    // Must be divisible by 2
    public const int DataSize = 4096;

    public MemoryAllocation Alloc { get; set; }

    public byte[] Data { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Alloc = new Reloaded.Memory.Memory().Allocate(DataSize);
        Data = new byte[DataSize];
    }

    [GlobalCleanup]
    public void Cleanup() => new Reloaded.Memory.Memory().Free(Alloc);

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public void SafeReadWrite_Direct()
    {
        var memory = new Reloaded.Memory.Memory();
        using var disposable = memory.ChangeProtectionDisposable(Alloc.Address, (int)Alloc.Length, MemoryProtection.READ_WRITE_EXECUTE);
        memory.WriteRaw(Alloc.Address, Data.AsSpanFast());
    }

    [Benchmark]
    public void SafeReadWrite_Extension()
    {
        // Inlining (correctly) blocked by try/finally. Otherwise zero-overhead.
        var memory = new Reloaded.Memory.Memory();
        memory.SafeWrite(Alloc.Address, Data.AsSpanFast());
    }
}
