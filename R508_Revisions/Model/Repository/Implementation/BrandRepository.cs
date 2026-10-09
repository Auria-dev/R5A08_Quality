using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Model.Repository.Implementation
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(ProduitsDBContext context) : base(context) { }

        public async Task<Brand?> GetByNameAsync(string name)
        {
            return await _context.Marques.FirstOrDefaultAsync(m => m.nomMarque == name);
        }
    }
}

