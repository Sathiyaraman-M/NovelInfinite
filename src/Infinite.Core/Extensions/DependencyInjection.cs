using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Core.Permissions;
using Infinite.Core.Persistence;
using Infinite.Core.Services.Identity;
using Infinite.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infinite.Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, string connectionName)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(connectionName)));
        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionPolicyHandler>();
        services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        return services;
    }
}