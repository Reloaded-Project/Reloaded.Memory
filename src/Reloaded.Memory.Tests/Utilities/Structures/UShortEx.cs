using System.Buffers.Binary;
using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Tests.Utilities.Structures;

public struct UShortEx : ICanReverseEndian
{
    public ushort Short;
    public void ReverseEndian() => Short = BinaryPrimitives.ReverseEndianness(Short);
    public static implicit operator ushort(UShortEx value) => value.Short;
    public static implicit operator UShortEx(ushort value) => new() { Short = value };
}
