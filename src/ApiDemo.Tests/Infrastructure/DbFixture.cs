namespace ApiDemo.Tests.Infrastructure;

using ApiDemo.Infrastructure.Extensions;
using MongoDB.Driver;

public class DbFixture : IDisposable
{
    public DbFixture()
    {
        DatabaseName = $"product-catalogue-{Guid.NewGuid()}";

        var mongoClient = new MongoClient(ConnectionString);

        MongoDatabase = mongoClient.GetDatabase(DatabaseName);

        MongoExtensions.SetupMongoConventions();
    }

    public string ConnectionString => "mongodb://localhost:27017/?directConnection=true";
    public string DatabaseName { get; }

    public IMongoDatabase MongoDatabase { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        var mongoClient = new MongoClient(ConnectionString);

        mongoClient.DropDatabase(DatabaseName);
    }
}