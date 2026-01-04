using Benkyo.Client.Pages;
using Benkyo.Client.Services;
using Benkyo.Components;
using Benkyo.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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
builder.Services.AddSingleton<FirebaseAuthentication>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<FirebaseService>();


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
