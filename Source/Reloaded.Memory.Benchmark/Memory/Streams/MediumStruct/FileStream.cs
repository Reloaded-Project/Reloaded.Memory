using System.IO;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Shared.Generator;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Streams;

namespace Reloaded.Memory.Benchmark.Memory.Streams.MediumStruct
{
    [CoreJob]
    public class FileStream
    {
        private RandomQuadnodeGenerator _generator;

        [Params(100)]
        public int TotalDataMB { get; set; }

        [Params(1024, 4096, 16384, 65536)]
        public int BufferSize { get; set; }

        /* Constructor/Destructor */
        [GlobalSetup]
        public void Setup()
        {
            _generator = new RandomQuadnodeGenerator(TotalDataMB);
        }

        /* Create */
        public void BinaryReaderGetStruct(BinaryReader binaryReader, out Quadnode randomQuadnode)
        {
            randomQuadnode.NodeIndex = binaryReader.ReadUInt16();
            randomQuadnode.NodeParent = binaryReader.ReadUInt16();
            randomQuadnode.NodeChild = binaryReader.ReadUInt16();
            randomQuadnode.RightNodeNeighbour = binaryReader.ReadUInt16();
            randomQuadnode.LeftNodeNeighbour = binaryReader.ReadUInt16();
            randomQuadnode.BottomNodeNeighbour = binaryReader.ReadUInt16();
            randomQuadnode.TopNodeNeighbour = binaryReader.ReadUInt16();
            randomQuadnode.NumberOfTriangles = binaryReader.ReadUInt16();
            randomQuadnode.OffsetTriangleList = binaryReader.ReadUInt32();
            randomQuadnode.PositioningOffsetValueLR = binaryReader.ReadUInt16();
            randomQuadnode.PositioningOffsetValueTB = binaryReader.ReadUInt16();
            randomQuadnode.NodeDepthLevel = binaryReader.ReadByte();
            randomQuadnode.Null1 = binaryReader.ReadByte();
            randomQuadnode.Null2 = binaryReader.ReadUInt16();
            randomQuadnode.Null3 = binaryReader.ReadUInt32();
        }

        public void BufferedStreamReaderGetStruct(BufferedStreamReader bufferedStreamReader, out Quadnode randomQuadnode)
        {
            bufferedStreamReader.Read(out randomQuadnode);
        }

        /* Bench for Waitmarks */
        [Benchmark]
        public Quadnode BinaryReader()
        {
            using (var fileStream = _generator.GetFileStream())
            {
                var binaryReader = new BinaryReader(fileStream);
                Quadnode randomQuadnode = new Quadnode();

                for (int x = 0; x < _generator.Structs.Length; x++)
                {
                    BinaryReaderGetStruct(binaryReader, out randomQuadnode);
                }

                return randomQuadnode;
            }
        }

        [Benchmark]
        public Quadnode ReloadedStreamReader()
        {
            using (var fileStream = _generator.GetFileStream())
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
    }
}
