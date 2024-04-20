// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoryDto.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos.Category;

public record GetCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}