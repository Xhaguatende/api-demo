// -------------------------------------------------------------------------------------
//  <copyright file="ProblemDetailsExtensions.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Extensions;

using System.Text.Json;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

public static class ProblemDetailsExtensions
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static List<Error> GetErrors(this ProblemDetails problemDetails)
    {
        var errorsObject = problemDetails.Extensions["errors"];
        var errorsString = errorsObject?.ToString() ?? string.Empty;

        var errors = JsonSerializer.Deserialize<List<Error>>(errorsString, Options);

        return errors ?? [];
    }
}