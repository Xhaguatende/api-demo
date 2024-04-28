// -------------------------------------------------------------------------------------
//  <copyright file="AccountsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using Application.Commands.RegisterAccount;
using Application.Commands.SignIn;
using Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ApiDemoControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountCommand command)
    {
        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Value);
    }

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