using Reloaded.Memory.Interfaces;

namespace Reloaded.Memory.Structs;

/// <summary>
///     Represents an individual memory allocation made via <see cref="ICanAllocateMemory" />.
/// </summary>
public struct MemoryAllocation
{
    /// <summary>
    ///     Address of the allocated memory.
    /// </summary>
    public nuint Address;

    /// <summary>
    ///     Length of the allocated memory.
    /// </summary>
    public nuint Length;

    /// <summary>
    ///     Creates a new memory allocation.
    /// </summary>
    /// <param name="address">Address.</param>
    /// <param name="length">Length.</param>
    public MemoryAllocation(UIntPtr address, UIntPtr length)
    {
        Address = address;
        Length = length;
    }
}
