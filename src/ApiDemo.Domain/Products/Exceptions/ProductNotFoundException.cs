// -------------------------------------------------------------------------------------
//  <copyright file="ProductNotFoundException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid id)
        : base($"Product with id {id} was not found.")
    {
    }
}