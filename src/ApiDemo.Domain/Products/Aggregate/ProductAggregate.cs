// -------------------------------------------------------------------------------------
//  <copyright file="ProductAggregate.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Aggregate;

using Categories.Entity;
using Entity;

public class ProductAggregate : Product
{
    public ProductAggregate(Guid id) : base(id)
    {
    }

    public Category Category { get; set; } = default!;
}