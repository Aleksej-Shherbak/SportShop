using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domains.Entities;

namespace Domains.Concrete
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Product> Products { get; set; }

        public ApplicationContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Sportshop");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, b => b.MigrationsAssembly("Web"));
        }
    }
}