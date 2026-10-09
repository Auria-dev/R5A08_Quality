using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Repository.Interface
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        Task<ProductType?> GetByNameAsync(string name);
    }
}

