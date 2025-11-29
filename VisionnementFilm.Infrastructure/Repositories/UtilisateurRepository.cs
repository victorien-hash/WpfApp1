using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.Core.Interfaces;

namespace VisionnementFilm.Infrastructure.Repositories
{
    public class UtilisateurRepository : EfRepository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(CineDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Utilisateur?> ObtenirParNomUtilisateurAsync(string nomUtilisateur)
        {
            return await _dbContext.Utilisateurs
                .FirstOrDefaultAsync(u => u.NomUtilisateur == nomUtilisateur);
        }

        Task IUtilisateurRepository.AddAsync(Utilisateur nouvelUtilisateur)
        {
            return AddAsync(nouvelUtilisateur);
        }
    }
}
