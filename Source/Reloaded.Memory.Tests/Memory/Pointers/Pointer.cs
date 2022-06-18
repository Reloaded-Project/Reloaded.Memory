﻿using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class Pointer
    {
        /// <summary>
        /// Tests the <see cref="Pointer"/> class' get and set value.
        /// </summary>
        [Fact]
        public unsafe void GetSetValue()
        {
            int number = 5;
            var numberPtr = new Reloaded.Memory.Pointers.Pointer<int>((nuint)(&number), false, new Reloaded.Memory.Sources.Memory()); // Last parameter only for coverage.

            // Change value normally and try picking up new change.
            number = 10;
            numberPtr.GetValue(out int numberCopy);
            var otherNumberCopy = numberPtr.GetValue();

            Assert.Equal(number, numberCopy);
            Assert.Equal(number, otherNumberCopy);

            // Change value via Pointer class and try picking up change.
            int newValue = 15;
            numberPtr.SetValue(ref newValue);
            Assert.Equal(newValue, number);

            int otherNewValue = 25;
            numberPtr.SetValue(otherNewValue);
            Assert.Equal(otherNewValue, number);
        }
    }
}
