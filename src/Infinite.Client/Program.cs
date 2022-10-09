global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Infinite.Shared.Wrapper;
using Infinite.Client.Extensions;
using Infinite.Client.Services;

var host = WebAssemblyHostBuilder.CreateDefault(args).ConfigureHost().Build();
host.Services.GetService(typeof(NavigationService));
await host.RunAsync();
