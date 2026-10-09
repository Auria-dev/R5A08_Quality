using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Repository.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<Product?> GetByNameAsync(string name);
        Task<IEnumerable<Product>> GetByBrandAsync(int brandId);
        Task<IEnumerable<Product>> SearchByNameAsync(string term);
    }
}

