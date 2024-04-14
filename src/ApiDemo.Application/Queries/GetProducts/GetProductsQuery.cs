// -------------------------------------------------------------------------------------
//  <copyright file="GetProductsQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProducts;

using Domain.Products.Entity;
using MediatR;

public record GetProductsQuery : IRequest<List<Product>>;