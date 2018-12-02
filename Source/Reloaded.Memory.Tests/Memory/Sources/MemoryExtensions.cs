using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Sources;
using Reloaded.Memory.Tests.Memory.Helpers;
using Vanara.PInvoke;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Sources
{
    public class MemoryExtensions : IDisposable
    {
        // Create dummy HelloWorld.exe
        private Process _helloWorldProcess;
        public MemoryExtensions() { _helloWorldProcess = Process.Start("HelloWorld.exe"); }

        // Dispose of HelloWorld.exe
        public void Dispose()
        {
            _helloWorldProcess.Kill();
            _helloWorldProcess.Dispose();
        }


        /// <summary>
        /// Attempts to perform a read operation from a memory page/segment with no access rights.
        /// </summary>
        /// <param name="memorySource">The memory source to perform read/write operation.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void SafeReadWrite(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(Struct.GetSize<RandomIntStruct>());

            /* Start Test */

            // Generate random int struct to read/write to.
            var randomIntStruct = RandomIntStruct.BuildRandomStruct();

            // Run the change permission function to deny read/write access.
            try { memorySource.ChangePermission(pointer, Struct.GetSize<RandomIntStruct>(), Kernel32.MEM_PROTECTION.PAGE_NOACCESS); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Throws corrupted state exception if operations fail until restore.
            memorySource.SafeWrite(pointer, ref randomIntStruct);
            memorySource.SafeRead(pointer , out RandomIntStruct randomIntStructCopy);

            // Restore or NETCore execution engine will complain.
            try { memorySource.ChangePermission(pointer, Struct.GetSize<RandomIntStruct>(), Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Compare before exiting test.
            Assert.Equal(randomIntStruct, randomIntStructCopy);

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to perform a read operation from a memory page/segment with no access rights.
        /// </summary>
        /// <param name="memorySource">The memory source to perform read/write operation.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void SafeReadWriteRaw(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            int arrayElements = 13432;
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(arrayElements);

            /* Start Test */

            // Generate random int struct to read/write to.
            var randomByteArray = RandomByteArray.GenerateRandomByteArray(arrayElements);

            // Run the change permission function to deny read/write access.
            try { memorySource.ChangePermission(pointer, arrayElements, Kernel32.MEM_PROTECTION.PAGE_NOACCESS); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Throws corrupted state exception if operations fail until restore.
            memorySource.SafeWriteRaw(pointer, randomByteArray.Array);
            memorySource.SafeReadRaw (pointer, out byte[] randomByteArrayCopy, randomByteArray.Array.Length);

            // Restore or NETCore execution engine will complain.
            try { memorySource.ChangePermission(pointer, arrayElements, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Compare before exiting test.
            Assert.Equal(randomByteArray.Array, randomByteArrayCopy);

            // Cleanup 
            memorySource.Free(pointer);
        }

        /// <summary>
        /// Attempts to perform a read operation from a memory page/segment with no access rights.
        /// </summary>
        /// <param name="memorySource">The memory source to perform read/write operation.</param>
        [Theory]
        [ClassData(typeof(IMemoryGenerator))]
        public void SafeReadWriteArray(Reloaded.Memory.Sources.IMemory memorySource)
        {
            // Prepare
            int arrayElements = 500;
            IMemoryTools.SwapExternalMemorySource(ref memorySource, _helloWorldProcess);
            IntPtr pointer = memorySource.Allocate(Reloaded.Memory.StructArray.GetSize<RandomIntStruct>(arrayElements));

            /* Start Test */

            // Generate random int struct to read/write to.
            var randomIntStructArray = new RandomIntStruct[arrayElements];
            for (int x = 0; x < randomIntStructArray.Length; x++)
                randomIntStructArray[x] = RandomIntStruct.BuildRandomStruct();

            // Run the change permission function to deny read/write access.
            try { memorySource.ChangePermission(pointer, Reloaded.Memory.StructArray.GetSize<RandomIntStruct>(arrayElements), Kernel32.MEM_PROTECTION.PAGE_NOACCESS); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Throws corrupted state exception if operations fail until restore.
            memorySource.SafeWrite(pointer, randomIntStructArray);
            memorySource.SafeRead(pointer, out RandomIntStruct[] randomIntStructArrayCopy, arrayElements);

            // Restore or NETCore execution engine will complain.
            try { memorySource.ChangePermission(pointer, Reloaded.Memory.StructArray.GetSize<RandomIntStruct>(arrayElements), Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE); }
            catch (NotImplementedException) { return; } // ChangePermission is optional to implement

            // Compare before exiting test.
            Assert.Equal(randomIntStructArray, randomIntStructArrayCopy);

            // Cleanup 
            memorySource.Free(pointer);
        }
    }
}
