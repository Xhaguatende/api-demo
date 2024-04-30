// -------------------------------------------------------------------------------------
//  <copyright file="AccountsControllerTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using ApiDemo.Tests.Extensions;
using ApiDemo.Tests.Infrastructure;
using Application.Commands.RegisterAccount;
using Application.Commands.SignIn;
using Bogus;
using Domain.Accounts.Entity;
using Domain.Accounts.Errors;
using Domain.Accounts.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

public class AccountsControllerTests :
    IClassFixture<DbFixture>,
    IClassFixture<CustomWebApplicationFactory<IAssemblyReference>>,
    IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<IAssemblyReference> _factory;
    private readonly DbFixture _fixture;

    public AccountsControllerTests(DbFixture fixture, CustomWebApplicationFactory<IAssemblyReference> factory)
    {
        _fixture = fixture;
        _factory = factory;

        _factory.SetDatabaseInfo(_fixture.ConnectionString, _fixture.DatabaseName);
    }

    public async Task DisposeAsync()
    {
        await _fixture.MongoDatabase.DropCollectionsAsync("accounts");
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task RegisterAsync_Should_ReturnBadRequest_When_AccountExists()
    {
        // Arrange
        var faker = new Faker();
        var password = $"{faker.Internet.Password(12)}P!1";

        var command = new RegisterAccountCommand
        {
            Email = faker.Internet.Email(),
            Password = password,
            ConfirmPassword = password,
        };

        var account = new Account(
            new AccountId(command.Email),
            command.Password);

        await _fixture.MongoDatabase.GetCollection<Account>("accounts").InsertOneAsync(account);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts/register", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        problemDetails.Detail.Should().Contain("One or more validation errors occurred.");
        problemDetails.Title.Should().Contain("One or more validation errors occurred.");
        problemDetails.Extensions.Should().ContainKey("errors");

        var errors = problemDetails.GetErrors();

        Assert.NotNull(errors);
        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be(AccountErrors.AccountAlreadyExists(command.Email).Code);
        errors[0].Message.Should().Be(AccountErrors.AccountAlreadyExists(command.Email).Message);
        errors[0].Property.Should().Be(AccountErrors.AccountAlreadyExists(command.Email).Property);
    }

    [Fact]
    public async Task RegisterAsync_Should_ReturnRegisteredEmail_When_RegisteredSuccessfully()
    {
        // Arrange
        var faker = new Faker();
        var password = $"{faker.Internet.Password(12)}P!1";

        var command = new RegisterAccountCommand
        {
            Email = faker.Internet.Email(),
            Password = password,
            ConfirmPassword = password,
        };

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts/register", command);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<RegisterAccountCommandResponse>();

        Assert.NotNull(content);
        Assert.Equal(command.Email, content.Email);
    }

    [Fact]
    public async Task SignInAsync_Should_ReturnBadRequest_When_AccountDoesNotExist()
    {
        // Arrange
        var faker = new Faker();
        var email = faker.Internet.Email();
        var password = faker.Internet.Password(12);

        var command = new SignInCommand(email, password);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts/sign-in", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        problemDetails.Detail.Should().Contain("One or more validation errors occurred.");
        problemDetails.Title.Should().Contain("One or more validation errors occurred.");
        problemDetails.Extensions.Should().ContainKey("errors");

        var errors = problemDetails.GetErrors();

        Assert.NotNull(errors);
        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be(AccountErrors.InvalidCredentials().Code);
        errors[0].Message.Should().Be(AccountErrors.InvalidCredentials().Message);
        errors[0].Property.Should().Be(AccountErrors.InvalidCredentials().Property);
    }

    [Fact]
    public async Task SignInAsync_Should_ReturnBadRequest_When_InvalidCredentials()
    {
        // Arrange
        var faker = new Faker();
        var email = faker.Internet.Email();
        var password = faker.Internet.Password(12);

        var account = new Account(new AccountId(email), password);

        await _fixture.MongoDatabase.GetCollection<Account>("accounts").InsertOneAsync(account);

        var command = new SignInCommand(email, faker.Internet.Password(12));

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts/sign-in", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        problemDetails.Detail.Should().Contain("One or more validation errors occurred.");
        problemDetails.Title.Should().Contain("One or more validation errors occurred.");
        problemDetails.Extensions.Should().ContainKey("errors");

        var errors = problemDetails.GetErrors();

        Assert.NotNull(errors);
        errors.Should().HaveCount(1);
        errors[0].Code.Should().Be(AccountErrors.InvalidCredentials().Code);
        errors[0].Message.Should().Be(AccountErrors.InvalidCredentials().Message);
        errors[0].Property.Should().Be(AccountErrors.InvalidCredentials().Property);
    }

    [Fact]
    public async Task SignInAsync_Should_ReturnTokenResponse_When_CredentialsAreValid()
    {
        // Arrange
        var faker = new Faker();
        var email = faker.Internet.Email();
        var password = faker.Internet.Password(12);

        var account = new Account(new AccountId(email), password);

        await _fixture.MongoDatabase.GetCollection<Account>("accounts").InsertOneAsync(account);

        var command = new SignInCommand(email, password);

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/accounts/sign-in", command);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<SignInCommandResponse>();

        Assert.NotNull(content);
        content.AccessToken.Should().NotBeNullOrEmpty();
        content.RefreshToken.Should().NotBeNullOrEmpty();
        content.ExpiresIn.Should().BeGreaterThan(0);
        content.RefreshTokenExpiresIn.Should().BeGreaterThan(0);
        content.TokenType.Should().Be("Bearer");
    }
}