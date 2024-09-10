using Microsoft.EntityFrameworkCore;
using PatientNote.Domain;
using MongoDB.EntityFrameworkCore.Extensions;

namespace PatientNote.Data
{
    public class MongoDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public MongoDbContext(DbContextOptions options): base(options) { }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            model.Entity<Note>().ToCollection("notes");
        }
    }
}
