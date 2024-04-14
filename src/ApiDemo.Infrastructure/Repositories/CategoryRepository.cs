// -------------------------------------------------------------------------------------
//  <copyright file="CategoryRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using Application.Repositories;
using Base;
using Domain.Categories.Entity;
using MongoDB.Driver;

public class CategoryRepository : MongoDbRepository<Category, Guid, Category>, ICategoryRepository
{
    public CategoryRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override string CollectionName => "categories";
}