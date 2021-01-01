using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Shared.Structs;
using Reloaded.Memory.Tests.Memory.Helpers;
using Reloaded.Memory.Utilities;
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

        [Fact]
        public unsafe void IsBlittable()
        {
            Assert.True(Blittable.IsBlittable<BlittablePointer<byte>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<sbyte>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<short>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<ushort>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<int>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<uint>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<long>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<ulong>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<IntPtr>>());
            Assert.True(Blittable.IsBlittable<BlittablePointer<UIntPtr>>());

            Assert.True(Blittable.IsBlittable<BlittablePointer<RandomIntStruct>>());
        }
    }
}
