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
    /// Logique d'interaction pour Accueil.xaml
    
    public partial class Accueil : Window
    {
        private readonly AuthService _authService;
        private readonly FilmService _filmService;
        private readonly MembreService _memberService;
        private readonly Utilisateur _currentUser;

        
        public Accueil(AuthService authService, FilmService filmService, MembreService membreService, Utilisateur currentUser)
        {
            InitializeComponent();
            _authService = authService;
            _filmService = filmService;
            _memberService = membreService;
            _currentUser = currentUser;


            // Pour permettre l'accès à l'admin seulement si l'utilisateur est admin
            if (_currentUser.EstAdmin == true)
            {
                // Si admin, le bouton est visible
                BtnNavAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                // Sinon, le bouton est masqué et ne prend pas de place (Collapsed)
                BtnNavAdmin.Visibility = Visibility.Collapsed;
            }
            // ---------------------------

            this.Loaded += Accueil_Loaded;
        }

        private void Accueil_Loaded(object sender, RoutedEventArgs e)
        {
            // Afficher le catalogue complet au chargement
            ChargerFilms("");
        }

        // --- NAVIGATION ---

        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Retour à la case départ (Nouvelle MainWindow avec les services)
            var loginWindow = new MainWindow(_authService, _filmService, _memberService);
            loginWindow.Show();
            this.Close();
        }

        private void BtnNavAdmin_Click(object sender, RoutedEventArgs e)
        {
            // Pour permettre à un membre d'aller vers admin (seulement s'il a les droits normalement)
            
            var adminWindow = new gerer_compte_admin(_authService, _filmService, _memberService, _currentUser);
            adminWindow.Show();
            this.Close();
        }

        private void BtnProfil_Click(object sender, RoutedEventArgs e)
        {
            // Ouvre la gestion de compte en passant l'utilisateur courant et le service
            var gestionMembre = new GererCompteMembre(_currentUser, _memberService, this);
            gestionMembre.Show();
        }

        // --- LOGIQUE FILMS ---

        private async void ChargerFilms(string motCle)
        {
            ListeFilms.Children.Clear();

            IEnumerable<Film> films;
            if (string.IsNullOrWhiteSpace(motCle) || motCle == "Rechercher un film...")
            {
                films = await _filmService.ObtenirTousLesFilmsAsync();
                if (txtTitreSection != null) txtTitreSection.Text = "🎬 Tous les films";
            }
            else
            {
                films = await _filmService.RechercherFilmsAsync(motCle);
                if (txtTitreSection != null) txtTitreSection.Text = $"🔍 Résultats pour : {motCle}";
            }

            foreach (var film in films)
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(46, 46, 46)),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(10),
                    Width = 150,
                    Height = 220,
                    Cursor = Cursors.Hand,
                    Tag = film
                };

                border.MouseLeftButtonUp += CarteFilm_Click;

                var stackPanel = new StackPanel();

                var imageSource = string.IsNullOrEmpty(film.CheminImage)
                   ? "https://via.placeholder.com/150x200?text=No+Image"
                   : film.CheminImage;

                var image = new Image
                {
                    Source = new BitmapImage(new System.Uri(imageSource, System.UriKind.RelativeOrAbsolute)),
                    Height = 180,
                    Stretch = Stretch.UniformToFill
                };

                var textBlock = new TextBlock
                {
                    Text = film.Titre,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0),
                    TextTrimming = TextTrimming.CharacterEllipsis
                };

                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                border.Child = stackPanel;

                ListeFilms.Children.Add(border);
            }
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            ChargerFilms(txtRecherche.Text);
        }

        private void CarteFilm_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Film film)
            {
                var videoWindow = new VisionnerFilm(film, this);
                videoWindow.Show();
                this.Hide();
            }
        }

        private void TxtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtRecherche.Text == "Rechercher un film...")
            {
                txtRecherche.Text = "";
                txtRecherche.Foreground = Brushes.White;
            }
        }

        private void TxtRecherche_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecherche.Text))
            {
                txtRecherche.Text = "Rechercher un film...";
                txtRecherche.Foreground = Brushes.Gray;
            }
        }
    }
}
