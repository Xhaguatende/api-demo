// -------------------------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Extensions;

using Application.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PipelineBehaviors;
using Repositories;
using Settings;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        var mongoDbSettings = services.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        services.AddRepositories(mongoDbSettings);

        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    private static void AddRepositories(this IServiceCollection services, MongoDbSettings mongoDbSettings)
    {
        services.AddSingleton(_ =>
        {
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);

            return mongoDatabase;
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        MongoExtensions.SetupMongoConventions();
    }

    private static T GetRequiredService<T>(this IServiceCollection services) where T : class
    {
        var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<T>();

        return options;
    }
}