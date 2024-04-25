// -------------------------------------------------------------------------------------
//  <copyright file="CategoryResponseBase.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Responses.Category;

public class CategoryResponse
{
    public string Description { get; set; } = default!;
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}