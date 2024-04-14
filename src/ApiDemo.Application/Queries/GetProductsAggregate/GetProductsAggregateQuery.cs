// -------------------------------------------------------------------------------------
//  <copyright file="GetProductsAggregateQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductsAggregate;

using Domain.Products.Aggregate;
using MediatR;

public record GetProductsAggregateQuery : IRequest<List<ProductAggregate>>;