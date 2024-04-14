// -------------------------------------------------------------------------------------
//  <copyright file="ProductCategoryDto.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos;

public record ProductCategoryDto
{
    public string Name { get; set; } = default!;
    public Guid Id { get; set; }
}