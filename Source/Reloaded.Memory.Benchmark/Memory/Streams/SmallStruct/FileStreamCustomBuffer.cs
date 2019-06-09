using System.IO;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Memory.Helpers;

namespace Reloaded.Memory.Benchmark.Memory.Streams.SmallStruct
{
    [CoreJob]
    public class FileStreamCustomBuffer
    {
        private RandomIntStructGenerator _generator;

        [Params(100)]
        public int TotalDataMB { get; set; }

        [Params(1024, 4096, 16384, 65536)]
        public int BufferSize { get; set; }

        /* Constructor/Destructor */
        [GlobalSetup]
        public void Setup()
        {
            _generator = new RandomIntStructGenerator(TotalDataMB);
        }

        /* Create */
        public void BinaryReaderGetStruct(BinaryReader binaryReader, out RandomIntStruct randomIntStruct)
        {
            randomIntStruct.A = binaryReader.ReadByte();
            randomIntStruct.B = binaryReader.ReadInt16();
            randomIntStruct.C = binaryReader.ReadInt32();
        }

        public void BufferedStreamReaderGetStruct(BufferedStreamReader bufferedStreamReader, out RandomIntStruct randomIntStruct)
        {
            bufferedStreamReader.Read(out randomIntStruct);
        }

        /* Bench for Waitmarks */
        [Benchmark]
        public RandomIntStruct BinaryReaderCustomBufferSize()
        {
            using (var fileStream = _generator.GetFileStreamWithBufferSize(BufferSize))
            {
                var binaryReader = new BinaryReader(fileStream);
                RandomIntStruct randomIntStruct = new RandomIntStruct();

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BinaryReaderGetStruct(binaryReader, out randomIntStruct);
                }

                return randomIntStruct;
            }
        }

        [Benchmark]
        public RandomIntStruct ReloadedStreamReaderCustomBufferSize()
        {
            using (var fileStream = _generator.GetFileStreamWithBufferSize(BufferSize))
            {
                var reloadedReader = new BufferedStreamReader(fileStream, BufferSize);
                RandomIntStruct randomIntStruct = new RandomIntStruct();

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BufferedStreamReaderGetStruct(reloadedReader, out randomIntStruct);
                }

                return randomIntStruct;
            }
        }
    }
}
