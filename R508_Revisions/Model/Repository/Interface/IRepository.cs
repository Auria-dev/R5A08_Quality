using Microsoft.AspNetCore.Mvc;
using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<ActionResult<IEnumerable<Produit>>> GetAllAsync();
        Task<ActionResult<T>> GetByIdAsync(int id);
        Task<ActionResult<T>> GetByStringAsync(string str);
        Task AddAsync(T entity);
        Task UpdateAsync(T entityToUpdate, T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}