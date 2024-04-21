// -------------------------------------------------------------------------------------
//  <copyright file="AuthSettingsValidator.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Validators;

using FluentValidation;
using Settings;

public class AuthSettingsValidator : AbstractValidator<AuthSettings>
{
    public AuthSettingsValidator()
    {
        RuleFor(x => x.Audience).NotEmpty();
        RuleFor(x => x.Issuer).NotEmpty();
        RuleFor(x => x.Secret).NotEmpty();
        RuleFor(x => x.AccessTokenExpirationInMinutes).GreaterThan(0);
        RuleFor(x => x.RefreshTokenExpirationInMinutes).GreaterThan(0);
    }
}