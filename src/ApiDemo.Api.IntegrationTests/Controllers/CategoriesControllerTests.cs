// -------------------------------------------------------------------------------------
//  <copyright file="CategoriesControllerTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using ApiDemo.Tests.Infrastructure;
using Application.Responses.Category;
using Domain.Categories.Entity;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Tests.Extensions;
using Tests.Fakers.Category;

public class CategoriesControllerTests :
    IClassFixture<DbFixture>,
    IClassFixture<CustomWebApplicationFactory<IAssemblyReference>>,
    IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<IAssemblyReference> _factory;
    private readonly DbFixture _fixture;

    public CategoriesControllerTests(DbFixture fixture, CustomWebApplicationFactory<IAssemblyReference> factory)
    {
        _fixture = fixture;
        _factory = factory;

        _factory.SetDatabaseInfo(_fixture.ConnectionString, _fixture.DatabaseName);
    }

    public async Task DisposeAsync()
    {
        await _fixture.MongoDatabase.DropCollectionsAsync("categories");
    }

    [Fact]
    public async Task GetCategories_Should_EmptyCategories_When_CategoriesDoNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<List<CategoryResponse>>("/api/categories");

        // Assert
        response.Should().NotBeNull();

        response.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetCategories_Should_ReturnCategories_When_CategoriesExist()
    {
        // Arrange
        var categories = new CategoryFaker().Generate(5);
        await _fixture.MongoDatabase.GetCollection<Category>("categories").InsertManyAsync(categories);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<List<CategoryResponse>>("/api/categories");

        // Assert
        response.Should().NotBeNull();

        response.Should().HaveCount(5);

        response.Should().BeEquivalentTo(categories.Select(x => new CategoryResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
    }

    [Fact]
    public async Task GetCategory_Should_ReturnCategory_When_CategoryExist()
    {
        // Arrange
        var categories = new CategoryFaker().Generate(3);
        await _fixture.MongoDatabase.GetCollection<Category>("categories").InsertManyAsync(categories);

        var client = _factory.CreateClient();

        var categoryId = categories[0].Id;

        // Act
        var response = await client.GetFromJsonAsync<CategoryResponse>($"/api/categories/{categoryId}");

        // Assert
        Assert.NotNull(response);

        response.Id.Should().Be(categoryId);
        response.Name.Should().Be(categories[0].Name);
        response.Description.Should().Be(categories[0].Description);
    }

    [Fact]
    public async Task GetCategory_Should_ReturnNotFound_When_CategoryDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        var categoryId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/categories/{categoryId}");

        // Assert
        Assert.NotNull(response);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(content);
        content.Title.Should().Be("Resource not found");
        content.Detail.Should().Be($"Category with id '{categoryId}' was not found.");
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}