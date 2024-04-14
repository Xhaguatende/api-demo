// -------------------------------------------------------------------------------------
//  <copyright file="ProductRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using Application.Repositories;
using Base;
using Domain.Products.Aggregate;
using Domain.Products.Entity;
using MongoDB.Driver;

public class ProductRepository : MongoDbRepository<Product, Guid, ProductAggregate>, IProductRepository
{
    public ProductRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override string CollectionName => "products";
}