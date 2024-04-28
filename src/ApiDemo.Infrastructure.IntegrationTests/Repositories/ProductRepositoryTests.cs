// -------------------------------------------------------------------------------------
//  <copyright file="ProductRepositoryTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.IntegrationTests.Repositories;

using ApiDemo.Infrastructure.Repositories;
using Bogus;
using Domain.Products.Entity;
using FluentAssertions;
using Tests.Extensions;
using Tests.Fakers.Category;
using Tests.Fakers.Product;
using Tests.Infrastructure;

public class ProductRepositoryTests : IClassFixture<DbFixture>, IAsyncLifetime
{
    private readonly CategoryRepository _categoryRepository;
    private readonly DbFixture _fixture;
    private readonly ProductRepository _sut;

    public ProductRepositoryTests(DbFixture fixture)
    {
        _fixture = fixture;
        _categoryRepository = new CategoryRepository(_fixture.MongoDatabase);
        _sut = new ProductRepository(_fixture.MongoDatabase);
    }

    [Fact]
    public async Task DeleteOneAsync_Should_DeleteProduct_When_ProductExists()
    {
        // Arrange

        var faker = new Faker();
        var name = faker.Commerce.Product();
        var description = faker.Commerce.ProductAdjective();

        var product = new Product(
            name,
            description,
            Guid.NewGuid(),
            faker.Random.Decimal(0.1m, 100),
            faker.Random.Int(1, 100));

        await _sut.UpsertOneAsync(product, CancellationToken.None);

        // Act
        await _sut.DeleteOneAsync(product.Id, CancellationToken.None);

        // Assert
        var deletedProduct = await _sut.GetByIdAsync(product.Id, CancellationToken.None);
        deletedProduct.Should().BeNull();
    }

