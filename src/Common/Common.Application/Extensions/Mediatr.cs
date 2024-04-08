using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Application.Extensions;
public static class Mediatr
{
    public static IServiceCollection AddServiceMediatr(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();
        services.AddMediatR(e =>
            e.RegisterServicesFromAssembly(assembly));

        return services;
    }
}
