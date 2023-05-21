# Memory API

!!! info

    `Reloaded.Memory` provides various abstractions that can be used to wrap around contiguous regions of memory.  

!!! success "All APIs listed here are zero overhead."

!!! note "Stuff listed here isn't that impressive, but it's the basic building block for what follows next."

## Memory & ExternalMemory

!!! info 
    
    The `Memory` and `ExternalMemory` classes are the most basic abstractions provided by `Reloaded.Memory`.  
    They allow you to access memory either within the current or a `target process`.

=== "Raw Pointer"

    ```csharp
    while (ptr < maxAddress)
    {
        result += *ptr;
        ptr += 1;
    }
    ```

=== "Another Process"

    ```csharp
    if (Polyfills.IsWindows())
        return Kernel32.ReadProcessMemory(_processHandle, location, (nuint)buffer, numBytes, out _);

    if (Polyfills.IsLinux())
        return Posix.process_vm_readv_k32(_processHandle, location, (nuint)buffer, numBytes);

    // And other cases!
    ```

=== "w/ `Memory`"

    ```csharp
    // memory = Memory.Instance;
    while (ptr < maxAddress)
    {
        result += memory.Read<nuint>((UIntPtr)ptr);
        ptr += 1;
    }
    ```

=== "w/ `ExternalMemory`"

    ```csharp
    // memory = new ExternalMemory(process);
    while (ptr < maxAddress)
    {
        result += memory.Read<nuint>((UIntPtr)ptr);
        ptr += 1;
    }
    ```

As you can see, with the library and its `ICanReadWriteMemory` interface; usage is unified across all sources. Instead
of having to write different code for different sources (first 2 examples), you can now write the same code for all sources.  

And of course, various different utility methods are provided to make your life easier.  

=== "Raw Pointer Marshalling"

    ```csharp
    while (ptr < maxAddress)
        Marshal.StructureToPtr(items[x++], (nint)offset, false);
    ```

=== "Another Process"

    ```csharp
    byte* bufferPtr = new byte[structSize];
    bool succeeded = ReadProcessMemory(offset, bufferPtr, (nuint)structSize);
    if (!succeeded)
        ThrowHelpers.ThrowReadExternalMemoryExceptionWindows(offset, structSize);

    Marshal.PtrToStructure((nint)bufferPtr, value);
    ```

=== "w/ `Memory`"

    ```csharp
    while (ptr < maxAddress)
        memory.WriteWithMarshalling(ptr, items[x++]);
    ```

=== "w/ `ExternalMemory`"

    ```csharp
    while (ptr < maxAddress)
        memory.WriteWithMarshalling(ptr, items[x++]);
    ```

All silly boilerplate needed to manipulate different sources is gone; and this is all done with zero-overhead.

## Unified Memory Allocation API

!!! info

    Structs like `Memory` and `ExternalMemory` employ `ICanAllocateMemory` API to make memory allocations convenient.  

=== "Regular Code (.NET 7+)"

    ```csharp
    var allocation = NativeMemory.Alloc(100);
    ```

=== "Classic Code (<= .NET 7)"

    ```csharp
    var allocation = Marshal.AllocHGlobal(100);
    ```

=== "w/ `Memory`"

    ```csharp
    var allocation = memory.Allocate(100);
    ```

=== "w/ `ExternalMemory`"

    ```csharp
    var allocation = memory.Allocate(100);
    ```

Now you can allocate in another process in a consistent manner. Useful?

!!! warning "`ICanAllocateMemory` for `ExternalMemory` currently implemented in Windows only; PRs for Linux and OSX."

## Unified Permission Change API

!!! info

    Structs like `Memory` and `ExternalMemory` employ `ICanChangeMemoryProtection` API to allow you to change memory permissions.  

    This allows you to make existing code etc. in memory writable for editing.  

=== "This Process"

    ```csharp
    if (Polyfills.IsWindows())
    {
        bool result = Kernel32.VirtualProtect(memoryAddress, (nuint)size, (Kernel32.MEM_PROTECTION)newProtection,
            out Kernel32.MEM_PROTECTION oldPermissions);
        if (!result)
            ThrowHelpers.ThrowMemoryPermissionExceptionWindows(memoryAddress, size, newProtection);

        return (nuint)oldPermissions;
    }

    if (Polyfills.IsLinux() || Polyfills.IsMacOS())
    {
        // ... lot more boilerplate
    }
    ```

=== "w/ `Memory`"

    ```csharp
    var oldPermissions = source.ChangeProtection(address, length, MemoryProtection.READ);
    ```

=== "w/ `ExternalMemory`"

    ```csharp
    var oldPermissions = source.ChangeProtection(address, length, MemoryProtection.READ);
    ```

Pretty useful huh?

!!! warning "`ICanChangeMemoryProtection` for `ExternalMemory` currently implemented in Windows only; PRs for Linux and OSX are welcome."

## Extensions

!!! info

    These interfaces, and combinations of them allow for some very useful utility methods to be made.

Temporary allocate a buffer:  

```csharp
// Automatically disposed, even on exception
using var alloc = memory.AllocateDisposable(DataSize);
```

Temporary change memory permission:  

```csharp
using var alloc = memory.ChangeProtectionDisposable(DataSize);
```

Temporary change memory permission, write data, and restore:  

```csharp
memory.SafeWrite(Alloc.Address, Data.AsSpanFast());
```

## Reference Benchmarks

!!! info "In most cases, the abstractions generate 1:1 code that matches exactly the same performance as working with raw pointers."

```
|                        Method |     Mean |   Error |  StdDev | Code Size | Allocated |
|------------------------------ |---------:|--------:|--------:|----------:|----------:|
|                ReadViaPointer | 130.7 ns | 0.77 ns | 0.69 ns |      49 B |         - |
|                 ReadViaMemory | 132.4 ns | 0.71 ns | 0.66 ns |      52 B |         - |
| ReadViaMemory_ViaOutParameter | 132.2 ns | 1.81 ns | 1.70 ns |      52 B |         - |
|               WriteViaPointer | 117.4 ns | 1.55 ns | 1.45 ns |      48 B |         - |
|                WriteViaMemory | 117.8 ns | 1.67 ns | 1.57 ns |      48 B |         - |
```

`ReadViaPointer` and `WriteViaPointer` are using raw pointers; remaining tests are using the abstractions.