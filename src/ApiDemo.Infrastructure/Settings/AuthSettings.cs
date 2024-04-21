// -------------------------------------------------------------------------------------
//  <copyright file="AuthSettings.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Infrastructure.Settings;

public class AuthSettings
{
    public int AccessTokenExpirationInMinutes { get; set; } = 60; // 1 hour
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int RefreshTokenExpirationInMinutes { get; set; } = 7 * 24 * 60; // 7 days
    public string Secret { get; set; } = default!;
}