// -------------------------------------------------------------------------------------
//  <copyright file="ProductFaker.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Fakers.Product;

using Bogus;
using Domain.Products.Entity;
using Extensions;

public sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        this.SkipConstructor();

        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Commerce.ProductName());
        RuleFor(x => x.Description, f => f.Lorem.Sentence());
        RuleFor(x => x.Price, f => f.Random.Decimal(1, 100));
        RuleFor(x => x.Stock, f => f.Random.Int(1, 1000));
        RuleFor(x => x.CategoryId, f => f.Random.Guid());
    }
}