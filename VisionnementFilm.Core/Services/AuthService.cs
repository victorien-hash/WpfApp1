using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisionnementFilm.Core.Entites;
using VisionnementFilm.Core.Interfaces;

namespace VisionnementFilm.Core.Services
{
    public class AuthService
    {
        private readonly IUtilisateurRepository _repo;

        // Injection de dépendance via le constructeur
        public AuthService(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        // Cas d'utilisation : Ouvrir session
        public async Task<Utilisateur?> ConnecterAsync(string nomUtilisateur, string motDePasse)
        {
            var utilisateur = await _repo.ObtenirParNomUtilisateurAsync(nomUtilisateur);

            // NOTE : En prod, utiliser un vérificateur de hash ici !
            if (utilisateur != null && utilisateur.MotDePasseHash == motDePasse)
            {
                return utilisateur;
            }
            return null;
        }

        // Cas d'utilisation : S'inscrire
        public async Task<bool> InscrireAsync(Utilisateur nouvelUtilisateur)
        {
            // Vérifier si l'utilisateur existe déjà
            var existant = await _repo.ObtenirParNomUtilisateurAsync(nouvelUtilisateur.NomUtilisateur);
            if (existant != null)
            {
                return false;
            }

            await _repo.AddAsync(nouvelUtilisateur);
            return true;
        }
    }
}
