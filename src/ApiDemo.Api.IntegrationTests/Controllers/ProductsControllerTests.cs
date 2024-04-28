// -------------------------------------------------------------------------------------
//  <copyright file="ProductsControllerTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using ApiDemo.Tests.Extensions;
using ApiDemo.Tests.Infrastructure;
using Application.Commands.UpsertProduct;
using Application.Responses.Product;
using Bogus;
using Domain.Categories.Entity;
using Domain.Products.Entity;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Tests.Fakers.Category;
using Tests.Fakers.Product;

public class ProductsControllerTests :
    IClassFixture<DbFixture>,
    IClassFixture<CustomWebApplicationFactory<IAssemblyReference>>,
    IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<IAssemblyReference> _factory;
    private readonly DbFixture _fixture;

    public ProductsControllerTests(DbFixture fixture, CustomWebApplicationFactory<IAssemblyReference> factory)
    {
        _fixture = fixture;
        _factory = factory;

        _factory.SetDatabaseInfo(_fixture.ConnectionString, _fixture.DatabaseName);
    }

    [Fact]
    public async Task DeleteProductAsync_Should_DeleteProduct_When_ProductExist()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        await _fixture.MongoDatabase.GetCollection<Product>("products").InsertOneAsync(product);

        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/api/products/{product.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var deletedProduct = await _fixture.MongoDatabase.GetCollection<Product>("products")
            .Find(x => x.Id == product.Id)
            .SingleOrDefaultAsync();

        Assert.Null(deletedProduct);
    }

    public async Task DisposeAsync()
    {
        await _fixture.MongoDatabase.DropCollectionsAsync("categories", "products");
    }

    [Fact]
    public async Task GetProductAsync_Should_ReturnNotFound_When_ProductDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        var productId = new Faker().Random.Guid();

        // Act
        var response = await client.GetAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);
        problemDetails.Title.Should().Be("Resource not found");
        problemDetails.Detail.Should().Be($"Product with id '{productId}' was not found.");
    }

    [Fact]
    public async Task GetProductAsync_Should_ReturnProduct_When_ProductExist()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        await _fixture.MongoDatabase.GetCollection<Category>("categories").InsertOneAsync(category);

        var products = new ProductFaker().Generate(3);

        foreach (var product in products)
        {
            product.UpdateCategory(category.Id);
        }

        await _fixture.MongoDatabase.GetCollection<Product>("products").InsertManyAsync(products);

        var client = _factory.CreateClient();

        var productId = products[0].Id;

        // Act
        var response = await client.GetFromJsonAsync<ProductResponse>($"/api/products/{productId}");

        // Assert
        Assert.NotNull(response);

        response.Id.Should().Be(productId);
        response.Name.Should().Be(products[0].Name);
        response.Description.Should().Be(products[0].Description);
        response.Price.Should().Be(products[0].Price);
        response.Stock.Should().Be(products[0].Stock);
        response.Category.Id.Should().Be(category.Id);
        response.Category.Name.Should().Be(category.Name);
    }

    [Fact]
    public async Task GetProductsAsync_Should_EmptyProducts_When_ProductsDoNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<List<ProductResponse>>("/api/products");

        // Assert
        response.Should().NotBeNull();

        response.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetProductsAsync_Should_ReturnProducts_When_ProductsExist()
    {
        // Arrange
        var category = new CategoryFaker().Generate();
        await _fixture.MongoDatabase.GetCollection<Category>("categories").InsertOneAsync(category);

        var products = new ProductFaker().Generate(5);

        foreach (var product in products)
        {
            product.UpdateCategory(category.Id);
        }

        await _fixture.MongoDatabase.GetCollection<Product>("products").InsertManyAsync(products);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetFromJsonAsync<List<ProductResponse>>("/api/products");

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(5);

        response.Should().BeEquivalentTo(
            products.Select(
                x => new ProductResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Stock = x.Stock,
                    Category = new ProductCategoryResponse
                    {
                        Id = category.Id,
                        Name = category.Name
                    }
                }));
    }

    public async Task InitializeAsync()
    {
        await _fixture.MongoDatabase.CreateProductsAggregateViewAsync();
    }

    [Fact]
    public async Task UpsertProductAsync_Should_CreateProduct_When_ProductDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();

        var product = new ProductFaker().Generate();

        var command = new UpsertProductCommand
        {
            CategoryId = product.CategoryId,
            Description = product.Description,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.EnsureSuccessStatusCode();

        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(productResponse);

        var createdProduct = await _fixture.MongoDatabase.GetCollection<Product>("products")
            .Find(x => x.Id == productResponse.Id)
            .SingleOrDefaultAsync();

        Assert.NotNull(createdProduct);
        createdProduct.Id.Should().Be(productResponse.Id);
        createdProduct.Name.Should().Be(productResponse.Name);
        createdProduct.Description.Should().Be(productResponse.Description);
        createdProduct.Price.Should().Be(productResponse.Price);
        createdProduct.Stock.Should().Be(productResponse.Stock);
        createdProduct.CategoryId.Should().Be(productResponse.Category.Id);
    }

    [Fact]
    public async Task UpsertProductAsync_Should_UpdateProduct_When_ProductExist()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        await _fixture.MongoDatabase.GetCollection<Product>("products").InsertOneAsync(product);

        var client = _factory.CreateClient();

        var faker = new Faker();

        var command = new UpsertProductCommand
        {
            CategoryId = product.CategoryId,
            Description = faker.Lorem.Sentence(),
            Id = product.Id,
            Name = faker.Commerce.ProductName(),
            Price = faker.Random.Decimal(1, 100),
            Stock = faker.Random.Int(1, 1000)
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.EnsureSuccessStatusCode();

        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();

        Assert.NotNull(productResponse);

        var updatedProduct = await _fixture.MongoDatabase.GetCollection<Product>("products")
            .Find(x => x.Id == productResponse.Id)
            .SingleOrDefaultAsync();

        Assert.NotNull(updatedProduct);
        updatedProduct.Id.Should().Be(productResponse.Id);
        updatedProduct.Name.Should().Be(productResponse.Name);
        updatedProduct.Description.Should().Be(productResponse.Description);
        updatedProduct.Price.Should().Be(productResponse.Price);
        updatedProduct.Stock.Should().Be(productResponse.Stock);
        updatedProduct.CategoryId.Should().Be(productResponse.Category.Id);
    }
}