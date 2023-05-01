﻿using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Memory.Enums;
using Reloaded.Memory.Memory.Interfaces;
using Reloaded.Memory.Memory.Structs;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[BenchmarkInfo("Memory Protection Extensions", "Checks if memory protection change extensions are minimal cost.", Categories.ZeroOverhead)]
public unsafe class MemoryChangeProtectionExtension
{
    // Must be divisible by 2
    public const int DataSize = 4096;

    public MemoryAllocation Alloc { get; set; }

    [GlobalSetup]
    public void Setup() => Alloc = Reloaded.Memory.Memory.Memory.Instance.Allocate(DataSize);

    [GlobalCleanup]
    public void Cleanup() => Reloaded.Memory.Memory.Memory.Instance.Free(Alloc);

    // Note: We're not unrolling because we don't care for it to run as fast as possible, only that it's zero overhead.

    [Benchmark]
    public nuint ChangePermission_Direct()
    {
        var memory = new Reloaded.Memory.Memory.Memory();
        var oldProtection = memory.ChangeProtection(Alloc.Address, (int)Alloc.Length, MemoryProtection.READ);
        memory.ChangeProtectionRaw(Alloc.Address, (int)Alloc.Length, oldProtection);
        return oldProtection;
    }

    [Benchmark]
    public nuint ChangePermission_Disposable()
    {
        var memory = new Reloaded.Memory.Memory.Memory();
        var oldProtection = memory.ChangeProtectionDisposable(Alloc.Address, (int)Alloc.Length, MemoryProtection.READ);
        oldProtection.Dispose();
        return oldProtection.OriginalProtection;
    }

    [Benchmark]
    public nuint ChangePermission_Disposable_Using()
    {
        var memory = new Reloaded.Memory.Memory.Memory();
        using var oldProtection = memory.ChangeProtectionDisposable(Alloc.Address, (int)Alloc.Length, MemoryProtection.READ);
        return oldProtection.OriginalProtection;
    }
}