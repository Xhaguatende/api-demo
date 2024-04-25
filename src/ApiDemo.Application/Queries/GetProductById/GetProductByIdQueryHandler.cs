// -------------------------------------------------------------------------------------
//  <copyright file="GetProductByIdQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductById;

using Domain.Products.Exceptions;
using MediatR;
using Repositories;
using Responses.Product;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetOneAggregateByExpressionAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        return new ProductResponse
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
        };
    }
}