// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoriesQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategories;

using MediatR;
using Responses.Category;

public record GetCategoriesQuery : IRequest<List<CategoryResponse>>;