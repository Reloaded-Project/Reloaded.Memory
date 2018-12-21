using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Reloaded.Memory.Tests.Memory.Helpers
{
    /// <summary>
    /// An implementation of an IEnumerator that contains different Memory sources to test against.
    /// </summary>
    public class IArrayPtrGenerator : IEnumerable<object[]>, IDisposable
    {
        public static Reloaded.Memory.Sources.Memory CurrentProcess = Reloaded.Memory.Sources.Memory.CurrentProcess;
        public static IntPtr AdventurePhysicsArray;

        /// <summary>
        /// The amount of elements in phys.bin. When we go test <see cref="StructArray"/>, we will test for autodetection of this value.
        /// </summary>
        public static int PhysicsArrayLength = 40;

        /// <summary>
        /// Set up this function by copying over an array of Sonic Adventure physics.
        /// </summary>
        public IArrayPtrGenerator()
        {
            // Read in the Sonic Adventure physics array from file, then copy over to self-allocated memory.
            byte[] bytes = File.ReadAllBytes("phys.bin");

            AdventurePhysicsArray = CurrentProcess.Allocate(bytes.Length);
            CurrentProcess.WriteRaw(AdventurePhysicsArray, bytes);

            _data = new List<object[]>
            {
                new object[] { new Reloaded.Memory.Pointers.ArrayPtr     <AdventurePhysics>((ulong) AdventurePhysicsArray, false, CurrentProcess) },
                new object[] { new Reloaded.Memory.Pointers.FixedArrayPtr<AdventurePhysics>((ulong) AdventurePhysicsArray, 10, false, CurrentProcess) }
            };
        }

        public void Dispose()
        {
            CurrentProcess.Free(AdventurePhysicsArray);
        }

        /* IEnumerable implementation to feed Theory Data. */
        private readonly List<object[]> _data;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
    }
}
