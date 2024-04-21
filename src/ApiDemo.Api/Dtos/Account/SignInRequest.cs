// -------------------------------------------------------------------------------------
//  <copyright file="SignInRequest.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Api.Dtos.Account;

using Application.Commands.SignIn;

public class SignInRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public SignInCommand ToCommand()
    {
        return new SignInCommand(Email, Password);
    } 
}