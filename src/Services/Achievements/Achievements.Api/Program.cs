using Achievements.Application;
using Achievements.Infrastructure;
using Common.Api;
using Common.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure()
    .AddApplication();


var app = builder.Build();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();