using Common.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gateway.Api.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddGatewayAuthorizationPolicy(this IServiceCollection services)
    {

        var serviceProvider = services.BuildServiceProvider();
        var tokenSettings = serviceProvider.GetRequiredService<IOptions<TokenSettings>>().Value;

        services.AddAuthorization(options =>
        {
            options.AddPolicy("LoggedInPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.NameIdentifier);
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = tokenSettings.Issuer,
                    ValidateIssuer = true,
                    ValidIssuer = tokenSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key)),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        return services;
    }
}
