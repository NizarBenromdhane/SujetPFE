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
        public DbSet<ObjetVisite> ObjetsVisite { get; set; }
        public DbSet<CreditObjectif> CreditObjectifs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration Client-Groupe
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Groupe)
                .WithMany()
                .HasForeignKey(c => c.GroupeId);

            // Configuration RDV-SuiviIRC
            modelBuilder.Entity<RDV>()
                .HasOne(r => r.SuiviIRC)
                .WithMany(s => s.RDVs)
                .HasForeignKey(r => r.SuiviIRCId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration RDV-CompteRendu (1-1)
            modelBuilder.Entity<RDV>()
                .HasOne(r => r.CompteRendu)
                .WithOne(c => c.RDV)
                .HasForeignKey<CompteRendu>(c => c.RDVId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration ObjetVisite-RDV
            modelBuilder.Entity<ObjetVisite>()
                .HasOne(ov => ov.RDV)
                .WithMany(r => r.ObjetsVisite)
                .HasForeignKey(ov => ov.RDVId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration SuiviIRC-Client
            modelBuilder.Entity<SuiviIRC>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration pour CreditObjectif si nécessaire
            modelBuilder.Entity<CreditObjectif>(entity =>
            {
                // Exemple de configuration - adapter selon vos besoins
                entity.HasKey(e => e.Id);
                // Autres configurations...
            });
        }
    }
}