using Blazored.LocalStorage;
using Infinite.Client.Authentication;
using Infinite.Client.Services.HttpClients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Infinite.Client.Extensions;

public static class DependencyInjection
{
    public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        return builder;
    }

    public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddHttpClients(builder.HostEnvironment.BaseAddress);
        builder.Services.AddMudServices();
        builder.Services.AddTransient<AuthenticationHeaderHandler>();
        builder.Services.AddTransient<AppAuthenticationStateProvider>();
        builder.Services.AddTransient<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        return builder;
    }

    private static void AddHttpClients(this IServiceCollection services, string baseAddress)
    {
        services.AddTransient<AuthenticationHttpClient>();
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("InfiniteAPI").EnableIntercept(sp));
        services.AddHttpClient("InfiniteAPI", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<AuthenticationHeaderHandler>();
        services.AddHttpClientInterceptor();
    }
}