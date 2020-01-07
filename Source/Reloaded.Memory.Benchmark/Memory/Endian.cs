using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Reloaded.Memory.Benchmark.Memory
{
    [CoreJob]
    public class Endian
    {
        // Number of Iterations
        public int Iterations { get; set; } = 10000;

        [Benchmark]
        public short IntrinsicReverseEndianShort()
        {
            short random = 0x28FB;
            short swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                random = Reloaded.Memory.Endian.Reverse(random);
            }

            return swapped;
        }

        [Benchmark]
        public short GenericReverseEndianShort()
        {
            short random = 0x28FB;
            short swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                Reloaded.Memory.Endian.Reverse(ref random, out swapped);
            }

            return swapped;
        }

        [Benchmark]
        public short OldGenericReverseEndianShort()
        {
            short random = 0x28FB;
            short swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                OldReverse(ref random, out swapped);
            }

            return swapped;
        }

        [Benchmark]
        public int IntrinsicReverseEndianInt()
        {
            int random = 0x11223344;
            int swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                random = Reloaded.Memory.Endian.Reverse(random);
            }

            return swapped;
        }

        [Benchmark]
        public int GenericReverseEndianInt()
        {
            int random = 0x11223344;
            int swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                Reloaded.Memory.Endian.Reverse(ref random, out swapped);
            }

            return swapped;
        }

        [Benchmark]
        public int OldGenericReverseEndianInt()
        {
            int random = 0x11223344;
            int swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                OldReverse(ref random, out swapped);
            }

            return swapped;
        }

        [Benchmark]
        public long IntrinsicReverseEndianLong()
        {
            long random = 0x1122334455667788;
            long swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                random = Reloaded.Memory.Endian.Reverse(random);
            }

            return swapped;
        }

        [Benchmark]
        public long GenericReverseEndianLong()
        {
            long random = 0x1122334455667788;
            long swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                Reloaded.Memory.Endian.Reverse(ref random, out swapped);
            }

            return swapped;
        }

        [Benchmark]
        public long OldGenericReverseEndianLong()
        {
            long random = 0x1122334455667788;
            long swapped = 0;
            for (int x = 0; x < Iterations; x++)
            {
                OldReverse(ref random, out swapped);
            }

            return swapped;
        }

        private static void OldReverse<T>(ref T type, out T swapped) where T : unmanaged
        {
            byte[] data = Reloaded.Memory.Struct.GetBytes(ref type);
            Array.Reverse(data);
            Reloaded.Memory.Struct.FromArray<T>(data, out swapped, false);
        }
    }
}
