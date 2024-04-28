// -------------------------------------------------------------------------------------
//  <copyright file="UpsertProductCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertProduct;

using Domain.Products.Entity;
using Domain.Products.Errors;
using Domain.Shared;
using MediatR;
using Repositories;
using Responses.Product;

public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public UpsertProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
    {
        Product product;

        if (request.Id.HasValue && request.Id.Value != Guid.Empty)
        {
            var existingProduct = await _productRepository.GetOneByExpressionAsync(
                x => x.Id == request.Id,
                cancellationToken);

            if (existingProduct is null)
            {
                return Result<ProductResponse>.Failure([ProductErrors.ProductNotFound(request.Id.Value)]);
            }

            existingProduct.UpdateName(request.Name);
            existingProduct.UpdateDescription(request.Description);
            existingProduct.UpdatePrice(request.Price);
            existingProduct.UpdateCategory(request.CategoryId);
            existingProduct.UpdateStock(request.Stock);

            product = existingProduct;
        }
        else
        {
            product = new Product(request.Name, request.Description, request.CategoryId, request.Price, request.Stock);
        }

        await _productRepository.UpsertOneAsync(product, cancellationToken);

        var savedProduct =
            await _productRepository.GetOneAggregateByExpressionAsync(x => x.Id == product.Id, cancellationToken);

        return new ProductResponse
        {
            Id = savedProduct!.Id,
            Name = savedProduct.Name,
            Description = savedProduct.Description,
            Price = savedProduct.Price,
            Stock = savedProduct.Stock,
            Category = new ProductCategoryResponse
            {
                Id = savedProduct.CategoryId,
                Name = savedProduct.Category.Name
            }
        };
    }
}