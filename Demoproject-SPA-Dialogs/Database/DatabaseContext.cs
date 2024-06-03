using Microsoft.EntityFrameworkCore;
using System.Net;
using System;
using System.Reflection;

namespace Demoproject_SPA_Dialogs.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Calculation> Calculations { get; set; }
        public DbSet<Costrecord> Costrecords { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calculation>().ToTable("Calculation");
            modelBuilder.Entity<Costrecord>().ToTable("Costrecord");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
