using System;

namespace Reloaded.Memory.Tests.Utilities.Structures;

public struct RandomByteArray
{
    public byte[] Array { get; set; }

    public static RandomByteArray GenerateRandomByteArray(int size)
    {
        RandomByteArray byteArray = new();
        Random randomGenerator = new();

        byteArray.Array = new byte[size];
        randomGenerator.NextBytes(byteArray.Array);

        return byteArray;
    }
}
