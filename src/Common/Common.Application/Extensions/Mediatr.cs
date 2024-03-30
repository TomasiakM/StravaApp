using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Application.Extensions;
public static class Mediatr
{
    public static IServiceCollection AddServiceMediatr(this IServiceCollection services)
    {
        services.AddMediatR(e =>
            e.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
