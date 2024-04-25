// -------------------------------------------------------------------------------------
//  <copyright file="AccountErrors.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.Errors;

using Shared;

public static class AccountErrors
{
    public static Error AccountAlreadyExists(string email, string property = "")
    {
        var message = $"The email: '{email}' already exists.";

        const string code = $"{nameof(AccountErrors)}.{nameof(AccountAlreadyExists)}";

        return new Error(code, message, property);
    }

    public static Error InvalidCredentials(string property = "")
    {
        const string message = "Invalid email or password.";

        const string code = $"{nameof(AccountErrors)}.{nameof(InvalidCredentials)}";

        return new Error(code, message, property);
    }
}