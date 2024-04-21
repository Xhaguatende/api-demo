// -------------------------------------------------------------------------------------
//  <copyright file="ServiceCollectionExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Extensions;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddApplicationMediator();

        services.AddValidatorsFromAssembly(typeof(IAssemblyReference).Assembly);

        return services;
    }

    private static void AddApplicationMediator(this IServiceCollection services)
    {
        var applicationAssembly = typeof(IAssemblyReference).Assembly;

        services.AddMediatR(
            cfg => { cfg.RegisterServicesFromAssembly(applicationAssembly); });
    }
}