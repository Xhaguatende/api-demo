// -------------------------------------------------------------------------------------
//  <copyright file="GetProductsAggregateQueryHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Queries.GetProductsAggregate;

using Domain.Products.Aggregate;
using MediatR;
using Repositories;

public class GetProductsAggregateQueryHandler : IRequestHandler<GetProductsAggregateQuery, List<ProductAggregate>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsAggregateQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductAggregate>> Handle(GetProductsAggregateQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetManyAggregatesByExpressionAsync(x => true, cancellationToken);
    }
}