// -------------------------------------------------------------------------------------
//  <copyright file="AccountRepositoryTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.IntegrationTests.Repositories;

using Bogus;
using Domain.Accounts.Entity;
using Domain.Accounts.ValueObjects;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.Infrastructure;

public class AccountRepositoryTests : IClassFixture<DbFixture>
{
    private readonly AccountRepository _sut;

    public AccountRepositoryTests(DbFixture fixture)
    {
        _sut = new AccountRepository(fixture.MongoDatabase);
    }

    [Fact]
    public async Task RegisterAccountAsync_Should_CreateAccount()
    {
        // Arrange
        var faker = new Faker();
        var email = faker.Internet.Email();
        var password = faker.Internet.Password();

        // Act
        var account = new Account(new AccountId(email), password);

        await _sut.RegisterAccountAsync(account);

        // Assert
        var createdAccount = await _sut.GetByIdAsync(account.Id, CancellationToken.None);
        createdAccount.Should().NotBeNull();

        createdAccount.Should().BeEquivalentTo(account);
    }
}