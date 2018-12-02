using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Reloaded.Memory.Example.Structs;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Example
{
    class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Reloaded 3.X+ exposes the <IMemory> interface that can be used to read/write arbitrary memory.
            // You can find implementations such as Reloaded.Memory.Sources;

            // Get a new instance of "Memory" class implementing "IMemory", that provides access to current process' memory.
            IMemory memory = Sources.Memory.CurrentProcess;      // Static/Preinitialized access to current process' memory.

            // Tutorial 0: Allocate/Free Memory
            IntPtr memoryLocation = memory.Allocate(65535); // Did you think it would be harder?
                                                            // Here's 65535 bytes at memoryLocation.
                                                            // You would free it with memory.Free(memoryLocation);

            // Tutorial 1: Basic Reading/Writing Primitives
            PrimitivesExample(memory, memoryLocation);

            // Tutorial 2: Writing Structs
            WriteStructsExample(memory, memoryLocation);

            // Tutorial 3: Memory Sources [Other Processes etc.]
            MemorySourceExample(memory, memoryLocation);

            // Tutorial 4: Struct Arrays
            StructArrayExample(memory, memoryLocation);

            // Tutorial 5: Marshalling
            MarshallingExample(memory, memoryLocation);

            // Tutorial 6: Struct & StructArray Utility Classes
            StructUtilityExample(memory, memoryLocation);

            // Cleanup
            memory.Free(memoryLocation);

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// A simple method example which demonstrates writing a simple primitives to memory.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void PrimitivesExample(IMemory memory, IntPtr memoryLocation)
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
            memory.Read(memoryLocation, out short anotherLeet);     // Implicitly memory.Read<short>(memoryLocation, out short anotherLeet);

            // If you are wondering why our read methods have out parameters instead of returning values, it's a performance measure.
            // Using the out parameter, we preallocate memory on the stack and pass a reference (pointer) to the stack allocated memory
            // to the function under the hood.

            // Normally when we return a struct from a function, **a copy is made** and it is returned as a value. This is a microoptimization. 
            // Food for thought: Think of where the struct is stored normally... the stack frame of function we are returning from...
            //                   that gets trashed on exit, hence the need to copy.
        }

        /// <summary>
        /// A simple method example which demonstrates writing structs.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void WriteStructsExample(IMemory memory, IntPtr memoryLocation)
        {
            // Note: Vector3 is just a struct composed of 3 floats.
            // Writing structs is no different to writing primitives; at all.
            Vector3 xyzPosition = new Vector3(1F, 2F, 3F);
            memory.Write(memoryLocation, xyzPosition);

            // Now to confirm our struct writing; let's read it back and check.
            memory.Read(memoryLocation, out Vector3 xyzPositionCopy);
        }

        /// <summary>
        /// A simple method example which demonstrates the <see cref="IMemory"/> interface.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void MemorySourceExample(IMemory memory, IntPtr memoryLocation)
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

        /// <summary>
        /// A simple method example which demonstrates struct Array operations.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void StructArrayExample(IMemory memory, IntPtr memoryLocation)
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
            adventurePhysics.Get(out AdventurePhysics value, 0);                        // And of course read/writes work..
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

        /// <summary>
        /// A simple method example which demonstrates marshalling at work.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void MarshallingExample(IMemory memory, IntPtr memoryLocation)
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

        /// <summary>
        /// Shows some functionality of the <see cref="Struct"/> and <see cref="StructArray"/> utility classes.
        /// </summary>
        /// <param name="memory">This object is used to perform memory read/write/free/allocate operations.</param>
        /// <param name="memoryLocation">Arbitrary location in memory where this tutorial will be held.</param>
        private static void StructUtilityExample(IMemory memory, IntPtr memoryLocation)
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

            // Beware of the double sided blade however.
            // A. Struct class allows you to change the source read/write source for FromPtr and ToPtr.
            // B. It affects both Struct and StructArray.

            // Note: There are also explicit overloads for FromPtr and ToPtr that let you use a source without modifying current source.
            Struct.Source = memory; // And of course the source is an implementation of IMemory.

            // Print details.
            if (physicsDataBack.SequenceEqual(physicsDataBack))
                Console.WriteLine($"Success: Original Physics Data and StructArray.GetBytes() are Equal");

            Console.WriteLine($"Struct Array Size: {arraySize}");
        }
    }
}
