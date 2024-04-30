// -------------------------------------------------------------------------------------
//  <copyright file="ValidationExceptionHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Handlers;

using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The validation exception handler.
/// </summary>
public sealed class ValidationExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Tries to handle instances of <see cref="ValidationException"/>.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A value indicating whether the exception was handled successfully.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "ValidationFailure",
            Title = "Validation error",
            Detail = "One or more validation errors has occurred"
        };

        if (validationException.Errors is not null)
        {
            problemDetails.Extensions["errors"] = validationException.Errors;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}