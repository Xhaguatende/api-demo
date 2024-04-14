// -------------------------------------------------------------------------------------
//  <copyright file="Product.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Entity;

using Primitives;

public class Product : Entity<Guid>
{
    public Product(Guid id) : base(id)
    {
    }

    public Guid CategoryId { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}