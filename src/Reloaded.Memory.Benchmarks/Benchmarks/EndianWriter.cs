using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Extensions;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("Endian Writer", "Tests performance of 'in' keyword on Endian Writers.", Categories.Performance)]
public unsafe class EndianWriter
{
    private const int NumItems = 1000;
    private Vector3[] _regularStructs = new Vector3[NumItems];
    private Vector3ReadOnly[] _readOnlyStructs = new Vector3ReadOnly[NumItems];
    private Vector3Property[] _propertyStructs = new Vector3Property[NumItems];
    private byte* _output = (byte*)NativeMemory.Alloc((nuint)(sizeof(Vector3) * NumItems));

    [GlobalSetup]
    public void Setup()
    {
        for (int x = 0; x < NumItems; x++)
        {
            _regularStructs[x] = new Vector3() { X = x, Y = x, Z = x };
            _readOnlyStructs[x] = new Vector3ReadOnly(x, x, x);
            _propertyStructs[x] = new Vector3Property() { X = x, Y = x, Z = x };
        }
    }

    [GlobalCleanup]
    public void Cleanup() => NativeMemory.Free(_output);

    [Benchmark]
    public void WriteReadOnly()
    {
        var writer = new LittleEndianWriter(_output);
        for (int x = 0; x < NumItems; x++)
            writer.Write(_readOnlyStructs.DangerousGetReferenceAt(x));
    }

    [Benchmark]
    public void WriteProperty()
    {
        var writer = new LittleEndianWriter(_output);
        for (int x = 0; x < NumItems; x++)
            writer.Write(_propertyStructs.DangerousGetReferenceAt(x));
    }

    [Benchmark]
    public void Write()
    {
        var writer = new LittleEndianWriter(_output);
        for (int x = 0; x < NumItems; x++)
            writer.Write(_regularStructs.DangerousGetReferenceAt(x));
    }

    private struct Vector3 : ICanWriteToAnEndianWriter
    {
        public float X;
        public float Y;
        public float Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TEndianWriter>(ref TEndianWriter writer) where TEndianWriter : IEndianWriter
        {
            writer.WriteAtOffset(X, 0);
            writer.WriteAtOffset(Y, 4);
            writer.WriteAtOffset(Z, 8);
            writer.Seek(12);
        }
    }

    private readonly struct Vector3ReadOnly : ICanWriteToAnEndianWriter
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3ReadOnly(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TEndianWriter>(ref TEndianWriter writer) where TEndianWriter : IEndianWriter
        {
            writer.WriteAtOffset(X, 0);
            writer.WriteAtOffset(Y, 4);
            writer.WriteAtOffset(Z, 8);
            writer.Seek(12);
        }
    }

    private struct Vector3Property : ICanWriteToAnEndianWriter
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write<TEndianWriter>(ref TEndianWriter writer) where TEndianWriter : IEndianWriter
        {
            writer.WriteAtOffset(X, 0);
            writer.WriteAtOffset(Y, 4);
            writer.WriteAtOffset(Z, 8);
            writer.Seek(12);
        }
    }
}
