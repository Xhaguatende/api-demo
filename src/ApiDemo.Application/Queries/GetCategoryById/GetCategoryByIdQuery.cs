// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoryByIdQuery.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategoryById;

using MediatR;
using Responses.Category;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryResponse>;