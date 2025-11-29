using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;

namespace VisionnementFilm.Infrastructure.Repositories
{
    public class CineDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Constructeur par défaut nécessaire pour les migrations
        public CineDbContext() { }

        // Constructeur pour passer des options (utile pour l'injection de dépendances)
        public CineDbContext(DbContextOptions<CineDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Chaîne de connexion par défaut (LocalDB) pour le développement
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TP2CineDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration basique des entités
            modelBuilder.Entity<Utilisateur>()
                .HasKey(u => u.Id); // Supposant que BaseEntity a une propriété Id

            modelBuilder.Entity<Film>()
                .HasKey(f => f.Id);

            // Exemple : Le prix doit avoir une précision spécifique pour l'argent
            modelBuilder.Entity<Film>()
                .Property(f => f.PrixAchat)
                .HasColumnType("decimal(18,2)");
            // Précision pour les montants financiers
            modelBuilder.Entity<Utilisateur>().Property(u => u.Solde).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Transaction>().Property(t => t.Montant).HasColumnType("decimal(18,2)");

        }
    }
}
