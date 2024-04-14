// -------------------------------------------------------------------------------------
//  <copyright file="EnumAsStringSerializationProvider.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ApiDemo.Infrastructure.Providers;

public class EnumAsStringSerializationProvider : BsonSerializationProviderBase
{
    public override IBsonSerializer? GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
    {
        if (!type.GetTypeInfo().IsEnum)
        {
            return null;
        }

        var enumSerializerType = typeof(EnumSerializer<>).MakeGenericType(type);
        var enumSerializerConstructor = enumSerializerType.GetConstructor([typeof(BsonType)]);
        var enumSerializer = (IBsonSerializer)enumSerializerConstructor!.Invoke([BsonType.String]);

        return enumSerializer;
    }
}