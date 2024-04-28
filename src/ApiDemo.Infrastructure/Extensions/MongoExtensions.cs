// -------------------------------------------------------------------------------------
//  <copyright file="MongoExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Accounts.ValueObjects;
using Domain.Primitives;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Providers;

public static class MongoExtensions
{
    private static readonly object Lock = new();

    private static volatile bool _isMongoConventionsInitialized;

    public static UpdateDefinition<TDocument> SetAll<TDocument, TModel>(
                this UpdateDefinitionBuilder<TDocument> updateBuilder,
        TModel value,
        params string[]? excludeProperties)
    {
        var excludedPropertiesSet = new HashSet<string>(excludeProperties ?? Enumerable.Empty<string>());
        var updateDefinitions = new List<UpdateDefinition<TDocument>>();

        foreach (var property in value!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!excludedPropertiesSet.Contains(property.Name))
            {
                updateDefinitions.Add(updateBuilder.Set(property.Name, property.GetValue(value)));
            }
        }

        return updateBuilder.Combine(updateDefinitions);
    }

    public static void SetupMongoConventions()
    {
        lock (Lock)
        {
            if (_isMongoConventionsInitialized)
            {
                return;
            }

            InitializeMongoConventions();

            _isMongoConventionsInitialized = true;
        }
    }

    private static void InitializeMongoConventions()
    {
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));

        BsonSerializer.RegisterSerializer(
            typeof(decimal?),
            new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        var enumAsStringSerializationProvider = new EnumAsStringSerializationProvider();
        BsonSerializer.RegisterSerializationProvider(enumAsStringSerializationProvider);

        var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", camelCaseConvention, _ => true);

        var ignoreConvention = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("ignoreExtraElements", ignoreConvention, _ => true);

        var enumConvention = new ConventionPack { new EnumRepresentationConvention(BsonType.String) };
        ConventionRegistry.Register("EnumStringConvention", enumConvention, _ => true);

        BsonClassMap.RegisterClassMap<Entity<Guid>>(
            cm =>
            {
                cm.AutoMap();
                cm.UnmapProperty("Id");
                cm.MapMember(x => x.Id)
                    .SetOrder(0)
                    .SetElementName("id");
            });

        BsonClassMap.RegisterClassMap<Entity<AccountId>>(
            cm =>
            {
                cm.AutoMap();
                cm.UnmapProperty("Id");
                cm.MapMember(x => x.Id)
                    .SetOrder(0)
                    .SetElementName("id");
            });
    }
}