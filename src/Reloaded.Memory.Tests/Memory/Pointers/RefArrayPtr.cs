using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class RefArrayPtr
    {
        [Fact]
        public unsafe void SetElement()
        {
            int[] numbers = {0, 0, 0, 0, 0, 0, 0, 0};
            fixed (int* numbersPtr = numbers)
            {
                var arrayPtr = new Reloaded.Memory.Pointers.RefArrayPtr<int>(numbersPtr);

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
                var arrayPtr = new Reloaded.Memory.Pointers.RefArrayPtr<int>(numbersPtr);

                for (int x = 0; x < numbers.Length; x++)
                {
                    numbers[x] = x;
                    ref int currentNumber = ref arrayPtr[x];
                    Assert.Equal(numbers[x], currentNumber);
                }
            }
        }
    }
}
