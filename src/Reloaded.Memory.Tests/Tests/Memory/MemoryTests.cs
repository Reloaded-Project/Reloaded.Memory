using System.Runtime.InteropServices;
using Reloaded.Memory.Structs;
using Reloaded.Memory.Tests.Utilities;
using Reloaded.Memory.Tests.Utilities.Structures;
using Xunit;
using static Reloaded.Memory.Tests.Utilities.MemorySources;

namespace Reloaded.Memory.Tests.Tests.Memory;

public class MemoryTests
{
    /// <summary>
    ///     Tests writing and reading of raw data (byte arrays) to/from allocated memory.
    /// </summary>
    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ReadAndWrite_RawData(MemorySourceKind kind)
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;

        MemoryAllocation allocation = source.AllocateMemory.Allocate(allocLength);

        for (var x = 0; x < 100; x++)
        {
            var randomArray = RandomByteArray.GenerateRandomByteArray(allocLength);
            source.ReadWriteMemory.WriteRaw(allocation.Address, randomArray.Array);
            var randomValueCopy = source.ReadWriteMemory.ReadRaw(allocation.Address, randomArray.Array.Length);
            Assert.Equal(randomArray.Array, randomValueCopy);
        }

        source.AllocateMemory.Free(allocation);
    }

    /// <summary>
    ///     Attempts to write structs to a specific allocated memory address, then attempts to read the written value.
    /// </summary>
    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ReadAndWrite_BlittableStructs(MemorySourceKind kind)
    {
        // Prepare
        using ITemporaryMemorySource source = GetMemorySource(kind);
        const int allocLength = 0x100;
        MemoryAllocation allocation = source.AllocateMemory.Allocate(allocLength);

        for (var x = 0; x < 100; x++)
        {
            var randomIntStruct = RandomIntStruct.BuildRandomStruct();
            source.ReadWriteMemory.Write(allocation.Address, randomIntStruct);
            source.ReadWriteMemory.Read(allocation.Address, out RandomIntStruct randomValueCopy);
            Assert.Equal(randomIntStruct, randomValueCopy);
        }

        source.AllocateMemory.Free(allocation);
    }

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ReadAndWrite_WithMarshalling(MemorySourceKind kind)
        => ReadAndWrite_WithMarshalling_Common<MarshallingStruct>(kind);

    [Theory]
    [ClassData(typeof(MemorySourceKindNoExternalOnNonWindows))]
    public void ReadAndWrite_WithMarshalling_Huge(MemorySourceKind kind)
        => ReadAndWrite_WithMarshalling_Common<HugeMarshallingStruct>(kind);

    private static void ReadAndWrite_WithMarshalling_Common<T>(MemorySourceKind kind) where T : MarshallingStruct, new()
    {
        using ITemporaryMemorySource source = GetMemorySource(kind);
        var minSize = Marshal.SizeOf<T>();
        var allocLength = minSize;
        MemoryAllocation allocation = source.AllocateMemory.Allocate((nuint)allocLength);

        for (var x = 0; x < 100; x++)
        {
            var randomStruct = new T();
            source.ReadWriteMemory.WriteWithMarshalling(allocation.Address, randomStruct);
            source.ReadWriteMemory.ReadWithMarshallingOutParameter(allocation.Address, out T? randomValueCopy);

            Assert.Equal(randomStruct, randomValueCopy);

            // Test references:
            // If marshalling did not take place, write function would have written pointer to string and read it back in.
            // If marshalling did take place, a new string was created with the value of the string found in memory.
            Assert.False(ReferenceEquals(randomStruct.Name, randomValueCopy!.Name));
        }

        source.AllocateMemory.Free(allocation);
    }
}
