// -------------------------------------------------------------------------------------
//  <copyright file="ApiDemoControllerBase.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers.Base;

using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

public abstract class ApiDemoControllerBase : ControllerBase
{
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