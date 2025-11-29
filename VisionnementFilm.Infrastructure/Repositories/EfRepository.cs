using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Interfaces;
using VisionnementFilm.SharedKernel;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Infrastructure.Repositories
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly CineDbContext _dbContext;

        public EfRepository(CineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            var existingEntity = _dbContext.Set<T>().Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity != null)
            {
                // 2. Si on la trouve, on la détache. Le contexte arrête de la suivre.
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
            }
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Les méthodes CountAsync, ListAsync(spec) etc. peuvent être ajoutées ici si définies dans l'interface
        //public Task<int> CountAsync(ISpecification<T> spec) => throw new System.NotImplementedException();
        //public Task<IEnumerable<T>> ListAsync(ISpecification<T> spec) => throw new System.NotImplementedException();
    }
}
