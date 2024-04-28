// -------------------------------------------------------------------------------------
//  <copyright file="CustomWebApplicationFactory.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Infrastructure;

using Fakers.Auth;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private string? _connectionString;
    private string? _database;

    public void SetDatabaseInfo(
        string connectionString,
        string databaseName)
    {
        _connectionString = connectionString;
        _database = databaseName;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                AddMongoDb(services);

                services.AddSingleton<IPolicyEvaluator, PolicyEvaluatorFaker>();
            });
    }

    private void AddMongoDb(IServiceCollection services)
    {
        if (string.IsNullOrEmpty(_connectionString) || string.IsNullOrEmpty(_database))
        {
            return;
        }

        services.RemoveAll<IMongoClient>();
        services.RemoveAll<IMongoDatabase>();

        services
            .AddSingleton<IMongoClient>(new MongoClient(_connectionString))
            .AddSingleton(
                sp =>
                {
                    var mongoClient = sp.GetRequiredService<IMongoClient>();
                    return mongoClient.GetDatabase(_database);
                });
    }
}