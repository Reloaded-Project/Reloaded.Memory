using System;

namespace Reloaded.Memory.Tests.Utilities.Structures;

public struct RandomIntStruct
{
    private static readonly Random Random = new();

    private byte A;
    private short B;
    private int C;

    public static RandomIntStruct BuildRandomStruct()
    {
        RandomIntStruct randomIntStruct;
        randomIntStruct.A = (byte)Random.Next(byte.MinValue, byte.MaxValue);
        randomIntStruct.B = (short)Random.Next(short.MinValue, short.MaxValue);
        randomIntStruct.C = Random.Next(int.MinValue, int.MaxValue);
        return randomIntStruct;
    }

    /* Custom Equals and GetHashCode */
    private bool Equals(RandomIntStruct other) => A == other.A && B == other.B && C == other.C;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is RandomIntStruct other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = A.GetHashCode();
            hashCode = (hashCode * 397) ^ B.GetHashCode();
            hashCode = (hashCode * 397) ^ C;
            return hashCode;
        }
    }
}
