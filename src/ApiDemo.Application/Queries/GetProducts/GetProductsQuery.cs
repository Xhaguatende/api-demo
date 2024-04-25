// -------------------------------------------------------------------------------------
//  <copyright file="GetProductsQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProducts;

using MediatR;
using Responses.Product;

public record GetProductsQuery : IRequest<List<ProductResponse>>;