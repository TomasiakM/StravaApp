namespace Gateway.Api.Extensions;

public static class CorsExtensions
{
    public const string CORS_NAME = "DefaultCorsPolicy";
    public static IServiceCollection AddGatewayCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(CORS_NAME, corsBuilder =>
            {
                corsBuilder
                    .WithOrigins(configuration.GetValue<string>("AllowOrigin"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