    public async Task DisposeAsync()
    {
        await _fixture.MongoDatabase.DropCollectionsAsync("products");
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_ProductDoesNotExist()
    {
        // Arrange

        // Act
        var product = await _sut.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnProduct_When_ProductExists()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        await _sut.UpsertOneAsync(product, CancellationToken.None);

        // Act
        var createdProduct = await _sut.GetByIdAsync(product.Id, CancellationToken.None);

        // Assert
        createdProduct.Should().NotBeNull();
        createdProduct.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetManyAggregatesByExpressionAsync_Should_ReturnEmptyList_When_ProductsDoNotExist()
    {
        // Arrange

        // Act
        var products = await _sut.GetManyAggregatesByExpressionAsync(
            p => p.Name == "non-existing",
            CancellationToken.None);

        // Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task GetManyAggregatesByExpressionAsync_Should_ReturnProducts_When_ProductsExist()
    {
        // Arrange
        var category = new CategoryFaker().Generate();

        await _categoryRepository.UpsertOneAsync(category, CancellationToken.None);

        var products = new ProductFaker().Generate(2);
        products.ForEach(p => p.UpdateCategory(category.Id));

        foreach (var product in products)
        {
            await _sut.UpsertOneAsync(product, CancellationToken.None);
        }

        await _fixture.MongoDatabase.CreateProductsAggregateViewAsync();

        // Act
        var productsSaved = await _sut.GetManyAggregatesByExpressionAsync(
            p => p.CategoryId == category.Id,
            CancellationToken.None);

        // Assert
        productsSaved.Should().NotBeNullOrEmpty();
        productsSaved.Should().HaveCount(2);

        productsSaved.ForEach(
            p =>
            {
                p.Category.Should().NotBeNull();
                p.Category.Id.Should().Be(category.Id);
                p.Category.Name.Should().Be(category.Name);
                p.Category.Description.Should().Be(category.Description);
            });
    }

    [Fact]
    public async Task GetManyByExpressionAsync_Should_ReturnEmptyList_When_ProductsDoNotExist()
    {
        // Arrange

        // Act
        var products = await _sut.GetManyByExpressionAsync(
            p => p.Name == "non-existing",
            CancellationToken.None);

        // Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task GetManyByExpressionAsync_Should_ReturnProducts_When_ProductsExist()
    {
        // Arrange
        var products = new ProductFaker().Generate(2);
        const string name = "SameName";
        foreach (var product in products)
        {
            product.UpdateName(name);
            await _sut.UpsertOneAsync(product, CancellationToken.None);
        }

        // Act
        var productsSaved = await _sut.GetManyByExpressionAsync(p => p.Name == name, CancellationToken.None);

        // Assert
        productsSaved.Should().NotBeNullOrEmpty();
        productsSaved.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetOneAggregateByExpressionAsync_Should_ReturnNull_When_ProductDoesNotExist()
    {
        // Arrange

        // Act
        var product = await _sut.GetOneAggregateByExpressionAsync(
            p => p.Name == "non-existing",
            CancellationToken.None);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task GetOneAggregateByExpressionAsync_Should_ReturnProduct_When_ProductExists()
    {
        // Arrange
        var category = new CategoryFaker().Generate();

        await _categoryRepository.UpsertOneAsync(category, CancellationToken.None);

        var product = new ProductFaker().Generate();
        product.UpdateCategory(category.Id);

        await _sut.UpsertOneAsync(product, CancellationToken.None);

        await _fixture.MongoDatabase.CreateProductsAggregateViewAsync();

        // Act
        var createdProduct =
            await _sut.GetOneAggregateByExpressionAsync(p => p.Name == product.Name, CancellationToken.None);

        // Assert
        createdProduct.Should().NotBeNull();
        createdProduct.Should().BeEquivalentTo(product);

        createdProduct!.Category.Should().NotBeNull();
        createdProduct.Category.Id.Should().Be(category.Id);
        createdProduct.Category.Name.Should().Be(category.Name);
        createdProduct.Category.Description.Should().Be(category.Description);
    }

    [Fact]
    public async Task GetOneByExpressionAsync_Should_ReturnNull_When_ProductDoesNotExist()
    {
        // Arrange

        // Act
        var product = await _sut.GetOneByExpressionAsync(
            p => p.Name == "non-existing",
            CancellationToken.None);

        // Assert
        product.Should().BeNull();
    }

    [Fact]
    public async Task GetOneByExpressionAsync_Should_ReturnProduct_When_ProductExists()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        await _sut.UpsertOneAsync(product, CancellationToken.None);

        // Act
        var createdProduct =
            await _sut.GetOneByExpressionAsync(p => p.Name == product.Name, CancellationToken.None);

        // Assert
        createdProduct.Should().NotBeNull();
        createdProduct.Should().BeEquivalentTo(product);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task UpsertOneAsync_Should_CreateProduct_When_ProductDoesNotExist()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        // Act
        await _sut.UpsertOneAsync(product, CancellationToken.None);

        // Assert
        var createdProduct = await _sut.GetByIdAsync(product.Id, CancellationToken.None);
        createdProduct.Should().NotBeNull();

        createdProduct.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task UpsertOneAsync_Should_UpdateProduct_When_ProductExists()
    {
        // Arrange
        var product = new ProductFaker().Generate();

        await _sut.UpsertOneAsync(product, CancellationToken.None);

        var faker = new Faker();
        product.UpdateName(faker.Commerce.Product());
        product.UpdateDescription(faker.Commerce.ProductAdjective());
        product.UpdatePrice(faker.Random.Decimal(0.1m, 100));
        product.UpdateStock(faker.Random.Int(1, 100));

        // Act
        await _sut.UpsertOneAsync(product, CancellationToken.None);

        // Assert
        var updatedProduct = await _sut.GetByIdAsync(product.Id, CancellationToken.None);
        updatedProduct.Should().NotBeNull();
        updatedProduct.Should().BeEquivalentTo(product);
    }
}