using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmarks.Framework;
using Reloaded.Memory.Streams;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Reloaded.Memory.Benchmarks.Benchmarks;

[DisassemblyDiagnoser]
[BenchmarkInfo("Buffered Stream Reader (FileStream)", "Tests performance of Buffered Stream Reader on FileStream.", Categories.Performance)]
public class BufferedStreamReader_FileStream
{
    private FileStream _fileStream = null!;
    private BufferedStreamReader<FileStream> _bufferedStreamReader = null!;
    private BinaryReader _binaryReader = null!;
    private string _filePath = null!;

    [Params(8388608)] // 8 MiB
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _filePath = Path.GetTempFileName();
        _fileStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        for (int x = 0; x < N; x++)
            _fileStream.WriteByte((byte)x);

        _binaryReader = new BinaryReader(_fileStream);
        _bufferedStreamReader = new BufferedStreamReader<FileStream>(_fileStream);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fileStream.Dispose();
        _bufferedStreamReader.Dispose();
        File.Delete(_filePath);
    }

    [Benchmark]
    public byte BufferedStreamReader_ReadByte() => BufferedStreamReader_Read<byte>();

    [Benchmark]
    public byte BinaryReader_ReadByte()
    {
        _fileStream.Position = 0;
        byte result = 0;
        var n = N;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadByte();

        return result;
    }

    [Benchmark]
    public byte BufferedStreamReader_ReadByteRaw() => BufferedStreamReader_ReadRaw<byte>();

    [Benchmark]
    public int BufferedStreamReader_ReadInt()
    {
        _fileStream.Position = 0;
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
        _fileStream.Position = 0;
        int result = 0;
        var n = N / 4;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadInt32();

        return result;
    }

    [Benchmark]
    public int BufferedStreamReader_ReadIntRaw() => BufferedStreamReader_ReadRaw<int>();

    [Benchmark]
    public long BinaryReader_ReadLong()
    {
        _fileStream.Position = 0;
        long result = 0;
        var n = N / 8;
        for (int x = 0; x < n; x++)
            result = _binaryReader.ReadInt64();

        return result;
    }

    [Benchmark]
    public long BufferedStreamReader_ReadLong() => BufferedStreamReader_Read<long>();

    [Benchmark]
    public long BufferedStreamReader_ReadLongRaw() => BufferedStreamReader_ReadRaw<long>();

    private unsafe T BufferedStreamReader_Read<T>() where T : unmanaged
    {
        _fileStream.Position = 0;
        _bufferedStreamReader.Seek(0, SeekOrigin.Begin);
        T result = default;
        var n = N / sizeof(T);
        for (int x = 0; x < n; x++)
            result = _bufferedStreamReader.Read<T>();

        return result;
    }

    private unsafe T BufferedStreamReader_ReadRaw<T>() where T : unmanaged
    {
        _fileStream.Position = 0;
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
