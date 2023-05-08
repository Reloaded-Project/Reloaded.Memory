using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("Ptr Read Write", "Tests whether Ptr's Read/Write Functionality is Zero-Cost.", Categories.ZeroOverhead)]
public class CircularBuffer
{
    /*
    private Utility.CircularBuffer _circularBuffer;
    private byte[] _data;

    [GlobalSetup]
    public void Setup()
    {
        int bufferSize = 1024 * 1024; // 1 MB
        byte[] buffer = new byte[bufferSize];
        _circularBuffer = new Utility.CircularBuffer((nuint)Unsafe.AsPointer(ref buffer[0]), bufferSize);

        _data = new byte[256];
        new Random().NextBytes(_data);
    }

    [Benchmark]
    public nuint AddBytes() => _circularBuffer.Add((byte*)Unsafe.AsPointer(ref _data[0]), (uint)_data.Length);

    [Benchmark]
    public nuint AddInt() => _circularBuffer.Add<int>(42);

    [Benchmark]
    public nuint AddLong() => _circularBuffer.Add<long>(42L);

    [Benchmark]
    public nuint AddVector128() => _circularBuffer.Add<System.Numerics.Vector128<float>>(System.Numerics.Vector128<float>.Zero);

    [Benchmark]
    public nuint AddVector256() => _circularBuffer.Add<System.Numerics.Vector256<float>>(System.Numerics.Vector256<float>.Zero);

    [Benchmark]
    public CircularBuffer.ItemFit CanItemFitBytes() => _circularBuffer.CanItemFit((uint)_data.Length);

    [Benchmark]
    public CircularBuffer.ItemFit CanItemFitInt() => _circularBuffer.CanItemFit<int>();
    */
}
