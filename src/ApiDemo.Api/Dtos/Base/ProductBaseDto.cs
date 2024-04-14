// -------------------------------------------------------------------------------------
//  <copyright file="ProductBaseDto.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos.Base;

public abstract record ProductBaseDto
{
    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}