using PatientFront.Service;
using PatientService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(option =>
    {
        option.HtmlHelperOptions.ClientValidationEnabled = true;
    });

// Ajout du cache en m�moire distribu�
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Dur�e de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Ajouter IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configuration de HttpClient 
builder.Services.AddHttpClient<PatientServiceApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7239"); // Adresse PatientService launchSettings.json
});

builder.Services.AddHttpClient<AuthenticationLogin>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7239"); // Adresse PatientService launchSettings.json
});

builder.Services.AddHttpClient<PatientNoteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7078"); // Adress PatientNote launchSettings.json
});

builder.Services.AddHttpClient<PatientRapportDiabeteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7234"); // Adress PatientRapportDiabete launchSetting.json
});

// Enregistrement du service IAuthenticationServices
builder.Services.AddScoped<IAuthenticationServices, AuthenticationLogin>();

// Configuration de l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
    });

// Fournisseurs de journalisation
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Page 404
    app.UseStatusCodePagesWithReExecute("/Home/Error{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
// Ajout de l'utilisation des sessions pour le token
app.UseSession();
// Ajout pour l'Authentication
app.UseAuthentication();

app.UseAuthorization();

// Ajouter le middleware personnalis� Authentication
app.UseMiddleware<RedirectUnauthorizedMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();