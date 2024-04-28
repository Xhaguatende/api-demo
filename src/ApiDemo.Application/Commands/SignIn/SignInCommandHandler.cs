// -------------------------------------------------------------------------------------
//  <copyright file="SignInCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.SignIn;

using Domain.Accounts.Errors;
using Domain.Accounts.ValueObjects;
using Domain.Shared;
using MediatR;
using Repositories;
using Services;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<SignInCommandResponse>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;

    public SignInCommandHandler(IAccountRepository accountRepository, ITokenService tokenService)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInCommandResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(new AccountId(request.Email), cancellationToken);

        if (account is null)
        {
            // TODO: Log this event. So we know that someone tried to sign in with an email that doesn't exist
            return Result<SignInCommandResponse>.Failure([AccountErrors.InvalidCredentials()]);
        }

        if (!account.VerifyPassword(request.Password))
        {
            return Result<SignInCommandResponse>.Failure([AccountErrors.InvalidCredentials()]);
        }

        var accessTokenTuple = _tokenService.GenerateAccessToken(account);

        var refreshTokenTuple = _tokenService.GenerateRefreshToken();

        account.UpdateRefreshToken(refreshTokenTuple.Item1, refreshTokenTuple.Item2);

        await _accountRepository.UpsertOneAsync(account, cancellationToken);

        return new SignInCommandResponse
        {
            AccessToken = accessTokenTuple.Item1,
            ExpiresIn = accessTokenTuple.Item2,
            RefreshToken = refreshTokenTuple.Item1,
            RefreshTokenExpiresIn = refreshTokenTuple.Item2
        };
    }
}