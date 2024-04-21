// -------------------------------------------------------------------------------------
//  <copyright file="AccountRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Repositories;

using Application.Repositories;
using Base;
using Domain.Accounts.Entity;
using MongoDB.Driver;

public class AccountRepository : MongoDbRepository<Account, Guid, Account>, IAccountRepository
{
    public AccountRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    protected override string CollectionName => "accounts";

    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Account>.Filter.Eq(x => x.Email, email);
        return await Collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RegisterAccountAsync(Account account, CancellationToken cancellationToken = default)
    {
        await UpsertOneAsync(account, cancellationToken);

        return true;
    }
}