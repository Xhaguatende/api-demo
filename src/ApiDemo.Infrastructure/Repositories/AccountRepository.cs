// -------------------------------------------------------------------------------------
//  <copyright file="AccountRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using Application.Repositories;
using Base;
using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;
using MongoDB.Driver;

public class AccountRepository : MongoDbRepository<Account, AccountId, Account>, IAccountRepository
{
    public AccountRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override string CollectionName => "accounts";

    public async Task RegisterAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        await UpsertOneAsync(account, cancellationToken);
    }
}