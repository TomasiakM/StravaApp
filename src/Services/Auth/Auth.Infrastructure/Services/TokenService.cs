﻿using Auth.Infrastructure.Interfaces;
using Common.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Services;
public sealed class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;

    public TokenService(IOptions<TokenSettings> tokenSettingOptions)
    {
        _tokenSettings = tokenSettingOptions.Value;
    }

    public string GenerateToken(long stravaUserId)
    {
        var data = Encoding.UTF8.GetBytes(_tokenSettings.Key);
        var securityKey = new SymmetricSecurityKey(data);

        var claims = new Dictionary<string, object>
        {
            [ClaimTypes.NameIdentifier] = stravaUserId,
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Issuer,
            Claims = claims,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JsonWebTokenHandler();
        handler.SetDefaultTimesOnTokenCreation = false;

        return handler.CreateToken(descriptor);
    }
}
