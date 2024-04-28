// -------------------------------------------------------------------------------------
//  <copyright file="PolicyEvaluatorFaker.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Tests.Fakers.Auth;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

public class PolicyEvaluatorFaker : IPolicyEvaluator
{
    public virtual Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var principal = new ClaimsPrincipal();

        principal.AddIdentity(
            new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim(ClaimTypes.NameIdentifier, "admin")
                ],
                "FakeScheme"));

        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    "FakeScheme")));
    }

    public virtual Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object? resource)
    {
        return Task.FromResult(PolicyAuthorizationResult.Success());
    }
}