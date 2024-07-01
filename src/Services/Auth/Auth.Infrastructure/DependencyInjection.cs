using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Interfaces.Services.StravaService;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Services.Auth;
using Auth.Infrastructure.Services.StravaService;
using Common.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceDbContext<ServiceDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IAuthorizeCodeService, AuthorizeCodeService>();

        return services;
    }
}
