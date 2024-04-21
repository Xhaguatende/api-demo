﻿// -------------------------------------------------------------------------------------
//  <copyright file="SignInCommand.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.SignIn;

using MediatR;

public record SignInCommand(string Email, string Password) : IRequest<SignInCommandResponse>;