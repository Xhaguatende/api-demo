// -------------------------------------------------------------------------------------
//  <copyright file="Category.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Categories.Entity;

using Primitives;

public class Category : Entity<Guid>
{
    public Category(Guid id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Description { get; private set; }

    public string Name { get; private set; }
}