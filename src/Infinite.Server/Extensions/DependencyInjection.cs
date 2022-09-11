using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Infinite.Core.Interfaces.Services;
using Infinite.Server.Services;
using Infinite.Shared.Configurations;
using Infinite.Shared.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Infinite.Server.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfiguration>(configuration.GetSection(nameof(AppConfiguration)));
        services.Configure<MailConfiguration>(configuration.GetSection(nameof(MailConfiguration)));
        return services;
    }
        
    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        return services;
    }

    public static IServiceCollection EnableAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfiguration = configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();
        var key = Encoding.UTF8.GetBytes(appConfiguration.Secret);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            bearer.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = c =>
                {
                    if (c.Exception is SecurityTokenExpiredException)
                    {
                        c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        c.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(Result.Fail("The Token is expired."));
                        return c.Response.WriteAsync(result);
                    }
                    else
                    {
                        c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        c.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(Result.Fail("An unhandled error has occurred."));
                        return c.Response.WriteAsync(result);
                    }
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (context.Response.HasStarted) return Task.CompletedTask;
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(Result.Fail("You are not Authorized."));
                    return context.Response.WriteAsync(result);

                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(Result.Fail("You are not authorized to access this resource."));
                    return context.Response.WriteAsync(result);
                },
            };
        });
        return services;
    }
}