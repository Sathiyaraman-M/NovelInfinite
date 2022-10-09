using Infinite.Client.Services;
using Infinite.Client.Services.HttpClients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using TabBlazor;

namespace Infinite.Client.Extensions;

public static class HostingExtensions
{
    public static WebAssemblyHostBuilder ConfigureHost(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        return builder.ConfigureClientServices();
    }

    private static WebAssemblyHostBuilder ConfigureClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.ConfigureHttpClients(builder.HostEnvironment.BaseAddress);
        builder.Services.AddApiAuthorization();
        builder.Services.AddTabBlazor();
        builder.Services.AddSingleton<NavigationService>();
        return builder;
    }

    private static void ConfigureHttpClients(this IServiceCollection services, string baseAddress)
    {
        services.AddHttpClient("Infinite.ServerAPI", client => client.BaseAddress = new Uri(baseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Infinite.ServerAPI"));
        services.AddHttpClient<PublicHttpClient>(client => client.BaseAddress = new Uri(baseAddress));
    }
}