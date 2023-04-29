using System.IO;
using Reloaded.Memory.Shared.Structs;

namespace Reloaded.Memory.Shared.Generator
{
    public class RandomIntStructGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "RandomIntStruct.bin";

        /* Target size of buffers for testing. */
        public RandomIntStruct[] Structs { get; set; }
        public byte[] Bytes { get; set; }

        /* Construction/Destruction */
        public RandomIntStructGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<RandomIntStruct>(totalBytes);
            Structs = new RandomIntStruct[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = RandomIntStruct.BuildRandomStruct();

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
