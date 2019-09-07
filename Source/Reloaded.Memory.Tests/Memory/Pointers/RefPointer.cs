using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class RefPointer
    {
        [Fact]
        public unsafe void TryDereferenceAlternateSyntax()
        {
            int number = 1;
            int* numberPtr = &number;
            int** numberPtrPtr = &numberPtr;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*)numberPtrPtr, 2);
            for (int x = 0; x < 100; x++)
            {
                number = x;

                int newNumber = 0;
                bool success = refPointer.TryDereference(ref newNumber);

                Assert.True(success);
                Assert.Equal(number, newNumber);
            }
        }

        [Fact]
        public unsafe void ReadMultiLevelPointer()
        {
            int number = 1;
            int* numberPtr = &number;
            int** numberPtrPtr = &numberPtr;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*) numberPtrPtr, 2);
            for (int x = 0; x < 100; x++)
            {
                number = x;
                ref int newNumber = ref refPointer.TryDereference(out bool success);
                Assert.True(success);
                Assert.Equal(number, newNumber);
            }
        }

        [Fact]
        public unsafe void WriteMultiLevelPointer()
        {
            int number = 1;
            int* numberPtr = &number;
            int** numberPtrPtr = &numberPtr;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*)numberPtrPtr, 2);
            for (int x = 0; x < 100; x++)
            {
                ref int newNumber = ref refPointer.TryDereference(out bool success);
                newNumber = x;
                Assert.True(success);
                Assert.Equal(number, newNumber);
            }
        }

        [Fact]
        public unsafe void ReadSingleLevelPointer()
        {
            int number = 1;
            int* numberPtr = &number;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*)numberPtr, 1);
            for (int x = 0; x < 100; x++)
            {
                number = x;
                ref int newNumber = ref refPointer.TryDereference(out bool success);
                Assert.True(success);
                Assert.Equal(number, newNumber);
            }
        }

        [Fact]
        public unsafe void WriteSingleLevelPointer()
        {
            int number = 1;
            int* numberPtr = &number;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*)numberPtr, 1);
            for (int x = 0; x < 100; x++)
            {
                ref int newNumber = ref refPointer.TryDereference(out bool success);
                newNumber = x;
                Assert.True(success);
                Assert.Equal(number, newNumber);
            }
        }


        [Fact]
        public unsafe void FailDereference()
        {
            int number = 1;
            int* numberPtr = &number;
            int** numberPtrPtr = &numberPtr;

            // Invalidate pointer in middle.
            numberPtr = (int*) 0;

            var refPointer = new Reloaded.Memory.Pointers.RefPointer<int>((int*)numberPtrPtr, 2);
            for (int x = 0; x < 100; x++)
            {
                number = x;
                ref int newNumber = ref refPointer.TryDereference(out bool success);
                Assert.False(success);
            }
        }
    }
}
