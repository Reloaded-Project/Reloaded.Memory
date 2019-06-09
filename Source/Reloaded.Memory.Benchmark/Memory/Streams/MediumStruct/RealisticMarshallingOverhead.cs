using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Benchmark.Memory.Streams.MediumStruct
{
    [CoreJob]
    public class RealisticMarshallingOverhead
    {
        private RandomQuadnodeGenerator _generator;

        [Params(10)]
        public int TotalDataMB { get; set; }

        [Params(4096)]
        public int BufferSize { get; set; }

        /* Constructor/Destructor */
        [GlobalSetup]
        public void Setup()
        {
            _generator = new RandomQuadnodeGenerator(TotalDataMB);
        }

        /* Create */
        public void BufferedStreamReaderGetStruct(BufferedStreamReader bufferedStreamReader, out Quadnode randomInt)
        {
            bufferedStreamReader.Read(out randomInt);
        }

        public void BufferedStreamReaderGetStructManaged(BufferedStreamReader bufferedStreamReader, out Quadnode randomInt)
        {
            bufferedStreamReader.Read(out randomInt, true);
        }

        /* Bench for Waitmarks */
        [Benchmark]
        public Quadnode ReloadedStreamReader()
        {
            using (var fileStream = _generator.GetMemoryStream())
            {
                var reloadedReader = new BufferedStreamReader(fileStream, BufferSize);
                Quadnode randomQuadnode = new Quadnode();

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BufferedStreamReaderGetStruct(reloadedReader, out randomQuadnode);
                }

                return randomQuadnode;
            }
        }

        [Benchmark]
        public Quadnode ReloadedStreamReaderMarshallingOn()
        {
            using (var fileStream = _generator.GetMemoryStream())
            {
                var reloadedReader = new BufferedStreamReader(fileStream, BufferSize);
                Quadnode randomQuadnode = new Quadnode();

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BufferedStreamReaderGetStructManaged(reloadedReader, out randomQuadnode);
                }

                return randomQuadnode;
            }
        }
    }
}
