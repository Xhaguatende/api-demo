// -------------------------------------------------------------------------------------
//  <copyright file="DeleteProductCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteProduct;

using Domain.Shared;
using MediatR;

public record DeleteProductCommand(Guid Id) : IRequest<Result<DeleteProductResponse>>;