// -------------------------------------------------------------------------------------
//  <copyright file="GetProductByIdQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductById;

using Domain.Products.Aggregate;
using MediatR;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductAggregate>;