// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoryByIdQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategoryById;

using Domain.Categories.Exceptions;
using MediatR;
using Repositories;
using Responses.Category;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            throw new CategoryNotFoundException(request.Id);
        }

        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return response;
    }
}