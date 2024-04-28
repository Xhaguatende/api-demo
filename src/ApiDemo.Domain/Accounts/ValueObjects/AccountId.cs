// -------------------------------------------------------------------------------------
//  <copyright file="AccountId.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.ValueObjects;

using Primitives;

public class AccountId : ValueObject
{
    public AccountId(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        Email = email;
    }

    public string Email { get; private set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
    }
}