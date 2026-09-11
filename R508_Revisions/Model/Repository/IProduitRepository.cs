using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Model.Repository
{
    public interface IProduitRepository : IRepository<Produit>
    {
        Task<IEnumerable<Produit>> GetAllWithDetailsAsync();
        Task<Produit?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Produit>> GetByMarqueAsync(int idMarque);
        Task<IEnumerable<Produit>> SearchByNameAsync(string term);
    }
}
