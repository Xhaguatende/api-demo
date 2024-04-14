// -------------------------------------------------------------------------------------
//  <copyright file="GetProductDto.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos;

using Base;

public record GetProductDto : ProductBaseDto
{
    public ProductCategoryDto Category { get; set; } = default!;
}