using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Repository.Interface
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand?> GetByNameAsync(string name);
    }
}

