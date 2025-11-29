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
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        private readonly AuthService _authService;
        private readonly Window _loginWindow;

        // Constructeur qui reçoit le service
        public Inscription(AuthService authService, Window loginWindow)
        {
            InitializeComponent();
            _authService = authService;
            _loginWindow = loginWindow;
        }

        private async void BtnSInscrire_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation basique
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            if (txtPassword.Password != txtConfirmPass.Password)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.");
                return;
            }

            // 2. Création de l'entité
            var nouvelUtilisateur = new Utilisateur
            {
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                NomUtilisateur = txtUsername.Text,
                Courriel = txtEmail.Text,
                MotDePasseHash = txtPassword.Password, // À hasher en prod !
                EstAdmin = chkEstAdmin.IsChecked == true
            };

            // 3. Appel au service
            bool succes = await _authService.InscrireAsync(nouvelUtilisateur);

            if (succes)
            {
                MessageBox.Show("Compte créé avec succès ! Vous pouvez maintenant vous connecter.");
                _loginWindow.Show();
                this.Hide();

                // Retour à la connexion (on réouvre une nouvelle MainWindow proprement via App serait l'idéal, 
                // mais ici on va simplement fermer cette fenêtre pour l'exemple)
                this.Close();
            }
            else
            {
                MessageBox.Show("Erreur : Ce nom d'utilisateur est déjà pris.");
            }
        }

        private void BtnRetourConnexion_Click(object sender, MouseButtonEventArgs e)
        {
            _loginWindow.Show();
            this.Close();
        }

        private void BtnCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            // On passe 'this' (la fenêtre actuelle) pour pouvoir la réafficher plus tard
            var inscriptionPage = new Inscription(_authService, this);

            inscriptionPage.Show();
            this.Hide(); // On cache la connexion au lieu de la fermer
        }
    }
}
