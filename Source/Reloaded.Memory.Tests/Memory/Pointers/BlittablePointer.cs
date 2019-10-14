using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Pointers;
using Xunit;

namespace Reloaded.Memory.Tests.Memory.Pointers
{
    public class BlittablePointer
    {
        [Fact]
        public unsafe void ArrayOfPointersSetElement()
        {
            // Make array of four pointers.
            int a = 1;
            int b = 2;
            int c = 3;
            int d = 4;

            BlittablePointer<int>[] arrayOfPointers =
            {
                new BlittablePointer<int>(&a), 
                new BlittablePointer<int>(&b), 
                new BlittablePointer<int>(&c), 
                new BlittablePointer<int>(&d)
            };

            Assert.Equal(a, arrayOfPointers[0].AsReference());
            Assert.Equal(b, arrayOfPointers[1].AsReference());
            Assert.Equal(c, arrayOfPointers[2].AsReference());
            Assert.Equal(d, arrayOfPointers[3].AsReference());

            d = 99;
            Assert.Equal(99, arrayOfPointers[3].AsReference());
        }
    }
}
