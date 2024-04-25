// -------------------------------------------------------------------------------------
//  <copyright file="ProductCategoryResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Responses.Product;

public class ProductCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}