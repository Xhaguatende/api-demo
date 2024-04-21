// -------------------------------------------------------------------------------------
//  <copyright file="AccountAlreadyExistException.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.Exceptions;

using Domain.Exceptions;

public class AccountAlreadyExistException : DomainException
{
    public AccountAlreadyExistException(string email)
        : base($"The account '{email}' already exist.")
    {
        Email = email;
    }

    public string Email { get; private set; }
}