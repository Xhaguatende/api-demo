// -------------------------------------------------------------------------------------
//  <copyright file="AccountsController.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Controllers;

using Dtos.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountRequest request)
    {
        await _mediator.Send(request.ToCommand());

        return Ok();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        var response = await _mediator.Send(request.ToCommand());

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}