// -------------------------------------------------------------------------------------
//  <copyright file="Product.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Entity;

using Primitives;

public class Product : Entity<Guid>
{
    public Product(
        string name,
        string description,
        Guid categoryId,
        decimal price,
        int stock) : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        Stock = stock;
    }

    protected Product(Guid id) : base(id)
    {
        Description = string.Empty;
        Name = string.Empty;
    }

    public Guid CategoryId { get; private set; }

    public string Description { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public void UpdateCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public void UpdateStock(int stock)
    {
        Stock = stock;
    }
}