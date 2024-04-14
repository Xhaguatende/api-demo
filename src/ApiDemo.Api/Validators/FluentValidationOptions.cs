// -------------------------------------------------------------------------------------
//  <copyright file="FluentValidationOptions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

using FluentValidation;
using Microsoft.Extensions.Options;

namespace ApiDemo.Api.Validators;

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    public FluentValidationOptions(string name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    public string Name { get; }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (!string.IsNullOrWhiteSpace(Name) && Name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var errors = validationResult.Errors.Select(
            x =>
                $"Options validation ({_validator.GetType().Name}) failed for '{x.PropertyName}' with error: '{x.ErrorMessage}' ");

        return ValidateOptionsResult.Fail(errors);
    }
}