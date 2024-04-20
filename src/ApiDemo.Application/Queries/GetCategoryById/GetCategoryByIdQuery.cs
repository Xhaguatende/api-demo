// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoryByIdQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategoryById;

using Domain.Categories.Entity;
using MediatR;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Category>;