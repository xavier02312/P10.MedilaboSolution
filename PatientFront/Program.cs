using PatientFront.Service;
using PatientService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(option =>
    {
        option.HtmlHelperOptions.ClientValidationEnabled = true;
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

// Enregistrement du service IAuthenticationServices
builder.Services.AddScoped<IAuthenticationServices, AuthenticationLogin>();

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
