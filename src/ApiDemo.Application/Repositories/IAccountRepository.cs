// -------------------------------------------------------------------------------------
//  <copyright file="IAccountRepository.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Repositories;

using Domain.Accounts.Entity;

public interface IAccountRepository : IRepository<Account, Guid, Account>
{
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<bool> RegisterAccountAsync(Account account, CancellationToken cancellationToken = default);
}