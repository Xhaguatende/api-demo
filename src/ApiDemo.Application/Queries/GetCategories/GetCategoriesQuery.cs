// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoriesQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategories;

using Domain.Categories.Entity;
using MediatR;

public record GetCategoriesQuery : IRequest<List<Category>>;