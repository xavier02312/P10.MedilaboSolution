using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatientService.Data;
using PatientService.Repositories;
using PatientService.Service;
using Serilog;
using System.Text;

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

// JWT Bearer
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwt["SecretKey"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("organizer", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("organizer");
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })
    .AddPolicy("practitioner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("practitioner");
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    })
    .AddPolicy("organizerOrPractitioner", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
            context.User.IsInRole("organizer") || context.User.IsInRole("practitioner"));
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });

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