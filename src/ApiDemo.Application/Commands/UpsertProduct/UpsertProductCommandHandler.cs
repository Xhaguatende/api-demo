// -------------------------------------------------------------------------------------
//  <copyright file="UpsertProductCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.UpsertProduct;

using Domain.Products.Entity;
using Domain.Products.Exceptions;
using MediatR;
using Repositories;

public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, Product>
{
    private readonly IProductRepository _productRepository;

    public UpsertProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
    {
        Product product;

        if (request.Id.HasValue)
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

        return result;
    }
}