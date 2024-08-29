using PatientFront.Service;
using PatientService.Service;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration de HttpClient 
builder.Services.AddHttpClient<PatientServiceApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7239"); // Adresse PatientService launchSettings.json
}).AddHttpMessageHandler(() => new BearerTokenHandler());

builder.Services.AddHttpClient<AuthenticationLogin>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7239"); // Adresse PatientService launchSettings.json
}).AddHttpMessageHandler(() => new BearerTokenHandler());

builder.Services.AddHttpClient<IAuthenticationServices, AuthenticationLogin>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7239"); // Adresse PatientService launchSettings.json
}).AddHttpMessageHandler(() => new BearerTokenHandler());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class BearerTokenHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Récupérer le token JWT depuis le stockage local ou session
        var token = await GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }

    private Task<string> GetTokenAsync()
    {
        // Implémentez la logique pour récupérer le token JWT
        return Task.FromResult("Jwt");
    }
}