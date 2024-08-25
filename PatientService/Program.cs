using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Repositories;
using PatientService.Service;
using Serilog;

namespace PatientService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Connexion BDD
            builder.Services.AddDbContext<LocalDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Patient-Gestion")));

            // Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            builder.Services.AddHttpContextAccessor();

            // Service configuration
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientServices, PatientServices>();
            builder.Services.AddScoped<IAdressRepository, AdressRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
