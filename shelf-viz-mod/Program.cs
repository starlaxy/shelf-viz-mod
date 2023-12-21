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
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<IShelfService, ShelfService>();
builder.Services.AddScoped<IDragStateService, DragStateService>();
builder.Services.AddSingleton<IScopedServiceFactory, ScopedServiceFactory>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IModalService, ModalService>();

await builder.Build().RunAsync();
