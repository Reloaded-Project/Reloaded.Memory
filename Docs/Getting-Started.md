<div align="center">
	<h1>Reloaded.Memory: Getting Started</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong><i>By my arts, become hard as steel</i></strong>
</div>

## Page Information

🕒  Reading Time: 05-10 Minutes

## Introduction

The following is a small, quick, non-exhaustive resource to help you get started with the *Reloaded.Memory* library - providing an introduction to writing code using the library. This serves as a guide to help you get going, covering the basics and essentials.

Fun fact: There is a project called `Reloaded.Memory.Example` in the solution, which is a mini mirror of this guide.

## Table of Contents
- [Page Information](#page-information)
- [Introduction](#introduction)
- [Table of Contents](#table-of-contents)
- [Prologue](#prologue)
- [Class Breakdown](#class-breakdown)
- [Source Code Samples](#source-code-samples)
  - [Sample: Reading/Writing Primitives](#sample-readingwriting-primitives)
  - [Sample: Reading/Writing Structs](#sample-readingwriting-structs)
  - [Sample: Memory Sources](#sample-memory-sources)
  - [Sample: Struct Array](#sample-struct-array)
  - [Sample: Marshalling](#sample-marshalling)
  - [Sample: Struct & StructArray Utility Classes](#sample-struct-structarray-utility-classes)

## Prologue
Project-Reloaded, also known as Reloaded 3.X (in terms of mod loader versions) exposes an interface named `IMemory` that can be used to perform memory manipulation actions.

The interface `IMemory` specifically is an interface designed to provide enhanced read/write access to *arbitrary memory*; "arbitrary memory" simply meaning *based on implementation*.

The two main implementations of `IMemory` in `Reloaded.Memory` are:
+ **Memory**: Provides Read/Write access to the memory of the current process.
+ **ExternalMemory**: Provides Read/Write access to the memory of another process.

Gaining access to memory of the current, or another process is as simple as creating an instance of either class.

```csharp
// Another process
Process helloWorldProcess = Process.Start("HelloWorld.exe");
IMemory externalMemory = new ExternalMemory(helloWorldProcess);

// Current process
IMemory externalMemory = new Memory();
```

Of course, you can create your own custom implementations of `IMemory`.
Additional functionality such as reading/writing arrays is available in the extension method `	MemoryExtensions` so make sure to include `using Reloaded.Memory.Sources`.

tl;dr: `IMemory` is an interface that allows you to read from a memory source, e.g. `Memory` for current process, `ExternalMemory` for another process.

## Class Breakdown
The following is a quick breakdown of the main classes you will probably find useful within the `Reloaded.Memory` library:

+ **ArrayPtr**: Pointer to an array in arbitrary memory.
+ **FixedArrayPtr**: Pointer to an array with known length in arbitrary memory. (Allows for LINQ, foreach etc.)
+ **Pointer**: Managed abstraction to a pointer in arbitrary memory.

All of these use the `IMemory` interface under the hood, which can be manually set.
This means that you can e.g. have a pointer to a variable in another process.

*In addition, the following utility classes are available:*

+ **Endian**: Utility method for swapping the endian of a specific variable.
+ **Struct**: Allows converting structs to bytes, converting bytes to structs, getting size of structs, writing struct to pointer etc.
+ **StructArray**: Array support for the `Struct` utility class. (The functions in `Struct`, but with arrays)

Pretty much this is all you need to know.

Play around with the classes yourself and discover what you can do. Everything below is extra.

## Source Code Samples

These are small extracts of code originally taken from a Reloaded Mod Loader Memory Manipulation example mod, largely unmodified aside from adjusting to changes in the API. 

They are heavily commented and originally served as a tutorial for the library; I kept the comments intact and slightly modified them as they may or may not still be useful.

While they are tailored to novice programmers with little experience; they also describe some of the design decisions of the library for the more experienced hackers/programmers, such as why the `out` parameters are used in reading memory.

You can see these code samples, replicated in the `Reloaded.Memory.Examples` project in the solution.

### Sample: Reading/Writing Primitives
```csharp
void PrimitivesExample(IMemory memory, IntPtr memoryLocation)
{
	// You can use the Memory Option to write any arbitrary generic primitive to memory.
    // Here is an example:
    memory.Write(memoryLocation, 1337);         // Implicitly memory.Write<int>(memoryLocation, 1337);

    // No <> brackets?
    // C# has this feature called "Type Inference"; it guesses the generic type from object supplied.
    // You typed in a number; it automatically assumed, as with each number, it was an int.

    // Nothing changes with a variable.
    int leet = 1337;
    memory.Write(memoryLocation, leet);

    // But what if you instead wanted to write a short?
    memory.Write<short>(memoryLocation, 1337);  // Implicit cast happens here.
    memory.Write(memoryLocation, (short)1337);  // Explicit cast; will also write a short.
    memory.Write(memoryLocation, (short)leet);  // Explicit cast with variable.

    // This is possible with any generic type that can be represented in the unmanaged C/C++/D/other language world.
    // Floats, Doubles, no problem.

    // And of course; reading is just as obvious.
    memory.Read(memoryLocation, out short anotherLeet); // Implicitly memory.Read<short>(memoryLocation, out short anotherLeet);

    // If you are wondering why our read methods have out parameters instead of returning values, it's a performance measure.
    // Using the out parameter, we preallocate memory on the stack and pass a reference (pointer) to the stack allocated memory
    // to the function under the hood.

    // Normally when we return a struct from a function, **a copy is made** and it is returned as a value. This is a microoptimization. 
    // Food for thought: Think of where the struct is stored normally... the stack frame of function we are returning from...
    //                   that gets trashed on exit, hence the need to copy.
}
```

### Sample: Reading/Writing Structs
```csharp
private static void WriteStructsExample(IMemory memory, IntPtr memoryLocation)
{
	// Note: Vector3 is just a struct composed of 3 floats.
    // Writing structs is no different to writing primitives; at all.
    Vector3 xyzPosition = new Vector3(1F, 2F, 3F);
    memory.Write(memoryLocation, xyzPosition);

    // Now to confirm our struct writing; let's read it back and check.
    memory.Read(memoryLocation, out Vector3 xyzPositionCopy);
}
```

### Sample: Memory Sources
```csharp
void MemorySourceExample(IMemory memory, IntPtr memoryLocation)
{
    // Earlier in the program; we have been writing generics to the program's memory using the 'Memory' class
    // implementing the IMemory interface. Well... that isn't the only stock class that implements this interface.

    // The implementation of `Memory` is just one of them; one that lets you read/write inside the current process.
    // Others you can find within Reloaded.Memory.Sources.

    // Well; let's look at another IMemory implementation, one that lets you read/write a DIFFERENT process.

    IMemory anotherProcessMemory = new ExternalMemory(Process.GetCurrentProcess());

    // ExternalMemory is yet another implementation of IMemory; allowing you to read from another process.
    // In this case we have pointed it at the current process - now let's show this working.

    // Write "1337" to memory address in external process.
    int leet = 1337;
    anotherProcessMemory.Write(memoryLocation, leet);

    // Read "1337" written by "another process" using our IMemory implementation (Memory) that reads from current process.
    memory.Read(memoryLocation, out int anotherLeet);

    // Implementing the IMemory interface is quite easy; especially with the many tools in the <Struct> class.
            
    // Extra note: Overloads for IMemory are implemented as Extension Methods
    // Make sure to add `using Reloaded.Memory.Sources;` in your own projects.
}
```

### Sample: Struct Array
```csharp
void StructArrayExample(IMemory memory, IntPtr memoryLocation)
{
    // Let's load a binary file from the disk and write it to memory.
    const int itemCount = 40; // Number of items in struct array (known).
    byte[] physicsData = File.ReadAllBytes($"phys.bin");
    memory.WriteRaw(memoryLocation, physicsData);

    // Array Read & Write from/to Memory
    memory.Read(memoryLocation, out AdventurePhysics[] adventurePhysicsData, itemCount);
    memory.Write(memoryLocation, adventurePhysicsData);

    // Pointer to array in memory. Provides enhanced functionality over a standard pointer.
    var adventurePhysics = new ArrayPtr<AdventurePhysics>((ulong)memoryLocation);
    adventurePhysics.Get(out AdventurePhysics value, 0);
    // Uh? Yeah, for performance the indexer is not overwritten.
    
    float speedCap = value.HorizontalSpeedCap;

    // Pointer to array in memory with known length. Provides even extra functionality. (Like foreach, LINQ)
    var adventurePhysicsFixed = new FixedArrayPtr<AdventurePhysics>((ulong)memoryLocation, itemCount);
    float averageAirAcceleration = adventurePhysicsFixed.Average(physics => physics.AirAcceleration); // LINQ

    // All of these classes support read/writes from arbitrary memory of course... 
    // this is where `IMemory` comes in after all.
    IMemory anotherProcessMemory = new ExternalMemory(Process.GetCurrentProcess());
    var physicsFixedOtherProcess = new FixedArrayPtr<AdventurePhysics>((ulong)memoryLocation, itemCount, false, anotherProcessMemory);
    float averageAirAcceleration2 = physicsFixedOtherProcess.Average(physics => physics.AirAcceleration);

    // What you just witnessed was LINQ over arbitrary structs inside memory of another process.

    // Foreach loop over structs in other processes? Of course.
    float greatestInitialJump = float.MinValue;
    float smallestInitialJump = float.MaxValue;
    foreach (var physics in physicsFixedOtherProcess)
    {
        if (physics.InitialJumpSpeed > greatestInitialJump)
            greatestInitialJump = physics.InitialJumpSpeed;

        if (physics.InitialJumpSpeed < smallestInitialJump)
            smallestInitialJump = physics.InitialJumpSpeed;
    }

    Console.WriteLine($"LINQ Over Arbitrary Memory: {averageAirAcceleration} (Average air Acceleration in Sonic Adventure-Heroes)");
    Console.WriteLine($"LINQ Over Memory in Another Process: {greatestInitialJump - smallestInitialJump} (Sonic Adventure-Heroes Physics delta between jump speeds)");
}
```

### Sample: Marshalling
```csharp
public struct CustomFileHeader
{
    /// <summary>
    /// Contains the actual filename of the file as ASCII encoded bytes.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Name;

    public uint Offset;
    public uint Length;
}
```

```csharp
void MarshallingExample(IMemory memory, IntPtr memoryLocation)
{
    // Marshalling is yet another feature that is supported when reading and writing from ANY IMemory source.
    // Consequently; this also means that classes based on IMemory - such as ArrayPtr or FixedArrayPtr support it under the hood.
    // This example will read simple binary struct with an inline fixed length array of strings and.

    // Let's load a binary file from the disk and write it to memory.
    byte[] characterData = File.ReadAllBytes($"CustomFileHeader.bin");
    memory.WriteRaw(memoryLocation, characterData);

    // Now let's parse it back. 
    memory.Read<CustomFileHeader>(memoryLocation, out var customHeader, true); 

    Console.WriteLine($"Marshal Test (Struct fixed length char array as string): \n" +
                      $"Name = {customHeader.Name} Offset = {customHeader.Offset} Length = {customHeader.Length}");
}
```

**Output:**
```
Marshal Test (Struct fixed length char array as string):
Name = ReloadedModLoader.dll Offset = 123456 Length = 78940
```

### Sample: Struct & StructArray Utility Classes
```csharp
void StructUtilityExample(IMemory memory, IntPtr memoryLocation)
{
    // Under the hood; the IMemory implementations may use a certain struct utility classes known as Struct
    // and StructArray which provide various methods for struct conversions and general work with structs.

    // Like earlier; let's load the adventure binary file.
    byte[] physicsData = File.ReadAllBytes($"phys.bin");

    // But this time; do a direct conversion rather than reading from memory.
    // Note that you don't even need to specify item count this time arounnd.
    // This is auto-decided from array size, but can be manually overwritten.
    StructArray.FromArray(physicsData, out AdventurePhysics[] adventurePhysics);

    // Calculate total array size (in bytes).
    int arraySize = StructArray.GetSize<AdventurePhysics>(adventurePhysics.Length);

    // Get raw bytes for the struct.
    byte[] physicsDataBack = StructArray.GetBytes(adventurePhysics);

    // You can also read/write structures; as a shorthand to Memory class.
    StructArray.ToPtr(memoryLocation, adventurePhysics);
    StructArray.FromPtr(memoryLocation, out AdventurePhysics[] adventurePhysicsCopy, adventurePhysics.Length);

    // Beware of dragons:
    // A. Struct class allows you to change the source read/write source for FromPtr and ToPtr.
    // B. It affects both Struct and StructArray.
	// Make sure you set (and preferably restore) the IMemory Struct.Source!
    Struct.Source = memory; // Struct's default source is now current process' memory.

    // Print details.
    if (physicsDataBack.SequenceEqual(physicsDataBack))
        Console.WriteLine($"Success: Original Physics Data and StructArray.GetBytes() are Equal");

    Console.WriteLine($"Struct Array Size: {arraySize}");
}
```

**Output:**
```
Success: Original Physics Data and StructArray.GetBytes() are Equal
Struct Array Size: 5280
```
