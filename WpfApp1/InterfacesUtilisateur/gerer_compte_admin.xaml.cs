using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.Core.Services;

namespace WpfApp1
{
    public partial class gerer_compte_admin : Window
    {
        private readonly AuthService _authService;
        private readonly FilmService _filmService;
        private readonly MembreService _memberService;
        private readonly Utilisateur _currentUser;

        private int _filmIdEnModification = -1;

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

        // --- BROWSE IMAGE ---

        private void BtnParcourirImageAjouter_Click(object sender, RoutedEventArgs e)
        {
            var path = OuvrirDialogImage();
            if (!string.IsNullOrEmpty(path))
            {
                AjouterCheminImageTextBox.Text = path;
            }
        }

        private void BtnParcourirImageModif_Click(object sender, RoutedEventArgs e)
        {
            var path = OuvrirDialogImage();
            if (!string.IsNullOrEmpty(path))
            {
                ModifCheminImageTextBox.Text = path;
            }
        }

        private string OuvrirDialogImage()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Sélectionner une image",
                Filter = "Images (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Tous les fichiers|*.*"
            };

            bool? result = dlg.ShowDialog();
            return result == true ? dlg.FileName : string.Empty;
        }

        // --- GESTION FILMS (CRUD) ---

        private async void BtnAjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AjouterTitreTextBox.Text))
            {
                MessageBox.Show("Le titre est obligatoire.");
                return;
            }

            var cheminImage = !string.IsNullOrWhiteSpace(AjouterCheminImageTextBox.Text)
                ? AjouterCheminImageTextBox.Text
                : "/images/fastfurious.jpg"; // valeur par défaut

            var film = new Film
            {
                Titre = AjouterTitreTextBox.Text,
                Genre = AjouterGenreComboBox.Text,
                Description = AjouterDescriptionTextBox.Text,
                Annee = int.TryParse(AjouterAnneeTextBox.Text, out int annee) ? annee : 2024,
                CheminImage = cheminImage,
                PrixAchat = 10.0m
            };

            await _filmService.AjouterFilmAsync(film);
            MessageBox.Show("Film ajouté avec succès !");

            AjouterTitreTextBox.Text = "";
            AjouterDescriptionTextBox.Text = "";
            AjouterCheminImageTextBox.Text = "";
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
                ModifCheminImageTextBox.Text = film.CheminImage ?? string.Empty;
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

            var cheminImage = !string.IsNullOrWhiteSpace(ModifCheminImageTextBox.Text)
                ? ModifCheminImageTextBox.Text
                : "/images/fastfurious.jpg";

            var film = new Film
            {
                Id = _filmIdEnModification,
                Titre = ModifTitreTextBox.Text,
                Genre = ModifGenreComboBox.Text,
                Description = ModifDescriptionTextBox.Text,
                Annee = 2024,
                CheminImage = cheminImage,
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