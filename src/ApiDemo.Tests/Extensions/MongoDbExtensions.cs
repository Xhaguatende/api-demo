// -------------------------------------------------------------------------------------
//  <copyright file="MongoDbExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Extensions;

using Domain.Products.Entity;
using MongoDB.Bson;
using MongoDB.Driver;

public static class MongoDbExtensions
{
    public static async Task CreateProductsAggregateViewAsync(this IMongoDatabase mongoDatabase)
    {
        var pipeline = new List<BsonDocument>
        {
            new(
                "$lookup",
                new BsonDocument
                {
                    { "from", "categories" },
                    { "localField", "categoryId" },
                    { "foreignField", "id" },
                    { "as", "categoryDetails" }
                }),
            new(
                "$unwind",
                new BsonDocument
                {
                    { "path", "$categoryDetails" },
                    { "preserveNullAndEmptyArrays", true }
                }),
            new(
                "$addFields",
                new BsonDocument
                {
                    { "category.id", "$categoryDetails.id" },
                    { "category.name", "$categoryDetails.name" },
                    { "category.description", "$categoryDetails.description" }
                }),
            new(
                "$project",
                new BsonDocument
                {
                    { "categoryDetails", 0 }
                })
        };

        var options = new CreateViewOptions<Product>
        {
            Collation = new Collation("en")
        };

        await mongoDatabase.CreateViewAsync<Product, Product>("productsAggregate", "products", pipeline, options);
    }

    public static async Task DropCollectionsAsync(this IMongoDatabase mongoDatabase, params string[] collections)
    {
        foreach (var collection in collections)
        {
            await mongoDatabase.DropCollectionAsync(collection);
        }
    }
}