using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reloaded.Memory.Tests
{
    public class Endian
    {
        /*
         *  This file tests the "Endian" class inside the Reloaded.Memory class library.
         *  The tests here simply involve byteswapping of various primitives.
         */

        [Fact]
        public void SwapEndianShort()
        {
            short input     = 0x7530;
            short expected  = 0x3075;

            Memory.Endian.Reverse(ref input, out short swapped);
            Assert.Equal(expected, swapped);
        }

        [Fact]
        public void SwapEndianInt()
        {
            int input       = 0x11223344;
            int expected    = 0x44332211;

            Memory.Endian.Reverse(ref input, out int swapped);
            Assert.Equal(expected, swapped);
        }

        [Fact]
        public void SwapEndianByte()
        {
            byte input      = 0x44;
            byte expected   = 0x44;

            Memory.Endian.Reverse(ref input, out byte swapped);
            Assert.Equal(expected, swapped);
        }

        [Fact]
        public void SwapEndianFloat()
        {
            float input    = 5F;
            float expected = 5.748687e-41F;

            Memory.Endian.Reverse(ref input, out float swapped);
            Assert.Equal(expected, swapped, 6);     // 6 = Minimum float precision.
        }

        [Fact]
        public void SwapEndianDouble()
        {
            double input    = 5F;
            double expected = 2.56123630804102e-320F;

            Memory.Endian.Reverse(ref input, out double swapped);
            Assert.Equal(expected, swapped, 15);    // 15 = Minimum double precision
        }

        [Fact]
        public void SwapEndianBool()
        {
            // .NET Internal representation of a bool is a byte.
            // So `true` is 0x01
            bool input      = true;
            bool expected   = true;

            Memory.Endian.Reverse(ref input, out bool swapped);
            Assert.Equal(expected, swapped);
        }
    }
}
