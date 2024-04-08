using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.Application.Extensions;
public static class Mapster
{
    public static IServiceCollection AddServiceMapper(this IServiceCollection services, Action<TypeAdapterConfig>? options = null)
    {
        var assembly = Assembly.GetCallingAssembly();
        var config = new TypeAdapterConfig();
        config.Scan(assembly);

        options?.Invoke(config);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
