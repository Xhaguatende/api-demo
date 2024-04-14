// -------------------------------------------------------------------------------------
//  <copyright file="MongoDbSettingsValidator.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

using ApiDemo.Infrastructure.Settings;
using FluentValidation;

namespace ApiDemo.Infrastructure.Validators;

public class MongoDbSettingsValidator : AbstractValidator<MongoDbSettings>
{
    public MongoDbSettingsValidator()
    {
        RuleFor(x => x.ConnectionString).NotEmpty();
        RuleFor(x => x.DatabaseName).NotEmpty();
    }
}