// -------------------------------------------------------------------------------------
//  <copyright file="ProductNotFoundException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Exceptions;

using ApiDemo.Domain.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id)
        : base($"Product with id '{id}' was not found.")
    {
    }
}