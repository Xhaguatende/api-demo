// -------------------------------------------------------------------------------------
//  <copyright file="GetProductByIdQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductById;

using Domain.Products.Aggregate;
using Domain.Products.Exceptions;
using MediatR;
using Repositories;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductAggregate>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductAggregate> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetOneAggregateByExpressionAsync(x => x.Id == request.Id, cancellationToken);

        return product ?? throw new ProductNotFoundException(request.Id);
    }
}