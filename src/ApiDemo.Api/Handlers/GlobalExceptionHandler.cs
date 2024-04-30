// -------------------------------------------------------------------------------------
//  <copyright file="GlobalExceptionHandler.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Handlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The global exception handler.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">An instance of <see cref="ILogger{GlobalExceptionHandler}"/>.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Tries to handle unspecified exceptions (i.e., <see cref="Exception"/>).
    /// Writes the details as a generic response and logs the exception.
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
        _logger.LogError(
            exception,
            "Exception occurred: {Message}",
            exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server error",
            Detail = "An unexpected error occurred while processing the request."
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}