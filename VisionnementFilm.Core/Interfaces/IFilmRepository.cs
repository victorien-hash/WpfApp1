using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Interfaces
{
    public interface IFilmRepository: IAsyncRepository<Film>
    {
        Task AddAsync(Film film);
        Task DeleteAsync(Film film);
        Task<IEnumerable<Film>> ListAllAsync();
        Task<IEnumerable<Film>> RechercherParMotCleAsync(string motCle);
        Task UpdateAsync(Film film);
    }
}
