using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Models;

namespace SujetPFE.Infrastructure;

public class PcbContext : IdentityDbContext
{
    public PcbContext(DbContextOptions<PcbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Objective> Objectives { get; set; }
    public DbSet<HistoriqueObjectif> HistoriqueObjectifs { get; set; }
    public DbSet<Pac> Pacs { get; set; }
    public DbSet<Groupe> Groupes { get; set; }
    public DbSet<KPIValue> KPIValues { get; set; } // Add this line
}