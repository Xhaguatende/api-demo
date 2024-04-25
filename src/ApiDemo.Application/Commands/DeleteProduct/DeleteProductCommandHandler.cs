// -------------------------------------------------------------------------------------
//  <copyright file="DeleteProductCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.DeleteProduct;

using Domain.Products.Errors;
using Domain.Shared;
using MediatR;
using Repositories;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result<DeleteProductResponse>.Failure([ProductErrors.ProductNotFound(request.Id)]);
        }

        var result = await _productRepository.DeleteOneAsync(request.Id, cancellationToken);

        return new DeleteProductResponse(result, request.Id);
    }
}