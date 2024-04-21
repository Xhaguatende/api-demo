// -------------------------------------------------------------------------------------
//  <copyright file="Account.cs" company="{Company Name}">
//    Copyright (c) {Company Name}. All rights reserved.
//  </copyright>
// -------------------------------------------------------------------------------------

namespace ApiDemo.Domain.Accounts.Entity;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Primitives;

public class Account : Entity<Guid>
{
    public Account(string email, string password)
    : base(Guid.NewGuid())
    {
        Email = email;
        var salt = GenerateSalt(16);
        PasswordSalt = Convert.ToBase64String(salt);
        PasswordHash = HashPassword(password, salt);
        RefreshToken = string.Empty;
    }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }
    public string RefreshToken { get; private set; }

    public DateTime RefreshTokenExpiration { get; private set; }

    public void UpdateRefreshToken(string refreshToken, int refreshTokenExpirationInSeconds)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiration = DateTime.UtcNow.AddSeconds(refreshTokenExpirationInSeconds);
    }

    public bool VerifyPassword(string password)
    {
        var salt = Convert.FromBase64String(PasswordSalt);
        var hash = HashPassword(password, salt);
        return hash == PasswordHash;
    }

    private static byte[] GenerateSalt(int size)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static string HashPassword(string password, byte[] salt)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        return Convert.ToBase64String(hash);
    }
}