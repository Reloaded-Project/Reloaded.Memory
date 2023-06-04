using System;
using System.Runtime.InteropServices;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

namespace Reloaded.Memory.Tests.Utilities.Structures;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct RandomIntStruct : ICanWriteToAnEndianWriter, ICanBeReadByAnEndianReader, ICanReverseEndian
{
    private static readonly Random Random = new();

    private byte A;
    private short B;
    private int C;

    public RandomIntStruct()
    {
        A = (byte)Random.Next(byte.MinValue, byte.MaxValue);
        B = (short)Random.Next(short.MinValue, short.MaxValue);
        C = Random.Next(int.MinValue, int.MaxValue);
    }

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

    public void ReverseEndian()
    {
        B = Endian.Reverse(B);
        C = Endian.Reverse(C);
    }

    public unsafe void Read<TEndianReader>(ref TEndianReader reader) where TEndianReader : IEndianReader
    {
        A = reader.ReadByteAtOffset(0);
        B = reader.ReadShortAtOffset(1);
        C = reader.ReadIntAtOffset(3);
        reader.Seek(sizeof(RandomIntStruct));
    }

    public unsafe void Write<TEndianWriter>(ref TEndianWriter writer) where TEndianWriter : IEndianWriter
    {
        writer.WriteAtOffset(A, 0);
        writer.WriteAtOffset(B, 1);
        writer.WriteAtOffset(C, 3);
        writer.Seek(sizeof(RandomIntStruct));
    }
}
