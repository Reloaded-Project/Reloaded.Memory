using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Reloaded.Memory.Exceptions;
using Reloaded.Memory.Tests.Memory.Helpers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Sources
{
    /* Note: The tests in this can randomly fail. */


    /// <summary>
    /// An implementation of an IEnumerator that contains different Memory sources to test against.
    /// </summary>
    public class IMemoryGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { new Reloaded.Memory.Sources.Memory() },
            new object[] { new Reloaded.Memory.Sources.ExternalMemory(Process.GetCurrentProcess()) }
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
    }

    // ReSharper disable once InconsistentNaming
    public class IMemory : IDisposable
    {
        // Create dummy HelloWorld.exe
        private Process _helloWorldProcess;

        public IMemory()
        {
            // Cleanup after possible dirty exit.
            var processes = Process.GetProcessesByName("HelloWorld.exe");
            foreach (var process in processes)
            {
                process.Kill();
                process.Dispose();
            }

            _helloWorldProcess = Process.Start("HelloWorld.exe");
        }

        // Dispose of HelloWorld.exe
        public void Dispose()
        {
            _helloWorldProcess.Kill();
            _helloWorldProcess.Dispose();
        }

        /// <summary>
        /// Attempts to allocate memory, checks if result points to valid memory (nonzero).
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void AllocateMemory(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);

            // Test
            IntPtr pointer = memorySource.Allocate(0xFFFF);
            Assert.NotEqual((IntPtr)0, pointer);
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write primitives to a specific allocated memory address, then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWritePrimitives(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random integer read/write.
            for (int x = 0; x < 100; x++)
            {
                int randomValue = new Random().Next();
                memorySource.Write(pointer, ref randomValue);
                memorySource.Read(pointer, out int randomValueCopy);
                Assert.Equal(randomValue, randomValueCopy);
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write primitives to a specific allocated memory address, then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteRawData(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            int allocationSize = 0x100;
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(allocationSize);

            /* Start Test */

            // Random integer read/write.
            for (int x = 0; x < 100; x++)
            {
                RandomByteArray randomArray = RandomByteArray.GenerateRandomByteArray(allocationSize);
                memorySource.WriteRaw(pointer, randomArray.Array);
                memorySource.ReadRaw (pointer, out byte[] randomValueCopy, randomArray.Array.Length);
                Assert.Equal(randomArray.Array, randomValueCopy);
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write structs to a specific allocated memory address, then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteStructs(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random struct read/write.
            for (int x = 0; x < 100; x++)
            {
                RandomIntStruct randomIntStruct = RandomIntStruct.BuildRandomStruct();
                memorySource.Write(pointer, ref randomIntStruct);
                memorySource.Read(pointer, out RandomIntStruct randomValueCopy);
                Assert.Equal(randomIntStruct, randomValueCopy);
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write structs, WITH MARSHALLING to a specific allocated memory address. Then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ReadWriteWithMarshalling(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Random marshal struct read/write.
            for (int x = 0; x < 100; x++)
            {
                MarshallingStruct randomIntStruct = MarshallingStruct.BuildRandomStruct();
                memorySource.Write(pointer, ref randomIntStruct, true);
                memorySource.Read(pointer, out MarshallingStruct randomValueCopy, true);

                // Test for equality.
                Assert.Equal(randomIntStruct, randomValueCopy);

                // Test references:
                // If marshalling did not take place, write function would have written pointer to string and read it back in.
                // If marshalling did take place, a new string was created with the value of the string found in memory.
                // Set marshal parameter to false in read/write operation above to test this.
                Assert.False(object.ReferenceEquals(randomIntStruct.Name, randomValueCopy.Name));
            }

            /* End Test */

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write structs, WITH MARSHALLING to a specific allocated memory address. Then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ChangePermissions(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(0x100);

            /* Start Test */

            // Run the change permission function to deny read/write access.
            try { memorySource.ChangePermission(pointer, 0x100, Kernel32.Kernel32.MEM_PROTECTION.PAGE_NOACCESS); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // NETCore removed handling of Corrupted State Exceptions https://github.com/dotnet/coreclr/issues/9045
            // We cannot properly test the method.

            // Restore or NETCore execution engine will complain.
            try { memorySource.ChangePermission(pointer, 0x100, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to write structs, WITH MARSHALLING to a specific allocated memory address. Then attempts to read the written value.
        /// </summary>
        /// <param name="memorySource">Memory source to test allocation for.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void ChangePermissionFail(Reloaded.Memory.Sources.IMemory memorySource)
        {
            /* Start Test */

            // Run the change permission function to deny read/write access.
            try { memorySource.ChangePermission((IntPtr)(-1), 0x100, Kernel32.Kernel32.MEM_PROTECTION.PAGE_NOACCESS); }
            catch (NotImplementedException)          { return; } // ChangePermission is optional to implement
            catch (MemoryPermissionException) { return; } // Thrown as expected.

            // Cleanup on fail.
            memorySource.ChangePermission((IntPtr)(-1), 0x100, Kernel32.Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            Assert.True(false, "This method should throw CannotChangePermissionsException");
        }
    }
}
