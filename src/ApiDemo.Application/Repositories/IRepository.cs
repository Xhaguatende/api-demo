// -------------------------------------------------------------------------------------
//  <copyright file="IRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using System.Linq.Expressions;

public interface IRepository<T, in TKey, TAggregate>
{
    Task DeleteOneAsync(TKey id, CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken);

    Task<List<TAggregate>> GetManyAggregatesByExpressionAsync(
                Expression<Func<TAggregate, bool>> filter,
        CancellationToken cancellationToken);

    Task<List<T>> GetManyByExpressionAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken);

    Task<TAggregate?> GetOneAggregateByExpressionAsync(
        Expression<Func<TAggregate, bool>> filter,
        CancellationToken cancellationToken);

    Task<T?> GetOneByExpressionAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken);

    Task UpsertOneAsync(T document, CancellationToken cancellationToken);
}