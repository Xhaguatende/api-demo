// -------------------------------------------------------------------------------------
//  <copyright file="OptionsBuilderExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Extensions;

using FluentValidation;
using Microsoft.Extensions.Options;
using Validators;

/// <summary>
/// The options builder extensions.
/// </summary>
public static class OptionsBuilderExtensions
{
    /// <summary>
    /// Validate the options.
    /// </summary>
    /// <param name="optionsBuilder">The option's builder.</param>
    /// <returns>The option's builder.</returns>
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            s => new FluentValidationOptions<TOptions>(optionsBuilder.Name, s.GetRequiredService<IValidator<TOptions>>()));

        return optionsBuilder;
    }
}