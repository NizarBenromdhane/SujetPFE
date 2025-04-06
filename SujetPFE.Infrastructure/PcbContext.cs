using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Models;

namespace SujetPFE.Infrastructure
{
    public class PcbContext : IdentityDbContext
    {
        public PcbContext(DbContextOptions<PcbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<HistoriqueObjectif> HistoriqueObjectifs { get; set; }
        public DbSet<Pac> Pacs { get; set; }

        public DbSet<ObjectifCreditDepot> ObjectifsCreditDepot { get; set; }

        public DbSet<ObjectifSuivi> ObjectifsSuivis { get; set; }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<SuiviIRC> SuivisIRC { get; set; }
        public DbSet<CompteRendu> ComptesRendus { get; set; }
        public DbSet<RDV> RDVs { get; set; }
        public DbSet<KPIValue> KPIValues { get; set; }

        // AJOUTEZ CETTE LIGNE :
        public DbSet<CreditObjectif> CreditObjectifs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Groupe)
                .WithMany()
                .HasForeignKey(c => c.GroupeId);
        }
    }
}