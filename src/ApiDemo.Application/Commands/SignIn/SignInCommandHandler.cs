// -------------------------------------------------------------------------------------
//  <copyright file="SignInCommandHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.SignIn;

using MediatR;
using Repositories;
using Services;

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInCommandResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;

    public SignInCommandHandler(IAccountRepository accountRepository, ITokenService tokenService)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }

    public async Task<SignInCommandResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (account is null)
        {
            return new SignInCommandResponse
            {
                Message = "Invalid email or password."
            };
        }

        if (!account.VerifyPassword(request.Password))
        {
            return new SignInCommandResponse
            {
                Message = "Invalid email or password."
            };
        }

        var accessTokenTuple = _tokenService.GenerateAccessToken(account);

        var refreshTokenTuple = _tokenService.GenerateRefreshToken();

        account.UpdateRefreshToken(refreshTokenTuple.Item1, refreshTokenTuple.Item2);

        await _accountRepository.UpsertOneAsync(account, cancellationToken);

        return new SignInCommandResponse
        {
            Success = true,
            AccessToken = accessTokenTuple.Item1,
            ExpiresIn = accessTokenTuple.Item2,
            RefreshToken = refreshTokenTuple.Item1
        };
    }
}