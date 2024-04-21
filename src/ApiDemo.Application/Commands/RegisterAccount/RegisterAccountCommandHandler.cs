// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

using Domain.Accounts.Entity;
using Domain.Accounts.Exceptions;
using MediatR;
using Repositories;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, bool>
{
    private readonly IAccountRepository _accountRepository;

    public RegisterAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<bool> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingAccount is not null)
        {
            throw new AccountAlreadyExistException(request.Email);
        }

        var account = new Account(
            request.Email,
            request.Password);

        return await _accountRepository.RegisterAccountAsync(account, cancellationToken);
    }
}