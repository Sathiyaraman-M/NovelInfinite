global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using Infinite.Shared.Wrapper;
using Infinite.Client.Extensions;

var host = WebAssemblyHostBuilder.CreateDefault(args).AddRootComponents().AddClientServices().Build();
await host.RunAsync();
