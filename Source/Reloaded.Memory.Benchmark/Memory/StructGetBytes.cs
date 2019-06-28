using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using Reloaded.Memory.Benchmark.Benchmarking.Structs;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Benchmark.Memory
{
    [CoreJob]
    public unsafe class StructGetBytes
    {   
        #region Old Code
        public IMemory Memory = new Sources.Memory();

        public byte[] GetBytesOldFn<T>(ref T item, bool marshalElement = false)
        {
            int size = Reloaded.Memory.Struct.GetSize<T>(marshalElement);
            byte[] array = new byte[size];

            fixed (byte* arrayPtr = array)
            {
                Reloaded.Memory.Struct.ToPtr((IntPtr)arrayPtr, ref item, Memory.Write, marshalElement);
            }

            return array;
        }
        #endregion Old Code
        public int Iterations { get; set; } = 100000;

        [Benchmark]
        public byte[] GetBytesUnmanagedNew()
        {
            Vector3 vector = new Vector3(100, 100, 100);
            byte[] bytes = null;
            for (int x = 0; x < Iterations; x++)
            {
                bytes = Reloaded.Memory.Struct.GetBytes(ref vector);
            }

            return bytes;
        }

        [Benchmark]
        public byte[] GetBytesUnmanagedUsingOverload()
        {
            Vector3 vector = new Vector3(100, 100, 100);
            byte[] bytes = null;
            for (int x = 0; x < Iterations; x++)
            {
                bytes = Reloaded.Memory.Struct.GetBytes(ref vector, false);
            }

            return bytes;
        }

        [Benchmark]
        public byte[] GetBytesUnmanagedOld()
        {
            Vector3 vector = new Vector3(100,100,100);
            byte[] bytes = null;
            for (int x = 0; x < Iterations; x++)
            {
                bytes = GetBytesOldFn(ref vector);
            }

            return bytes;
        }
    }
}
