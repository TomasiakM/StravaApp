using Activities.Application.Interfaces;
using Activities.Infrastracture.Extensions;
using Activities.Infrastracture.Persistence;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Activities.Infrastracture;
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
