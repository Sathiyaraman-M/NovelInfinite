using Infinite.Core.Interfaces.Services;
using Infinite.Server.Services;

namespace Infinite.Server.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        return services;
    }
}