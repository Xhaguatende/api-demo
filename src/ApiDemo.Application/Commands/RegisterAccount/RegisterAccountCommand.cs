// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

using Domain.Shared;
using MediatR;

/// <summary>
/// Defines the register account command.
/// </summary>
public record RegisterAccountCommand : IRequest<Result<RegisterAccountCommandResponse>>
{
    /// <summary>
    /// Gets or sets the password confirmation.
    /// </summary>
    public string ConfirmPassword { get; set; } = default!;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = default!;
}