// -------------------------------------------------------------------------------------
//  <copyright file="ApiDemoControllerBase.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers.Base;

using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The API demo controller base class.
/// </summary>
public abstract class ApiDemoControllerBase : ControllerBase
{
    /// <summary>
    /// Create a bad request response with a list of errors.
    /// </summary>
    /// <param name="errors">The list of errors.</param>
    /// <returns>The <see cref="BadRequestObjectResult"/> containing the list of errors.</returns>
    protected BadRequestObjectResult BadRequest(List<Error> errors)
    {
        var problemDetails = new ProblemDetails
        {
            Type = "ApplicationValidationFailure",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status404NotFound,
            Detail = "One or more validation errors occurred.",
            Instance = HttpContext.Request.Path,
            Extensions =
            {
                ["errors"] = errors
            }
        };

        return BadRequest(problemDetails);
    }
}