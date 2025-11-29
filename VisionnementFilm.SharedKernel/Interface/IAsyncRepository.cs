using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionnementFilm.SharedKernel.Interface
{
    public interface IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();


        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
