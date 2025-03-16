using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SujetPFE.Models;
using Microsoft.EntityFrameworkCore;

namespace SujetPFE.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
    }
}