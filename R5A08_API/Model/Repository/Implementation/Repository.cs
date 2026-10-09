using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Model.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ProduitsDBContext _context;

        public Repository(ProduitsDBContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAsync();
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
