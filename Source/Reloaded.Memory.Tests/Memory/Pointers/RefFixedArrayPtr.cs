using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class RefFixedArrayPtr
    {
        [Fact]
        public unsafe void SetElement()
        {
            int[] numbers = { 0, 0, 0, 0, 0, 0, 0, 0 };
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(numbersPtr, numbers.Length);

                for (int x = 0; x < numbers.Length; x++)
                {
                    ref int currentNumber = ref arrayPtr[x];
                    currentNumber = x;
                    Assert.Equal(currentNumber, numbers[x]);
                }
            }
        }

        [Fact]
        public unsafe void GetElement()
        {
            int[] numbers = { 0, 0, 0, 0, 0, 0, 0, 0 };
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(numbersPtr, numbers.Length);

                for (int x = 0; x < numbers.Length; x++)
                {
                    numbers[x] = x;
                    ref int currentNumber = ref arrayPtr[x];
                    Assert.Equal(numbers[x], currentNumber);
                }
            }
        }

        [Fact]
        public unsafe void Contains()
        {
            int[] numbers = { 0, 2, 4, 6, 8, 10, 12, 14 };
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(numbersPtr, numbers.Length);
                Assert.True(arrayPtr.Contains(2));
                Assert.True(arrayPtr.Contains(4));
                Assert.True(arrayPtr.Contains(6));
                Assert.True(arrayPtr.Contains(8));
                Assert.True(arrayPtr.Contains(12));
                Assert.True(arrayPtr.Contains(14));

                Assert.False(arrayPtr.Contains(-1));
                Assert.False(arrayPtr.Contains(1));
                Assert.False(arrayPtr.Contains(3));
            }
        }

        [Fact]
        public unsafe void IndexOf()
        {
            int[] numbers = { 0, 2, 4, 6, 8, 10, 12, 14 };
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(numbersPtr, numbers.Length);
                Assert.Equal(1, arrayPtr.IndexOf(2));
                Assert.Equal(2, arrayPtr.IndexOf(4));
                Assert.Equal(3, arrayPtr.IndexOf(6));
                Assert.Equal(4, arrayPtr.IndexOf(8));
                Assert.Equal(6, arrayPtr.IndexOf(12));
                Assert.Equal(7, arrayPtr.IndexOf(14));

                Assert.Equal(-1, arrayPtr.IndexOf(-1));
                Assert.Equal(-1, arrayPtr.IndexOf(1));
                Assert.Equal(-1, arrayPtr.IndexOf(3));
            }
        }

        [Fact]
        public unsafe void CopyTo()
        {
            int[] source        = { 0, 2, 4, 6, 8, 10, 12, 14 };
            int[] destination   = { 0, 0, 0, 0, 0, 0, 0, 0 };

            fixed (int* sourcePtr = source)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(sourcePtr, source.Length);
                arrayPtr.CopyTo(destination, source.Length);

                for (int x = 0; x < arrayPtr.Count; x++)
                    Assert.Equal(source[x], destination[x]);
            }
        }

        [Fact]
        public unsafe void CopyFrom()
        {
            int[] source      = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] destination = { 0, 2, 4, 6, 8, 10, 12, 14 };

            fixed (int* sourcePtr = source)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefFixedArrayPtr<int>(sourcePtr, source.Length);
                arrayPtr.CopyFrom(destination, destination.Length);

                for (int x = 0; x < arrayPtr.Count; x++)
                    Assert.Equal(source[x], destination[x]);
            }
        }
    }
}
