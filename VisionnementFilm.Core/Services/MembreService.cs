using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Services
{
    public class MembreService
    {
        private readonly IAsyncRepository<Utilisateur> _userRepo;
        private readonly IAsyncRepository<Transaction> _transactionRepo;

        public MembreService(IAsyncRepository<Utilisateur> userRepo, IAsyncRepository<Transaction> transactionRepo)
        {
            _userRepo = userRepo;
            _transactionRepo = transactionRepo;
        }

        public async Task<decimal> GetSoldeAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            return user?.Solde ?? 0;
        }

        public async Task AjouterFondsAsync(int userId, decimal montant)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user != null)
            {
                user.Solde += montant;
                await _userRepo.UpdateAsync(user);

                // Enregistrer la transaction
                await _transactionRepo.AddAsync(new Transaction
                {
                    UtilisateurId = userId,
                    DateTransaction = DateTime.Now,
                    Type = "Dépôt",
                    Montant = montant
                });
            }
        }

        public async Task<bool> PayerAbonnementAsync(int userId, string typeAbo, decimal prix)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null || user.Solde < prix) return false; // Solde insuffisant

            user.Solde -= prix;
            user.TypeAbonnement = typeAbo;
            await _userRepo.UpdateAsync(user);

            await _transactionRepo.AddAsync(new Transaction
            {
                UtilisateurId = userId,
                DateTransaction = DateTime.Now,
                Type = $"Abonnement {typeAbo}",
                Montant = -prix // Montant négatif pour indiquer une dépense
            });

            return true;
        }


        public async Task MettreAJourInfoUtilisateurAsync(Utilisateur utilisateurModifie)
        {
            await _userRepo.UpdateAsync(utilisateurModifie);
        }

        // Méthode pour traiter un remboursement
        public async Task<bool> DemanderRemboursementAsync(int userId, decimal montant, string motif)
        {
            

            var user = await _userRepo.GetByIdAsync(userId);
            if (user != null)
            {
                user.Solde += montant; // On remet l'argent
                await _userRepo.UpdateAsync(user);

                await _transactionRepo.AddAsync(new Transaction
                {
                    UtilisateurId = userId,
                    DateTransaction = DateTime.Now,
                    Type = "Remboursement",
                    Montant = montant // Positif car l'argent revient
                });
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<Transaction>> GetHistoriqueAsync(int userId)
        {
            var allTransactions = await _transactionRepo.ListAllAsync();
            return allTransactions.Where(t => t.UtilisateurId == userId).OrderByDescending(t => t.DateTransaction);
        }
    }
}
