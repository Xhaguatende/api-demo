// -------------------------------------------------------------------------------------
//  <copyright file="RegisterAccountResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.RegisterAccount;

public record RegisterAccountResponse(bool Success, string Email);