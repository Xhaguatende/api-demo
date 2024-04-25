// -------------------------------------------------------------------------------------
//  <copyright file="ProductResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Responses.Product;

public class ProductResponse
{
    public ProductCategoryResponse Category { get; set; } = new();
    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}