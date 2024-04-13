﻿using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Strava.Infrastructure.Extensions;
internal static class AuthenticationExtensions
{
    public static IServiceCollection AddStravaServiceAuthentication(this IServiceCollection services)
    {
        services.AddServiceAuthentication();
        services.AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie.Name = "X-Refresh-Token";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

                options.SlidingExpiration = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    }
                };

            });

        return services;
    }
}