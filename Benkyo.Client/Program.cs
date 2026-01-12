using Benkyo.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudysetService>();
builder.Services.AddScoped<FlashcardService>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddTransient<User>();


await builder.Build().RunAsync();
