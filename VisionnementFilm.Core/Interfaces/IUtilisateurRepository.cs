using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Interfaces
{
    public interface IUtilisateurRepository: IAsyncRepository<Utilisateur>
    {
        Task AddAsync(Utilisateur nouvelUtilisateur);
        Task<Utilisateur?> ObtenirParNomUtilisateurAsync(string nomUtilisateur);
        
    }
}
