using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Streams;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("Buffered Stream Reader (MemoryStream)", "Tests performance of Buffered Stream Reader on MemoryStream.", Categories.Performance)]
public class BufferedStreamReader
{
    private MemoryStream _memoryStream = null!;
    private BufferedStreamReader<MemoryStream> _bufferedStreamReader = null!;
    private byte[] _buffer = null!;
    private unsafe byte* _nativePtr;
    private GCHandle _handle;
    private BinaryReader _binaryReader = null!;

    [Params(8388608)] // 8 MiB
    public int N { get; set; }

    [GlobalSetup]
    public unsafe void Setup()
    {
        _buffer = new byte[N];
        for (int x = 0; x < _buffer.Length; x++)
            _buffer[x] = (byte)x;

        _handle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

        _memoryStream = new MemoryStream(_buffer);
        _binaryReader = new BinaryReader(_memoryStream);
        _bufferedStreamReader = new BufferedStreamReader<MemoryStream>(_memoryStream);
        _nativePtr = (byte*)_handle.AddrOfPinnedObject();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _bufferedStreamReader.Dispose();
        _handle.Free();
    }

    [Benchmark]
    public byte BufferedStreamReader_ReadByte() => BufferedStreamReader_Read<byte>();

    [Benchmark]
    public byte BinaryReader_ReadByte()
    {
        _memoryStream.Position = 0;
        byte result = 0;
        var n = N;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadByte();

        return result;
    }

    [Benchmark]
    public byte NativePointer_ReadByte() => NativePointer_Read<byte>();

    [Benchmark]
    public byte BufferedStreamReader_ReadByteRaw() => BufferedStreamReader_ReadRaw<byte>();

    [Benchmark]
    public int BufferedStreamReader_ReadInt()
    {
        _memoryStream.Position = 0;
        _bufferedStreamReader.Seek(0, SeekOrigin.Begin);
        int result = 0;
        var n = N / 4;
        for (int x = 0; x < n; x++)
            result = _bufferedStreamReader.Read<int>();

        return result;
    }

    [Benchmark]
    public int BinaryReader_ReadInt()
    {
        _memoryStream.Position = 0;
        int result = 0;
        var n = N / 4;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadInt32();

        return result;
    }

    [Benchmark]
    public int NativePointer_ReadInt() => NativePointer_Read<int>();

    [Benchmark]
    public int BufferedStreamReader_ReadIntRaw() => BufferedStreamReader_ReadRaw<int>();

    [Benchmark]
    public long BinaryReader_ReadLong()
    {
        _memoryStream.Position = 0;
        long result = 0;
        var n = N / 8;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadInt64();

        return result;
    }

    [Benchmark]
    public long BufferedStreamReader_ReadLong() => BufferedStreamReader_Read<long>();

    [Benchmark]
    public long NativePointer_ReadLong() => NativePointer_Read<long>();

    [Benchmark]
    public long BufferedStreamReader_ReadLongRaw() => BufferedStreamReader_ReadRaw<long>();

    private unsafe T BufferedStreamReader_Read<T>() where T : unmanaged
    {
        _memoryStream.Position = 0;
        _bufferedStreamReader.Seek(0, SeekOrigin.Begin);
        T result = default;
        var n = N / sizeof(T);
        for (int x = 0; x < n; x++)
            result = _bufferedStreamReader.Read<T>();

        return result;
    }

    private unsafe T NativePointer_Read<T>() where T : unmanaged
    {
        T result = default;
        var nativePtr = (T*)_nativePtr;
        var n = N / sizeof(T);
        for (int x = 0; x < n; x++)
            result = nativePtr[x];

        return result;
    }

    private unsafe T BufferedStreamReader_ReadRaw<T>() where T : unmanaged
    {
        _memoryStream.Position = 0;
        var n = N / sizeof(T);
        int numRead = 0;
        T result = default;
        while (numRead < n)
        {
            var ptr = _bufferedStreamReader.ReadRaw<T>(n - numRead, out int available);
            for (int x = 0; x < available; x++)
                result = ptr[x];

            numRead += available;
        }

        return result;
    }
}
