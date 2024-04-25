// -------------------------------------------------------------------------------------
//  <copyright file="DeleteProductResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteProduct;

public record DeleteProductResponse(bool Success, Guid Id);