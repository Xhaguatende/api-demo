// -------------------------------------------------------------------------------------
//  <copyright file="GetProductsQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProducts;

using MediatR;
using Repositories;
using Responses.Product;

internal class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetManyAggregatesByExpressionAsync(x => true, cancellationToken);

        return products.Select(product => new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = new ProductCategoryResponse
            {
                Id = product.Category.Id,
                Name = product.Category.Name
            }
        }).ToList();
    }
}