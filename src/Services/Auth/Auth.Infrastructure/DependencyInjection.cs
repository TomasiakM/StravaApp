using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;
using Auth.Infrastructure.Extensions;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Interfaces.Services.StravaService;
using Auth.Infrastructure.Interfaces.Utils;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Services.Auth;
using Auth.Infrastructure.Services.StravaService;
using Auth.Infrastructure.Utils;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Strava.Infrastructure.Services.Auth;

namespace Auth.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceSettings();
        services.AddServiceMassTransit();
        services.AddAuthServiceAuthentication();

        services.AddServiceDbContext<ServiceDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IAuthorizeCodeService, AuthorizeCodeService>();

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IRefreshStravaTokenService, RefreshStravaTokenService>();

        services.AddScoped<ITokenProvider, TokenProvider>();

        return services;
    }

    public static async Task<WebApplication> MigrateAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetService<ServiceDbContext>();
        await dbContext!.Database.MigrateAsync();

        return app;
    }
}
