using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.Owner.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Owner.DAL.Data.Contexts
{
    internal class CompanyDbContext : DbContext
    {
        public CompanyDbContext() : base()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = .; Database = CompanyS03; Trusted_Connection = True; TrustServerCertificate = True")
        }

        public DbSet<Department> Departments { get; set; }
    }
}
