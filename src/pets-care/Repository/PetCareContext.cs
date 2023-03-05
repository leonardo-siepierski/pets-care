using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace pets_care.Models
{
    public class PetCareContext : DbContext
    {
        public PetCareContext(DbContextOptions<PetCareContext> options) : base(options) {}

        public PetCareContext() {}

        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Pet> Pets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@$"
                    Server={Environment.GetEnvironmentVariable("DBCONNECTION")};
                    Database={Environment.GetEnvironmentVariable("DBNAME")};
                    User={Environment.GetEnvironmentVariable("DBUSER")};
                    Password={Environment.GetEnvironmentVariable("DBPASSWORD")};
                ");
            }
        }
    }
}