// -------------------------------------------------------------------------------------
//  <copyright file="IAccountRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;

public interface IAccountRepository : IRepository<Account, AccountId, Account>
{
    Task RegisterAccountAsync(Account account, CancellationToken cancellationToken = default);
}