using System;
using System.IO;

namespace Reloaded.Memory.Shared.Generator
{
    public class RandomIntegerGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Integers.bin";
        private static Random _random = new Random();

        /* Target size of buffers for testing. */
        public int[] Structs { get; set; }
        public byte[] Bytes { get; set; }

        /* Construction/Destruction */
        public RandomIntegerGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<int>(totalBytes);
            Structs = new int[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = _random.Next();

            Bytes = StructArray.GetBytes(Structs);
            File.WriteAllBytes(TestFileName, Bytes);
        }

        public System.IO.FileStream GetFileStream()
        {
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }
    }
}
