using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using shelf_viz_mod;
using shelf_viz_mod.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ISKUService, SKUService>();

// Register ShelfService using a factory
builder.Services.AddScoped<IShelfService>(serviceProvider =>
{
    var httpClient = serviceProvider.GetRequiredService<HttpClient>();
    var localStorage = serviceProvider.GetRequiredService<ILocalStorageService>();
    var logger = serviceProvider.GetRequiredService<ILogger<ShelfService>>();
    return ShelfService.CreateAsync(localStorage, httpClient, logger).GetAwaiter().GetResult();
});

await builder.Build().RunAsync();
