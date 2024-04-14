// -------------------------------------------------------------------------------------
//  <copyright file="DeleteProductCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteProduct;

using Domain.Products.Exceptions;
using MediatR;
using Repositories;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        return await _productRepository.DeleteOneAsync(request.Id, cancellationToken);
    }
}