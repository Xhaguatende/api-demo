// -------------------------------------------------------------------------------------
//  <copyright file="ProductErrors.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Products.Errors;

using Shared;

public static class ProductErrors
{
    public static Error ProductNotFound(Guid id, string property = "")
    {
        var message = $"The product with id: '{id}' was not found.";

        const string code = $"{nameof(ProductErrors)}.{nameof(ProductNotFound)}";

        return new Error(code, message, property);
    }
}