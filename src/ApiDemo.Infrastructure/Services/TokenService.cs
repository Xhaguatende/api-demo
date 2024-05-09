// -------------------------------------------------------------------------------------
//  <copyright file="TokenService.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Services;

using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Services;
using Domain.Accounts.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Settings;

public class TokenService : ITokenService
{
    private readonly AuthSettings _authSettings;

    public TokenService(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
    }

    public Tuple<string, int> GenerateAccessToken(Account account)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JsonWebTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _authSettings.Issuer,
            Audience = _authSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_authSettings.AccessTokenExpirationInMinutes),
            SigningCredentials = signingCredentials,
            Claims = new Dictionary<string, object>
            {
                { "sub", "user_id" },
                { ClaimTypes.Name, account.Id.Email },
                { ClaimTypes.NameIdentifier, account.Id.Email },
                { ClaimTypes.Email, account.Id.Email }
            }
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new Tuple<string, int>(token, _authSettings.AccessTokenExpirationInMinutes * 60);
    }

    public Tuple<string, int> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new Tuple<string, int>(
            Convert.ToBase64String(randomNumber),
            _authSettings.RefreshTokenExpirationInMinutes * 60);
    }
}