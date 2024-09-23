using System;
using Microsoft.EntityFrameworkCore;
using api_energy.Models;

namespace api_energy.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Periods> Periods { get; set; }
        public DbSet<Measurements> Measurements { get; set; }
        public DbSet<Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.user_id);
            modelBuilder.Entity<Periods>().HasKey(x => x.id);
            modelBuilder.Entity<Measurements>().HasKey(x => x.id);
            modelBuilder.Entity<Files>().HasKey(x => x.id);

            modelBuilder.Entity<Files>()
            .HasOne(m => m.Periods)
            .WithMany() 
            .HasForeignKey(m => m.id_period)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Periods>().HasMany(x => x.files).WithOne(m => m.Periods);


        }
    }
}

