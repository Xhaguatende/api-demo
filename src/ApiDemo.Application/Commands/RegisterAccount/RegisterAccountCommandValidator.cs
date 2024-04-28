// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountCommandValidator.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

using FluentValidation;

public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(50)
            .Matches("[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches("[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches("[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.\#\+\-\\_]+").WithMessage("Your password must contain at least one of the following: '! ? * . # + - _'");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .MinimumLength(6)
            .Equal(x => x.Password)
            .WithMessage("Confirm Password must match Password");
    }
}