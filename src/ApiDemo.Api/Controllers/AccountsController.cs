// -------------------------------------------------------------------------------------
//  <copyright file="AccountsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using System.Net.Mime;
using Application.Commands.RegisterAccount;
using Application.Commands.SignIn;
using Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// The account's controller.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountsController"/> class.
    /// </summary>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Register a new account.
    /// </summary>
    /// <param name="command">The command to register an account.</param>
    /// <returns>The account registration response</returns>
    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status200OK, "The account was registered successfully.", typeof(RegisterAccountCommandResponse))]
    [ProducesResponseType(typeof(RegisterAccountCommandResponse), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Unable to register the account.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Value);
    }

    /// <summary>
    /// Signs in the user/account.
    /// </summary>
    /// <param name="request">The sign-in request.</param>
    /// <returns>The sign-in response.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "The account was signed-in successfully.", typeof(SignInCommandResponse))]
    [ProducesResponseType(typeof(SignInCommandResponse), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Unable to sign-in.", typeof(ProblemDetails))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInCommand request)
    {
        var response = await _mediator.Send(request);

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Value);
    }
}