using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.SharedKernel;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Entites
{
    public class Utilisateur: BaseEntity, IAggregateRoot
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string NomUtilisateur { get; set; } = string.Empty;
        public string Courriel { get; set; } = string.Empty;
        public string MotDePasseHash { get; set; } = string.Empty;
        public bool EstAdmin { get; set; } = false;
        public decimal Solde { get; set; } = 0.0m;
        public string TypeAbonnement { get; set; } = "Aucun";
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();  //Relation One-to-Many avec les transactions


        // Constructeur 
        public Utilisateur() { }

        public Utilisateur(string nomUtilisateur, string courriel)
        {
            NomUtilisateur = nomUtilisateur;
            Courriel = courriel;
        }
    }
}
