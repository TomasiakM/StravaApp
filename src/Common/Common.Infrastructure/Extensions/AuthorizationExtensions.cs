﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Common.Infrastructure.Extensions;
public static class AuthorizationExtensions
{
    public static AuthenticationBuilder AddServiceAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = false,

                    SignatureValidator = (string token, TokenValidationParameters parameters) =>
                    {
                        var jwt = new JwtSecurityToken(token);

                        return jwt;
                    },
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
    }
}
