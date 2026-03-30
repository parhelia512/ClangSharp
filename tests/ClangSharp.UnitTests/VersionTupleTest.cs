// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using ClangSharp.Interop;
using NUnit.Framework;

namespace ClangSharp.UnitTests;

public class VersionTupleTest
{
    [Test]
    public void Empty_IsEmpty()
    {
        Assert.That(VersionTuple.Empty.IsEmpty, Is.True);
    }

    [Test]
    public void MajorOnly_Zero_IsEmpty()
    {
        var vt = new VersionTuple(0);
        Assert.That(vt.IsEmpty, Is.True);
    }

    [Test]
    public void MajorMinor_Zero_IsEmpty()
    {
        var vt = new VersionTuple(0, 0);
        Assert.That(vt.IsEmpty, Is.True);
    }

    [Test]
    public void MajorMinorSubminor_Zero_IsEmpty()
    {
        var vt = new VersionTuple(0, 0, 0);
        Assert.That(vt.IsEmpty, Is.True);
    }

    [Test]
    public void MajorMinorSubminorBuild_Zero_IsEmpty()
    {
        var vt = new VersionTuple(0, 0, 0, 0);
        Assert.That(vt.IsEmpty, Is.True);
    }

    [Test]
    public void NonZeroMajor_IsNotEmpty()
    {
        var vt = new VersionTuple(1);
        Assert.That(vt.IsEmpty, Is.False);
    }

    [Test]
    public void NonZeroMinor_IsNotEmpty()
    {
        var vt = new VersionTuple(0, 1);
        Assert.That(vt.IsEmpty, Is.False);
    }

    [Test]
    public void NonZeroSubminor_IsNotEmpty()
    {
        var vt = new VersionTuple(0, 0, 1);
        Assert.That(vt.IsEmpty, Is.False);
    }

    [Test]
    public void NonZeroBuild_IsNotEmpty()
    {
        var vt = new VersionTuple(0, 0, 0, 1);
        Assert.That(vt.IsEmpty, Is.False);
    }

    [Test]
    public void HasMinor_ReflectsConstructor()
    {
        Assert.That(new VersionTuple(1).HasMinor, Is.False);
        Assert.That(new VersionTuple(1, 0).HasMinor, Is.True);
    }

    [Test]
    public void HasSubminor_ReflectsConstructor()
    {
        Assert.That(new VersionTuple(1, 0).HasSubminor, Is.False);
        Assert.That(new VersionTuple(1, 0, 0).HasSubminor, Is.True);
    }

    [Test]
    public void HasBuild_ReflectsConstructor()
    {
        Assert.That(new VersionTuple(1, 0, 0).HasBuild, Is.False);
        Assert.That(new VersionTuple(1, 0, 0, 0).HasBuild, Is.True);
    }

    [Test]
    public void Properties_ReturnCorrectValues()
    {
        var vt = new VersionTuple(1, 2, 3, 4);
        Assert.That(vt.Major, Is.EqualTo(1));
        Assert.That(vt.Minor, Is.EqualTo(2));
        Assert.That(vt.Subminor, Is.EqualTo(3));
        Assert.That(vt.Build, Is.EqualTo(4));
    }

    [Test]
    public void Properties_NullableWhenNotSet()
    {
        var vt = new VersionTuple(5);
        Assert.That(vt.Major, Is.EqualTo(5));
        Assert.That(vt.Minor, Is.Null);
        Assert.That(vt.Subminor, Is.Null);
        Assert.That(vt.Build, Is.Null);
    }

    [Test]
    public void Equality_SameValues()
    {
        var a = new VersionTuple(1, 2, 3, 4);
        var b = new VersionTuple(1, 2, 3, 4);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equality_DifferentValues()
    {
        var a = new VersionTuple(1, 2, 3, 4);
        var b = new VersionTuple(1, 2, 3, 5);
        Assert.That(a, Is.Not.EqualTo(b));
    }

    [Test]
    public void Equality_MajorOnlyVsMajorMinorZero()
    {
        var a = new VersionTuple(1);
        var b = new VersionTuple(1, 0);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void LessThan()
    {
        Assert.That(new VersionTuple(1) < new VersionTuple(2), Is.True);
        Assert.That(new VersionTuple(1, 0) < new VersionTuple(1, 1), Is.True);
        Assert.That(new VersionTuple(2) < new VersionTuple(1), Is.False);
        Assert.That(new VersionTuple(1, 1) < new VersionTuple(1, 1), Is.False);
    }

    [Test]
    public void GreaterThan()
    {
        Assert.That(new VersionTuple(2) > new VersionTuple(1), Is.True);
        Assert.That(new VersionTuple(1, 1) > new VersionTuple(1, 0), Is.True);
        Assert.That(new VersionTuple(1) > new VersionTuple(2), Is.False);
        Assert.That(new VersionTuple(1, 1) > new VersionTuple(1, 1), Is.False);
    }

    [Test]
    public void LessThanOrEqual()
    {
        Assert.That(new VersionTuple(1) <= new VersionTuple(2), Is.True);
        Assert.That(new VersionTuple(1, 1) <= new VersionTuple(1, 1), Is.True);
        Assert.That(new VersionTuple(2) <= new VersionTuple(1), Is.False);
    }

    [Test]
    public void GreaterThanOrEqual()
    {
        Assert.That(new VersionTuple(2) >= new VersionTuple(1), Is.True);
        Assert.That(new VersionTuple(1, 1) >= new VersionTuple(1, 1), Is.True);
        Assert.That(new VersionTuple(1) >= new VersionTuple(2), Is.False);
    }

    [Test]
    public void ToString_MajorOnly()
    {
        Assert.That(new VersionTuple(1).ToString(), Is.EqualTo("1"));
    }

    [Test]
    public void ToString_MajorMinor()
    {
        Assert.That(new VersionTuple(1, 2).ToString(), Is.EqualTo("1.2"));
    }

    [Test]
    public void ToString_MajorMinorSubminor()
    {
        Assert.That(new VersionTuple(1, 2, 3).ToString(), Is.EqualTo("1.2.3"));
    }

    [Test]
    public void ToString_Full()
    {
        Assert.That(new VersionTuple(1, 2, 3, 4).ToString(), Is.EqualTo("1.2.3.4"));
    }

    [Test]
    public void GetHashCode_EqualObjectsSameHash()
    {
        var a = new VersionTuple(1, 2, 3, 4);
        var b = new VersionTuple(1, 2, 3, 4);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void Equals_Object_DifferentType()
    {
        var vt = new VersionTuple(1);
        Assert.That(vt.Equals("not a version"), Is.False);
        Assert.That(vt.Equals(null), Is.False);
    }
}
