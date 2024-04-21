// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountRequest.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos.Account;

using Application.Commands.RegisterAccount;

public class RegisterAccountRequest
{
    public string ConfirmPassword { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public RegisterAccountCommand ToCommand()
    {
        return new RegisterAccountCommand
        {
            ConfirmPassword = ConfirmPassword,
            Email = Email,
            Password = Password
        };
    }
}