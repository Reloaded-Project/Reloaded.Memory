using System.IO;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Streams;
using Reloaded.Memory.Tests.Memory.Helpers;

namespace Reloaded.Memory.Benchmark.Memory.Streams.Integers
{
    [CoreJob]
    public class UnrealisticMarshallingOverhead
    {
        private RandomInt32Generator _generator;

        [Params(10)]
        public int TotalDataMB { get; set; }

        [Params(4096)]
        public int BufferSize { get; set; }

        /* Constructor/Destructor */
        [GlobalSetup]
        public void Setup()
        {
            _generator = new RandomInt32Generator(TotalDataMB);
        }

        /* Create */
        public void BufferedStreamReaderGetStruct(BufferedStreamReader bufferedStreamReader, out int randomInt)
        {
            bufferedStreamReader.Read(out randomInt);
        }

        public void BufferedStreamReaderGetStructManaged(BufferedStreamReader bufferedStreamReader, out int randomInt)
        {
            bufferedStreamReader.Read(out randomInt, true);
        }

        /* Bench for Waitmarks */
        [Benchmark]
        public int ReloadedStreamReader()
        {
            using (var fileStream = _generator.GetMemoryStream())
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

        [Benchmark]
        public int ReloadedStreamReaderMarshallingOn()
        {
            using (var fileStream = _generator.GetMemoryStream())
            {
                var reloadedReader = new BufferedStreamReader(fileStream, BufferSize);
                int randomInt = 0;

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BufferedStreamReaderGetStructManaged(reloadedReader, out randomInt);
                }

                return randomInt;
            }
        }
    }
}
