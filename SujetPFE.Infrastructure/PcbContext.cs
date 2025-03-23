using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using System.Security.AccessControl;

namespace SujetPFE.Infrastructure // Adjust namespace if needed
{
    public class PcbContext : IdentityDbContext
    {
        public PcbContext(DbContextOptions<PcbContext> options) : base(options)
        {
        }

        // Corrected DbSet properties:
        public DbSet<Employee> Employees { get; set; } // Corrected name
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Objectif> Objectives { get; set; } // Corrected name
        public DbSet<HistoriqueObjectif> HistoriqueObjectifs { get; set; }
    }
}