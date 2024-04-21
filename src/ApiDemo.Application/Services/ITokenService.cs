// -------------------------------------------------------------------------------------
//  <copyright file="ITokenService.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Services;

using Domain.Accounts.Entity;

public interface ITokenService
{
    Tuple<string, int> GenerateAccessToken(Account account);

    Tuple<string, int> GenerateRefreshToken();
}