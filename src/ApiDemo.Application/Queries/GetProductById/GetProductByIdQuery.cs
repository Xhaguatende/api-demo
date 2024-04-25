// -------------------------------------------------------------------------------------
//  <copyright file="GetProductByIdQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductById;

using MediatR;
using Responses.Product;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse>;