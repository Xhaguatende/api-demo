// -------------------------------------------------------------------------------------
//  <copyright file="ValueObjectTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.UnitTests.Primitives;

using TestClasses;

public class ValueObjectTests
{
    [Fact]
    public void EqualityOperator_WithEqualObjects_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "value");
        var obj2 = new TestValueObject(1, "value");

        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        var obj1 = new TestValueObject(1, "value1");
        var obj2 = new TestValueObject(2, "value2");

        Assert.False(obj1.Equals(obj2));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var obj1 = new TestValueObject(1, "value");

        Assert.False(obj1.Equals(null));
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "value");
        var obj2 = new TestValueObject(1, "value");

        Assert.True(obj1.Equals(obj2));
    }

    [Fact]
    public void GetHashCode_WithSameValues_IsEqual()
    {
        var obj1 = new TestValueObject(1, "value");
        var obj2 = new TestValueObject(1, "value");

        Assert.Equal(obj1.GetHashCode(), obj2.GetHashCode());
    }

    [Fact]
    public void InequalityOperator_WithDifferentObjects_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "value");
        var obj2 = new TestValueObject(2, "other");

        Assert.True(obj1 != obj2);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        var obj = new TestValueObject(1, "value");
        const string expected = "TestValueObject [1, value]";

        Assert.Equal(expected, obj.ToString());
    }
}