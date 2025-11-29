using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.Core.Services;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour gerer_compte_admin.xaml
    /// </summary>
    public partial class gerer_compte_admin : Window
    {
        private readonly AuthService _authService;
        private readonly FilmService _filmService;
        private readonly MembreService _memberService;
        private readonly Utilisateur _currentUser;

        private int _filmIdEnModification = -1;

        // Constructeur complet
        public gerer_compte_admin(AuthService authService, FilmService filmService, MembreService memberService, Utilisateur currentUser)
        {
            InitializeComponent();
            _authService = authService;
            _filmService = filmService;
            _memberService = memberService;
            _currentUser = currentUser;
        }

        // --- NAVIGATION ---

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow(_authService, _filmService, _memberService);
            loginWindow.Show();
            this.Close();
        }

        private void BtnRetourAccueil_Click(object sender, RoutedEventArgs e)
        {
            var accueilWindow = new Accueil(_authService, _filmService, _memberService, _currentUser);
            accueilWindow.Show();
            this.Close();
        }

        // --- GESTION FILMS (CRUD) ---

        private async void BtnAjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AjouterTitreTextBox.Text))
            {
                MessageBox.Show("Le titre est obligatoire.");
                return;
            }

            var film = new Film
            {
                Titre = AjouterTitreTextBox.Text,
                Genre = AjouterGenreComboBox.Text,
                Description = AjouterDescriptionTextBox.Text,
                Annee = int.TryParse(AjouterAnneeTextBox.Text, out int annee) ? annee : 2024,

                // Valeurs par défaut pour éviter l'erreur SQL
                CheminImage = "/images/fastfurious.jpg",
                //CheminVideo = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                PrixAchat = 10.0m
            };

            await _filmService.AjouterFilmAsync(film);
            MessageBox.Show("Film ajouté avec succès !");

            AjouterTitreTextBox.Text = "";
            AjouterDescriptionTextBox.Text = "";
        }

        private async void BtnChargerFilm_Click(object sender, RoutedEventArgs e)
        {
            var titre = ModifTitreRechercheBox.Text;
            var films = await _filmService.RechercherFilmsAsync(titre);
            var film = films.FirstOrDefault(f => f.Titre.Equals(titre, StringComparison.OrdinalIgnoreCase));

            if (film != null)
            {
                _filmIdEnModification = film.Id;
                ModifTitreTextBox.Text = film.Titre;
                ModifDescriptionTextBox.Text = film.Description;
                ModifGenreComboBox.Text = film.Genre;
                MessageBox.Show("Film trouvé. Vous pouvez modifier.");
            }
            else
            {
                MessageBox.Show("Film introuvable.");
                _filmIdEnModification = -1;
            }
        }

        private async void BtnEnregistrerModif_Click(object sender, RoutedEventArgs e)
        {
            if (_filmIdEnModification == -1)
            {
                MessageBox.Show("Veuillez d'abord charger un film.");
                return;
            }

            // Note: Pour éviter l'erreur de tracking, assurez-vous d'utiliser UpdateAsync corrigé dans EfRepository
            var film = new Film
            {
                Id = _filmIdEnModification,
                Titre = ModifTitreTextBox.Text,
                Genre = ModifGenreComboBox.Text,
                Description = ModifDescriptionTextBox.Text,
                Annee = 2024,
                CheminImage = "/images/fastfurious.jpg", // Garder l'image ou récupérer l'ancienne
                //CheminVideo = "http://...",
                PrixAchat = 10.0m
            };

            await _filmService.ModifierFilmAsync(film);
            MessageBox.Show("Film modifié avec succès !");
        }

        private async void BtnSupprimerFilm_Click(object sender, RoutedEventArgs e)
        {
            var titre = SupprTitreTextBox.Text;
            var films = await _filmService.RechercherFilmsAsync(titre);
            var film = films.FirstOrDefault(f => f.Titre.Equals(titre, StringComparison.OrdinalIgnoreCase));

            if (film != null)
            {
                await _filmService.SupprimerFilmAsync(film);
                MessageBox.Show("Film supprimé.");
            }
            else
            {
                MessageBox.Show("Film introuvable.");
            }
        }
    }
}
