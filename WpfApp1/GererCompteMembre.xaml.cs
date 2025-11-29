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
    /// Logique d'interaction pour GererCompteMembre.xaml
    /// </summary>
    public partial class GererCompteMembre : Window
    {
        private readonly Utilisateur _currentUser;
        private readonly MembreService _memberService;
        private readonly Window _parentWindow;

        public GererCompteMembre(Utilisateur user, MembreService memberService, Window parentWindow)
        {
            InitializeComponent();
            _currentUser = user;
            _memberService = memberService;
            _parentWindow = parentWindow;

            // Initialisation des champs
            txtBienvenue.Text = $"Bonjour, {_currentUser.Nom}";

            // Pré-remplir les infos de modif
            txtEditNom.Text = _currentUser.Nom;
            txtEditPrenom.Text = _currentUser.Prenom;
            txtEditEmail.Text = _currentUser.Courriel;

            ChargerDonneesFinancieres();
        }

        private async void ChargerDonneesFinancieres()
        {
            // Mise à jour du solde
            decimal solde = await _memberService.GetSoldeAsync(_currentUser.Id);

            // On met à jour les textes dans les différents onglets
            txtSoldeDisplay.Text = $"{solde:C}";
            txtAboActuel.Text = string.IsNullOrEmpty(_currentUser.TypeAbonnement) ? "Aucun" : _currentUser.TypeAbonnement;

            // Mise à jour de la liste
            var transactions = await _memberService.GetHistoriqueAsync(_currentUser.Id);
            lstTransactions.ItemsSource = transactions;
        }

        // --- 1. Modifier profil ---
        private async void BtnModifierProfil_Click(object sender, RoutedEventArgs e)
        {
            _currentUser.Nom = txtEditNom.Text;
            _currentUser.Prenom = txtEditPrenom.Text;
            _currentUser.Courriel = txtEditEmail.Text;

            if (!string.IsNullOrWhiteSpace(txtEditPassword.Password))
            {
                _currentUser.MotDePasseHash = txtEditPassword.Password; // À hasher en vrai
            }

            await _memberService.MettreAJourInfoUtilisateurAsync(_currentUser);
            MessageBox.Show("Informations mises à jour avec succès !");
        }

        // --- 2. S'abonner ---
        private async void BtnAbonner_Click(object sender, RoutedEventArgs e)
        {
            if (cbAbonnement.SelectedItem is ComboBoxItem selectedItem)
            {
                string type = selectedItem.Content.ToString().Split('-')[0].Trim();
                decimal prix = decimal.Parse(selectedItem.Tag.ToString().Replace(".", ","));

                bool succes = await _memberService.PayerAbonnementAsync(_currentUser.Id, type, prix);

                if (succes)
                {
                    MessageBox.Show($"Félicitations ! Vous êtes abonné : {type}");
                    _currentUser.TypeAbonnement = type; // Mise à jour locale
                    ChargerDonneesFinancieres();
                }
                else
                {
                    MessageBox.Show("Solde insuffisant. Allez dans l'onglet 'Approvisionner compte'.");
                }
            }
        }

        // --- 3. Remboursement ---
        private async void BtnRemboursement_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtMontantRemboursement.Text, out decimal montant) && montant > 0)
            {
                string motif = txtMotifRemboursement.Text;
                // Appel au service
                bool succes = await _memberService.DemanderRemboursementAsync(_currentUser.Id, montant, motif);

                if (succes)
                {
                    MessageBox.Show("Remboursement approuvé et effectué.");
                    txtMontantRemboursement.Text = "";
                    txtMotifRemboursement.Text = "";
                    ChargerDonneesFinancieres();
                }
            }
            else
            {
                MessageBox.Show("Montant invalide.");
            }
        }

        // --- 4. Consulter solde (Bouton Refresh) ---
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ChargerDonneesFinancieres();
        }

        // --- 6. Approvisionner ---
        private async void BtnApprovisionner_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtMontantApprovisionnement.Text, out decimal montant) && montant > 0)
            {
                await _memberService.AjouterFondsAsync(_currentUser.Id, montant);
                MessageBox.Show("Paiement accepté. Votre solde a été mis à jour.");
                txtMontantApprovisionnement.Text = "";
                ChargerDonneesFinancieres();
            }
            else
            {
                MessageBox.Show("Veuillez entrer un montant valide.");
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show();
            this.Close();
        }
    }
}
