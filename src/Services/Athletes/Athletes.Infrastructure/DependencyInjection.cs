using Athletes.Application.Interfaces;
using Athletes.Infrastructure.Extensions;
using Athletes.Infrastructure.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Athletes.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServiceSettings();
        services.AddServiceMassTransit();

        services.AddServiceAuthentication();

        services.AddServiceDbContext<ServiceDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

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
