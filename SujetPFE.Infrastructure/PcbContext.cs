using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;

public class PcbContext : IdentityDbContext
{
    public DbSet<Employe> Employes { get; set; }
    public DbSet<Direction> Directions { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Objectif> Objectifs { get; set; }
    public DbSet<HistoriqueObjectif> HistoriqueObjectifs { get; set; }  
    public PcbContext(DbContextOptions<PcbContext> options) : base(options)
    {
    }
  

}
