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

namespace WpfApp1
{
    
    /// Logique d'interaction pour VisionnerFilm.xaml
    
    public partial class VisionnerFilm : Window
    {
        private readonly Window _parentWindow;

        // Constructeur qui accepte le film sélectionné et la fenêtre parente (pour le retour)
        public VisionnerFilm(Film film, Window parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow;

            // Remplir l'interface avec les données du film
            txtTitreFilm.Text = film.Titre;
            txtGenre.Text = $"Genre : {film.Genre}";
            txtAnnee.Text = $"Année : {film.Annee}";
            txtDescription.Text = film.Description;
        }

        // Constructeur par défaut (juste pour éviter les erreurs XAML designer)
        public VisionnerFilm()
        {
            InitializeComponent();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.Show(); // Réafficher l'accueil
            this.Close();         // Fermer cette fenêtre
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            //si on ferme la fenêtre avec la croix, on s'assure que l'accueil revient
            _parentWindow.Show();
        }
    }
}
