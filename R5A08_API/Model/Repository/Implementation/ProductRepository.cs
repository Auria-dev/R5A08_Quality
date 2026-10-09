using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Model.Repository.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ProduitsDBContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .FirstOrDefaultAsync(e => e.idProduit == id);
        }

        public async Task<IEnumerable<Product>> GetByBrandAsync(int brandId)
        {
            return await _context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .Where(e => e.idMarque == brandId)
                .ToListAsync();
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .FirstOrDefaultAsync(e => e.nomProduit == name);
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string term)
        {
            return await _context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .Where(e => EF.Functions.Like(e.nomProduit, $"%{term}%"))
                .ToListAsync();
        }

        public override async Task UpdateAsync(Product entity)
        {
            var existing = await _context.Produits.FindAsync(entity.idProduit);
            if (existing != null) 
                _context.Entry(existing).CurrentValues.SetValues(entity);
            else 
                _context.Produits.Update(entity);
            await SaveAsync();
        }
    }
}

