// -------------------------------------------------------------------------------------
//  <copyright file="FakerExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Extensions;

using System.Runtime.CompilerServices;
using Bogus;

public static class FakerExtensions
{
    public static Faker<T> SkipConstructor<T>(this Faker<T> faker) where T : class
    {
        return faker.CustomInstantiator(_ => (RuntimeHelpers.GetUninitializedObject(typeof(T)) as T)!);
    }
}