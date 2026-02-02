using Benkyo.Client.Pages;
using Benkyo.Client.Services;
using Benkyo.Components;
using Benkyo.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

string credentialPath = @"C:\Users\CBB\source\repos\Benkyo\Benkyo\benkyoConfig.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

//Initialize the Engine for once
if(FirebaseApp.DefaultInstance == null)
{
    var firebaseApp = FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.GetApplicationDefault()
    });
    builder.Services.AddSingleton(firebaseApp);
} else
{
    builder.Services.AddSingleton(FirebaseApp.DefaultInstance);
}

    // Register FirestoreDB as a singleton service
    builder.Services.AddSingleton(s => FirestoreDb.Create("benkyo-9a049"));

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<FirebaseAuthentication>();
// Register server services not included the FirebaseAuthentication for the scoped lifetime
builder.Services.Scan(scan => scan
    .FromAssemblyOf<FirebaseService>()
    .AddClasses(classes => classes.InNamespaces("Benkyo.Services")
    .Where(c => c.Name != "FirebaseAuthentication"))
    .AsSelf()
    .WithScopedLifetime());


// Register Client services for the scoped lifetime to avoid server exceptions
builder.Services.Scan(scan => scan
    .FromAssemblyOf<AuthService>()
    .AddClasses(classes => classes.InNamespaces("Benkyo.Client.Services"))
    .AsSelf()
    .WithScopedLifetime());
    

builder.Services.AddScoped<Studyset>();
builder.Services.AddScoped<Flashcard>();
builder.Services.AddTransient<User>();
builder.Services.AddScoped<ColorOption>();
builder.Services.AddScoped<ColorOptions>();

builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Benkyo.Client._Imports).Assembly);

app.Run();
