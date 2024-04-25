// -------------------------------------------------------------------------------------
//  <copyright file="GetCategoriesQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetCategories;

using MediatR;
using Repositories;
using Responses.Category;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetManyByExpressionAsync(x => true, cancellationToken);

        return categories.Select(
            category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            }).ToList();
    }
}