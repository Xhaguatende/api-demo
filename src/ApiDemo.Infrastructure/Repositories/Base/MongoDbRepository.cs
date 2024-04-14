// -------------------------------------------------------------------------------------
//  <copyright file="MongoDbRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories.Base;

using System.Linq.Expressions;
using Application.Repositories;
using Domain.Primitives;
using Extensions;
using MongoDB.Driver;

public abstract class MongoDbRepository<T, TKey, TAggregate> : IRepository<T, TKey, TAggregate>
    where T : Entity<TKey> where TKey : notnull
{
    protected MongoDbRepository(IMongoDatabase mongoDatabase)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Collection = mongoDatabase.GetCollection<T>(CollectionName);
        // ReSharper disable once VirtualMemberCallInConstructor
        AggregateCollection = mongoDatabase.GetCollection<TAggregate>($"{CollectionName}Aggregate");
    }

    protected IMongoCollection<TAggregate> AggregateCollection { get; set; }
    protected IMongoCollection<T> Collection { get; set; }
    protected abstract string CollectionName { get; }

    public async Task<bool> DeleteOneAsync(TKey id, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(t => t.Id, id);

        var result = await Collection.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount > 0;
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(t => t.Id, id);

        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TAggregate>> GetManyAggregatesByExpressionAsync(
            Expression<Func<TAggregate, bool>> filter,
        CancellationToken cancellationToken)
    {
        var filterDefinition = BuildFilterDefinitionAggregate(filter);

        return await AggregateCollection.Find(filterDefinition).ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetManyByExpressionAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken)
    {
        var filterDefinition = BuildFilterDefinition(filter);

        return await Collection.Find(filterDefinition).ToListAsync(cancellationToken);
    }

    public async Task<TAggregate?> GetOneAggregateByExpressionAsync(
        Expression<Func<TAggregate, bool>> filter,
        CancellationToken cancellationToken)
    {
        var filterDefinition = BuildFilterDefinitionAggregate(filter);

        return await AggregateCollection.Find(filterDefinition).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetOneByExpressionAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken)
    {
        var filterDefinition = BuildFilterDefinition(filter);

        return await Collection.Find(filterDefinition).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T> UpsertOneAsync(T document, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(t => t.Id, document.Id);

        var excludeProperties = new[]
        {
            nameof(document.CreatedAt),
            nameof(document.CreatedBy),
            nameof(document.UpdatedAt),
            nameof(document.UpdatedBy),
            nameof(document.Version)
        };

        const string identity = "System"; // TODO: Get identity from context

        var update = Builders<T>.Update
            .SetAll(document, excludeProperties)
            .SetOnInsert(t => t.CreatedAt, DateTime.UtcNow)
            .SetOnInsert(t => t.CreatedBy, identity)
            .Set(t => t.UpdatedAt, DateTime.UtcNow)
            .Set(t => t.UpdatedBy, identity)
            .Inc(t => t.Version, 1);

        var updateOptions = new UpdateOptions { IsUpsert = true };

        await Collection.UpdateOneAsync(filter, update, updateOptions, cancellationToken);

        return document;
    }

    protected virtual FilterDefinition<T> BuildFilterDefinition(Expression<Func<T, bool>>? filter)
    {
        var filterDefinition = filter == null
            ? Builders<T>.Filter.Empty
            : Builders<T>.Filter.Where(filter);

        return filterDefinition;
    }

    protected virtual FilterDefinition<TAggregate> BuildFilterDefinitionAggregate(
        Expression<Func<TAggregate, bool>>? filter)
    {
        var filterDefinition = filter == null
            ? Builders<TAggregate>.Filter.Empty
            : Builders<TAggregate>.Filter.Where(filter);

        return filterDefinition;
    }
}