// -------------------------------------------------------------------------------------
//  <copyright file="UpsertProductCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertProduct;

using Domain.Products.Entity;
using MediatR;

public record UpsertProductCommand : IRequest<Product>
{
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = default!;
    public Guid? Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}