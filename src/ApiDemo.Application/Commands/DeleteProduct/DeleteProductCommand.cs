// -------------------------------------------------------------------------------------
//  <copyright file="DeleteProductCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteProduct;

using MediatR;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;