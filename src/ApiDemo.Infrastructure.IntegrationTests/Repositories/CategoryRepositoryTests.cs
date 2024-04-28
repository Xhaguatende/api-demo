// -------------------------------------------------------------------------------------
//  <copyright file="CategoryRepositoryTests.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.IntegrationTests.Repositories;

using FluentAssertions;
using Infrastructure.Repositories;
using Tests.Fakers.Category;
using Tests.Infrastructure;

public class CategoryRepositoryTests : IClassFixture<DbFixture>
{
    private readonly CategoryRepository _sut;

    public CategoryRepositoryTests(DbFixture fixture)
    {
        _sut = new CategoryRepository(fixture.MongoDatabase);
    }

    [Fact]
    public async Task UpsertOneAsync_Should_CreateCategory()
    {
        // Arrange
        var category = new CategoryFaker().Generate();

        // Act
        await _sut.UpsertOneAsync(category, CancellationToken.None);

        // Assert
        var createdCategory = await _sut.GetByIdAsync(category.Id, CancellationToken.None);
        createdCategory.Should().NotBeNull();

        createdCategory.Should().BeEquivalentTo(category);
    }
}