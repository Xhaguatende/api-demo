// -------------------------------------------------------------------------------------
//  <copyright file="TestValueObject.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.UnitTests.Primitives.TestClasses;

using Domain.Primitives;

public class TestValueObject : ValueObject
{
    public TestValueObject(int property1, string property2)
    {
        Property1 = property1;
        Property2 = property2;
    }

    public int Property1 { get; }
    public string Property2 { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Property1;
        yield return Property2;
    }
}