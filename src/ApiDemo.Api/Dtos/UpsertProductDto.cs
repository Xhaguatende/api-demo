// -------------------------------------------------------------------------------------
//  <copyright file="UpsertProductDto.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos;

using Base;

public record UpsertProductDto : ProductBaseDto
{
    public Guid CategoryId { get; set; }
    public new Guid? Id { get; set; }
}