using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Model.Repository.Implementation
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(ProduitsDBContext context) : base(context) { }

        public async Task<ProductType?> GetByNameAsync(string name)
        {
            return await _context.TypeProduits.FirstOrDefaultAsync(t => t.nomTypeProduit == name);
        }
    }
}

