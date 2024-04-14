// -------------------------------------------------------------------------------------
//  <copyright file="MongoDbSettings.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = default!;

    public string DatabaseName { get; set; } = default!;

}