using System;

namespace Reloaded.Memory.Shared.Structs
{
    public struct RandomByteArray
    {
        public byte[] Array { get; set; }

        private RandomByteArray(byte[] array)
        {
            Array = array;
        }

        public static RandomByteArray GenerateRandomByteArray(int size)
        {
            RandomByteArray byteArray = new RandomByteArray();
            Random randomGenerator    = new Random();

            byteArray.Array = new byte[size];
            randomGenerator.NextBytes(byteArray.Array);

            return byteArray;
        }
    }
}
