// -------------------------------------------------------------------------------------
//  <copyright file="TokenServicesTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.IntegrationTests.Services;

using System.Security.Claims;
using System.Text;
using ApiDemo.Infrastructure.Services;
using Bogus;
using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Settings;

public class TokenServicesTests
{
    [Fact]
    public async Task GenerateAccessToken_Should_ReturnTokenWithClaims()
    {
        // Arrange
        var faker = new Faker();

        var mockAuthSettings = Substitute.For<IOptions<AuthSettings>>();

        var secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(faker.Internet.Password(30)));

        mockAuthSettings.Value.Returns(new AuthSettings
        {
            Secret = secret,
            Issuer = faker.Internet.DomainName(),
            Audience = faker.Internet.DomainName(),
            AccessTokenExpirationInMinutes = faker.Random.Int(30, 60)
        });

        var email = faker.Internet.Email();
        var password = faker.Internet.Password();

        var account = new Account(new AccountId(email), password);
        var service = new TokenService(mockAuthSettings);

        // Act
        var (token, expiration) = service.GenerateAccessToken(account);

        // Assert
        var handler = new JsonWebTokenHandler();

        var principal = await handler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mockAuthSettings.Value.Secret)),
            ValidIssuer = mockAuthSettings.Value.Issuer,
            ValidAudience = mockAuthSettings.Value.Audience
        });

        var claims = principal.Claims;

        Assert.NotNull(token);
        Assert.Equal(mockAuthSettings.Value.AccessTokenExpirationInMinutes * 60, expiration);

        claims["sub"].Should().Be("user_id");
        claims["aud"].Should().Be(mockAuthSettings.Value.Audience);
        claims["iss"].Should().Be(mockAuthSettings.Value.Issuer);
        claims[ClaimTypes.NameIdentifier].Should().Be(account.Id.Email);
        claims[ClaimTypes.Email].Should().Be(account.Id.Email);
    }

    [Fact]
    public void GenerateRefreshToken_Should_ReturnBase64EncodedString()
    {
        // Arrange
        var mockAuthSettings = Substitute.For<IOptions<AuthSettings>>();

        var faker = new Faker();

        mockAuthSettings.Value.Returns(new AuthSettings
        {
            RefreshTokenExpirationInMinutes = faker.Random.Int(360, 3600)
        });

        var service = new TokenService(mockAuthSettings);

        // Act
        var (refreshToken, expiration) = service.GenerateRefreshToken();

        // Assert
        Assert.NotNull(refreshToken);
        Assert.Equal(32, Convert.FromBase64String(refreshToken).Length);
        Assert.Equal(mockAuthSettings.Value.RefreshTokenExpirationInMinutes * 60, expiration);
    }
}