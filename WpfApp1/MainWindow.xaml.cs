using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisionnementFilm.Core.Services;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AuthService _authService;
        private readonly FilmService _filmService;
        private readonly MembreService _membreService;

        public MainWindow(AuthService authService, FilmService filmService, MembreService membreService)
        {
            InitializeComponent();
            _authService = authService;
            _filmService = filmService;
            _membreService = membreService;
        }

        public MainWindow() { InitializeComponent(); }

        private async void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUsername.Text;
            string pass = txtPassword.Password;

            var utilisateurConnecte = await _authService.ConnecterAsync(user, pass);

            if (utilisateurConnecte != null)
            {
                if (utilisateurConnecte.EstAdmin)
                {
                    // --- CHANGEMENT ICI : On passe _authService aussi ---
                    var adminPage = new gerer_compte_admin(_authService, _filmService, _membreService, utilisateurConnecte);
                    adminPage.Show();
                }
                else
                {
                    // --- CHANGEMENT ICI : On passe _authService aussi ---
                    var accueilPage = new Accueil(_authService, _filmService, _membreService, utilisateurConnecte);
                    accueilPage.Show();
                }
                this.Close(); // On ferme la connexion car on en créera une nouvelle à la déconnexion
            }
            else
            {
                MessageBox.Show("Identifiants invalides.");
            }
        }

        private void BtnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionPage = new Inscription(_authService, this);
            inscriptionPage.Show();
            this.Hide();
        }
    }
}