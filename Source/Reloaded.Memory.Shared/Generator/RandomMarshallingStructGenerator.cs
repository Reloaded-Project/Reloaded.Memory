using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Reloaded.Memory.Shared.Structs;

namespace Reloaded.Memory.Shared.Generator
{
    public class RandomMarshallingStructGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "MarshallingStructs.bin";

        /* Target size of buffers for testing. */
        public MarshallingStruct[] Structs { get; set; }
        public byte[] Bytes { get; set; }

        /* Construction/Destruction */
        public RandomMarshallingStructGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<RandomIntStruct>(totalBytes);
            Structs = new MarshallingStruct[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = MarshallingStruct.BuildRandomStruct();

            Bytes = StructArray.GetBytes(Structs, true);
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
