using System;

namespace Reloaded.Memory.Shared.Structs
{
    public struct RandomIntStruct : IEndianReversible
    {
        public static Random Random = new Random();

        public byte A;
        public short B;
        public int C;

        public static RandomIntStruct BuildRandomStruct()
        {
            RandomIntStruct randomIntStruct;
            randomIntStruct.A = (byte)Random.Next(byte.MinValue, byte.MaxValue);
            randomIntStruct.B = (short)Random.Next(short.MinValue, short.MaxValue);
            randomIntStruct.C = (int)Random.Next(int.MinValue, int.MaxValue);
            return randomIntStruct;
        }

        /* Custom Equals and GetHashCode */

        public bool Equals(RandomIntStruct other)
        {
            return A == other.A
                   && B == other.B
                   && C == other.C;
        }

        public override bool Equals(object obj)
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

        public void SwapEndian()
        {
            Endian.Reverse(ref B);
            Endian.Reverse(ref C);
        }
    }
}
