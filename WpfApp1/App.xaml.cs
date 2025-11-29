using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using VisionnementFilm.Core.Services;
using VisionnementFilm.Infrastructure.Repositories;
using VisionnementFilm.Core.Entites; // Ajout de l'import pour Transaction

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CineDbContext _dbContext;
        private AuthService _authService;
        private FilmService _filmService;
        private  MembreService _membreService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Configurer la base de données (SQL Server LocalDB pour le dev)
            var optionsBuilder = new DbContextOptionsBuilder<CineDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TP2CineDB;Trusted_Connection=True;MultipleActiveResultSets=true");

            _dbContext = new CineDbContext(optionsBuilder.Options);


            //_dbContext.Database.EnsureDeleted();   //suppression temporaire
            // 2. Créer la base de données si elle n'existe pas (Code First simple)
            _dbContext.Database.EnsureCreated();

            // 3. Initialiser les Repositories
            var utilisateurRepo = new UtilisateurRepository(_dbContext);
            var filmRepo = new FilmRepository(_dbContext);

            // 4. Initialiser les Services (Injection des repos)
            _authService = new AuthService(utilisateurRepo);
            _filmService = new FilmService(filmRepo);

            // Code temporaire dans App.xaml.cs pour créer un admin
            if (!_dbContext.Utilisateurs.Any(u => u.NomUtilisateur == "admin"))
            {
                var admin = new VisionnementFilm.Core.Entites.Utilisateur
                {
                    NomUtilisateur = "admin",
                    MotDePasseHash = "admin123",
                    EstAdmin = true, // <--- C'est ici que vous définissez le rôle
                    Courriel = "admin@cine.com"
                };
                _dbContext.Utilisateurs.Add(admin);
                _dbContext.SaveChanges();
            }

            // Créer le Repo générique pour Transaction
            var transactionRepo = new EfRepository<Transaction>(_dbContext);

            // Initialiser le MemberService
            _membreService = new MembreService(utilisateurRepo, transactionRepo); // Note: utilisateurRepo doit être accessible ici


            // 5. Lancer la fenêtre de connexion avec le service injecté
            var loginWindow = new MainWindow(_authService, _filmService, _membreService); // On passe aussi FilmService si besoin plus tard
            loginWindow.Show();
        }

        // Nettoyage à la fermeture
        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext.Dispose();
            base.OnExit(e);
        }
    }

}
