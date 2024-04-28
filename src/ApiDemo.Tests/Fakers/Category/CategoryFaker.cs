// -------------------------------------------------------------------------------------
//  <copyright file="CategoryFaker.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Fakers.Category;

using Bogus;
using Domain.Categories.Entity;
using Extensions;

public sealed class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        this.SkipConstructor();

        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Commerce.Categories(1).First());
        RuleFor(x => x.Description, f => f.Commerce.ProductAdjective());
    }
}