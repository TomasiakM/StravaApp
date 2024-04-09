using Gateway.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddGatewaySettings();
builder.Services.AddGatewayAuthorizationPolicy();
builder.Services.AddGatewayCors(builder.Configuration);


var app = builder.Build();

app.UseCors(CorsExtensions.CORS_NAME);

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
