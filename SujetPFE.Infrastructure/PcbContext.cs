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

        // ✅ Utilise ce nom dans ton contrôleur : _context.ObjectifsCreditDepots
        public DbSet<ObjectifCreditDepot> ObjectifsCreditDepots { get; set; }

        public DbSet<ObjectifSuivi> ObjectifsSuivis { get; set; }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<SuiviIRC> SuivisIRC { get; set; }
        public DbSet<CompteRendu> ComptesRendus { get; set; }
        public DbSet<RDV> RDVs { get; set; }
        public DbSet<KPIValue> KPIValues { get; set; }
        public DbSet<ObjetVisite> ObjetsVisite { get; set; }
        public DbSet<CreditObjectif> CreditObjectifs { get; set; }
        public DbSet<TemplateClient> TemplateClients { get; set; }
        public DbSet<PratiquesManagériales> PratiquesManagériales { get; set; }
        public DbSet<Encours> Encours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Groupe)
                .WithMany()
                .HasForeignKey(c => c.GroupeId);

            modelBuilder.Entity<RDV>()
                .HasOne(r => r.SuiviIRC)
                .WithMany(s => s.RDVs)
                .HasForeignKey(r => r.SuiviIRCId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RDV>()
                .HasOne(r => r.CompteRendu)
                .WithOne(c => c.RDV)
                .HasForeignKey<CompteRendu>(c => c.RDVId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ObjetVisite>()
                .HasOne(ov => ov.RDV)
                .WithMany(r => r.ObjetsVisite)
                .HasForeignKey(ov => ov.RDVId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SuiviIRC>()
                .HasOne(s => s.Client)
                .WithMany()
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreditObjectif>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MontantObjectif).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<KPIValue>()
                .HasOne(k => k.Pac)
                .WithMany(p => p.KPIValues)
                .HasForeignKey(k => k.PacId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Encours>()
                .HasOne(e => e.Employee)
                .WithMany(emp => emp.Encours)
                .HasForeignKey(e => e.EmployeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Encours>()
                .HasOne(e => e.Groupe)
                .WithMany()
                .HasForeignKey(e => e.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ObjectifCreditDepot>()
                .Property(o => o.Montant).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ObjectifCreditDepot>()
                .Property(o => o.MontantDat).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ObjectifCreditDepot>()
                .Property(o => o.MontantDav).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ObjectifSuivi>()
                .Property(o => o.Ecart).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ObjectifSuivi>()
                .Property(o => o.Realisation).HasColumnType("decimal(18, 2)");
        }
    }
}
