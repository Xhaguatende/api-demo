// -------------------------------------------------------------------------------------
//  <copyright file="FluentValidationOptions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Validators;

using FluentValidation;
using Microsoft.Extensions.Options;

/// <summary>
/// The fluent validation options.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentValidationOptions{TOptions}"/> class.
    /// </summary>
    /// <param name="name">The name of the options instance</param>
    /// <param name="validator">The option's validator.</param>
    public FluentValidationOptions(string name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    /// <summary>
    /// Gets the name of the options instance to validate.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Validates the options.
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options.</param>
    /// <returns>An instance of <see cref="ValidateOptionsResult"/></returns>
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (!string.IsNullOrWhiteSpace(Name) && Name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        var validationResult = _validator.Validate(options);

        if (validationResult.IsValid) return ValidateOptionsResult.Success;

        var errors = validationResult.Errors.Select(
            x =>
                $"Options validation ({_validator.GetType().Name}) failed for '{x.PropertyName}' with error: '{x.ErrorMessage}' ");

        return ValidateOptionsResult.Fail(errors);
    }
}