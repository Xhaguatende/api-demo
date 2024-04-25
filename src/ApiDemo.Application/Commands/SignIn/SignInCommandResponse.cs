// -------------------------------------------------------------------------------------
//  <copyright file="SignInCommandResponse.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Application.Commands.SignIn;

using System.Text.Json.Serialization;

public class SignInCommandResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = default!;

    [JsonPropertyName("token_type")]
    public string TokenType => "Bearer";
}