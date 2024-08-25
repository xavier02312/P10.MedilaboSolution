﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientService.Domain;

namespace PatientService.Data
{
    public class LocalDbContext : IdentityDbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>()
                .HasMany(g => g.Patients)
                .WithOne(g => g.Address);

            builder.Entity<Patient>()
                .HasOne(p => p.Address)
                .WithMany(a => a.Patients)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(p => p.AddressId);
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
