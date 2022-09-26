using Infinite.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using MudBlazor.Services;

namespace Infinite.Client.Extensions;

public static class HostingExtensions
{
    public static WebAssemblyHostBuilder ConfigureHost(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.ConfigureClientServices();
        return builder;
    }

    private static WebAssemblyHostBuilder ConfigureClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.ConfigureHttpClients(builder.HostEnvironment.BaseAddress);
        builder.Services.AddApiAuthorization();
        builder.Services.AddMudServices();
        builder.Services.AddMudMarkdownServices();
        builder.Services.AddSingleton<AppearanceService>();
        return builder;
    }

    private static void ConfigureHttpClients(this IServiceCollection services, string baseAddress)
    {
        services.AddHttpClient("Infinite.ServerAPI", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Infinite.ServerAPI"));
    }
}