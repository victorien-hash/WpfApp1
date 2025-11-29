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
    public class FilmRepository : EfRepository<Film>, IFilmRepository
    {
        public FilmRepository(CineDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Film>> RechercherParMotCleAsync(string motCle)
        {
            return await _dbContext.Films
                .Where(f => f.Titre.Contains(motCle) || f.Genre.Contains(motCle))
                .ToListAsync();
        }

        Task IFilmRepository.AddAsync(Film film)
        {
            return AddAsync(film);
        }
    }
}
