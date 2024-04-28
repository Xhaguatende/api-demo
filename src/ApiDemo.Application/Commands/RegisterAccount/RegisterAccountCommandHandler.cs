// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

using Domain.Accounts.Entity;
using Domain.Accounts.Errors;
using Domain.Accounts.ValueObjects;
using Domain.Shared;
using MediatR;
using Repositories;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, Result<RegisterAccountResponse>>
{
    private readonly IAccountRepository _accountRepository;

    public RegisterAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<RegisterAccountResponse>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _accountRepository.GetByIdAsync(new AccountId(request.Email), cancellationToken);

        if (existingAccount is not null)
        {
            return Result<RegisterAccountResponse>.Failure([AccountErrors.AccountAlreadyExists(request.Email)]);
        }

        var account = new Account(
            new AccountId(request.Email),
            request.Password);

        await _accountRepository.RegisterAccountAsync(account, cancellationToken);

        return new RegisterAccountResponse(request.Email);
    }
}