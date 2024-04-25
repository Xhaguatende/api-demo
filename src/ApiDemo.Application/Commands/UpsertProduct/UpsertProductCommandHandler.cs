// -------------------------------------------------------------------------------------
//  <copyright file="UpsertProductCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertProduct;

using Domain.Products.Entity;
using Domain.Products.Exceptions;
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
                cancellationToken) ?? throw new ProductNotFoundException(request.Id.Value);

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.Stock = request.Stock;

            product = existingProduct;
        }
        else
        {
            product = new Product(Guid.NewGuid())
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = request.Price,
                Stock = request.Stock
            };
        }

        var result = await _productRepository.UpsertOneAsync(product, cancellationToken);

        return new ProductResponse
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Price = result.Price,
            Stock = result.Stock,
            Category = new ProductCategoryResponse
            {
                Id = request.CategoryId,
                Name = string.Empty
            }
        };
    }
}