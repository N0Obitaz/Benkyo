using Benkyo.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddMemoryCache();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<AuthService>()
    .AddClasses(classes => classes.InNamespaces("Benkyo.Client.Services"))
    .AsSelf()
    .WithScopedLifetime());


    

builder.Services.AddScoped<Studyset>();
builder.Services.AddScoped<ColorOption>();
builder.Services.AddScoped<ColorOptions>();
builder.Services.AddTransient<User>();


await builder.Build().RunAsync();
