using Microsoft.EntityFrameworkCore;
using PatientService.Domain;

namespace PatientService.Data
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Patient> Patients { get; set; }
    }
}
