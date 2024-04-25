// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

using Domain.Shared;
using MediatR;

public record RegisterAccountCommand : IRequest<Result<RegisterAccountResponse>>
{
    public string ConfirmPassword { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}