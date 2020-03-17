using System.IO;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Memory.Helpers;

namespace Reloaded.Memory.Benchmark.Memory.Streams.Integers
{
    [CoreJob]
    public class FileStream
    {
        private RandomInt32Generator _generator;

        [Params(100)]
        public int TotalDataMB { get; set; }

        [Params(1024, 4096, 16384, 65536)]
        public int BufferSize { get; set; }

        /* Constructor/Destructor */
        [GlobalSetup]
        public void Setup()
        {
            _generator = new RandomInt32Generator(TotalDataMB);
        }

        /* Create */
        public void BinaryReaderGetStruct(BinaryReader binaryReader, out int randomInt)
        {
            randomInt = binaryReader.ReadInt32();
        }

        public void BufferedStreamReaderGetStruct(BufferedStreamReader bufferedStreamReader, out int randomInt)
        {
            bufferedStreamReader.Read(out randomInt);
        }

        /* Bench for Waitmarks */
        [Benchmark]
        public int BinaryReader()
        {
            using (var fileStream = _generator.GetFileStream())
            {
                var binaryReader = new BinaryReader(fileStream);
                int randomInt = 0;

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BinaryReaderGetStruct(binaryReader, out randomInt);
                }

                return randomInt;
            }
        }

        [Benchmark]
        public int ReloadedStreamReader()
        {
            using (var fileStream = _generator.GetFileStream())
            {
                var reloadedReader = new BufferedStreamReader(fileStream, BufferSize);
                int randomInt = 0;

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BufferedStreamReaderGetStruct(reloadedReader, out randomInt);
                }

                return randomInt;
            }
        }
    }
}
