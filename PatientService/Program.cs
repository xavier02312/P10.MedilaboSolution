using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Repositories;
using PatientService.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connexion BDD
builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Patient-Gestion")));

 builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<LocalDbContext>()
        .AddDefaultTokenProviders();

// Serilog
Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

builder.Services.AddHttpContextAccessor();

// Service configuration
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientServices, PatientServices>();
builder.Services.AddScoped<IAdressRepository, AdressRepository>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbcontext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
    var authService = scope.ServiceProvider.GetService<IAuthenticationServices>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var succeed = dbcontext.Database.EnsureCreated();
        
        if (succeed)
        {
            var seeder = new DatabaseGestionRoles(userManager, roleManager);
            await seeder.EnsureAdminIsCreated();
            await seeder.EnsurePractitionerIsCreated();
            await seeder.EnsureAdminIsCreated();
        }

}

app.UseHttpsRedirection();

app.UseAuthorization();
// Identity
app.UseAuthentication();

app.MapControllers();

app.Run();