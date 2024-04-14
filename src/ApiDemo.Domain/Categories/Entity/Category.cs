// -------------------------------------------------------------------------------------
//  <copyright file="Category.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Categories.Entity;

using Primitives;

public class Category : Entity<Guid>
{
    public Category(Guid id) : base(id)
    {
    }

    public string Description { get; set; } = default!;

    public string Name { get; set; } = default!;
}