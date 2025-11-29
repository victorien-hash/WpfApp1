using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.Core.Interfaces;

namespace VisionnementFilm.Core.Services
{
    public class FilmService
    {
        private readonly IFilmRepository _repo;

        public FilmService(IFilmRepository repo)
        {
            _repo = repo;
        }

        // Cas d'utilisation : Rechercher films
        public async Task<IEnumerable<Film>> RechercherFilmsAsync(string motCle)
        {
            if (string.IsNullOrWhiteSpace(motCle))
            {
                return await _repo.ListAllAsync();
            }
            return await _repo.RechercherParMotCleAsync(motCle);
        }

        // Cas d'utilisation : Consulter film (Récupérer tout)
        public async Task<IEnumerable<Film>> ObtenirTousLesFilmsAsync()
        {
            return await _repo.ListAllAsync();
        }

        // Cas d'utilisation Admin : Ajouter film acheté
        public async Task AjouterFilmAsync(Film film)
        {
            await _repo.AddAsync(film);
        }

        // Cas d'utilisation Admin : Enlever film
        public async Task SupprimerFilmAsync(Film film)
        {
            await _repo.DeleteAsync(film);
        }

        // Cas d'utilisation Admin : Modifier info film
        public async Task ModifierFilmAsync(Film film)
        {
            await _repo.UpdateAsync(film);
        }

        //public static implicit operator FilmService(AuthService v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
