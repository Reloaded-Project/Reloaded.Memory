using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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
            var numberPtr = new Reloaded.Memory.Pointers.Pointer<int>((ulong) &number, false, new Reloaded.Memory.Sources.Memory()); // Last parameter only for coverage.

            // Change value normally and try picking up new change.
            number = 10;
            numberPtr.GetValue(out int numberCopy);

            Assert.Equal(number, numberCopy);

            // Change value via Pointer class and try picking up change.
            int newValue = 15;
            numberPtr.SetValue(ref newValue);
            Assert.Equal(newValue, number);
        }
    }
}
