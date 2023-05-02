using System;
using Reloaded.Memory.Pointers;
using Reloaded.Memory.Tests.Utilities.Structures;
using Reloaded.Memory.Utility;
using Xunit;

namespace Reloaded.Memory.Tests.Tests.Pointers;

public class Ptr
{
    [Fact]
    public unsafe void Test_UsingArrayOfPointers()
    {
        // Make array of four pointers.
        int a = 1;
        int b = 2;
        int c = 3;
        int d = 4;

        Ptr<int>[] arrayOfPointers =
        {
            new(&a),
            new(&b),
            new(&c),
            new(&d)
        };

        Assert.Equal(a, arrayOfPointers[0].AsRef());
        Assert.Equal(b, arrayOfPointers[1].AsRef());
        Assert.Equal(c, arrayOfPointers[2].AsRef());
        Assert.Equal(d, arrayOfPointers[3].AsRef());

        d = 99;
        Assert.Equal(99, arrayOfPointers[3].AsRef());
    }

    [Fact]
    public unsafe int* Cast_Implicit()
    {
        var a = 5;
        return new Ptr<int>(&a);
    }

    [Fact]
    public unsafe Ptr<int> Cast_Implicit_Back()
    {
        var a = 5;
        return &a;
    }

    [Fact]
    public void IsBlittable()
    {
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<byte>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<sbyte>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<short>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<ushort>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<int>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<uint>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<long>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<ulong>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<IntPtr>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<UIntPtr>>());
        Assert.True(Blittable.ApproximateIsBlittable<Ptr<RandomIntStruct>>());
    }
}
