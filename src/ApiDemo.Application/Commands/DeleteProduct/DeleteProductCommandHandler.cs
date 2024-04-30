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

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductCommandResponse>>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<DeleteProductCommandResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result<DeleteProductCommandResponse>.Failure([ProductErrors.ProductNotFound(request.Id)]);
        }

        await _productRepository.DeleteOneAsync(request.Id, cancellationToken);

        return new DeleteProductCommandResponse(request.Id);
    }
}