using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Repository.Implementation
{
    public class ProduitRepository(ProduitsDBContext context) : IProduitRepository
    {
        public async Task AddAsync(Produit entity)
        {
            await context.AddAsync(entity);
            await SaveAsync();        }

        public async Task DeleteAsync(Produit entity)
        {
            context.Produits.Remove(entity);
            await SaveAsync();
        }

        public async Task<ActionResult<IEnumerable<Produit>>> GetAllAsync()
        {
            return await context.Produits.ToListAsync();
        }

        public async Task<IEnumerable<Produit>> GetAllWithDetailsAsync()
        {
            return await context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .ToListAsync();
        }

        public async Task<ActionResult<Produit>> GetByIdAsync(int id)
        {
            return await context.Produits.FirstOrDefaultAsync(e => e.idProduit == id);
        }

        public async Task<Produit?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Produits
                .Include(p => p.idMarqueNavigation)
                .Include(p => p.idTypeProduitNavigation)
                .FirstOrDefaultAsync(e => e.idProduit == id);
        }

        public async Task<IEnumerable<Produit>> GetByMarqueAsync(int idMarque)
        {
            return await context.Produits.Where(e => e.idMarque == idMarque).ToListAsync();
        }

        public async Task<ActionResult<Produit>> GetByStringAsync(string str)
        {
            return await context.Produits.FirstOrDefaultAsync(e => e.nomProduit == str);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Produit>> SearchByNameAsync(string term)
        {
            return await context.Produits
                .Where(e => EF.Functions.Like(e.nomProduit, $"%{term}%"))
                .ToListAsync();
        }

        public async Task UpdateAsync(Produit entityOld, Produit entityNew)
        {
            context.Entry(entityOld).State = EntityState.Modified;
            entityOld.nomProduit = entityNew.nomProduit;
            entityOld.description = entityNew.description;
            entityOld.nomPhoto = entityNew.nomPhoto;
            entityOld.uriPhoto = entityNew.uriPhoto;
            entityOld.idTypeProduit = entityNew.idTypeProduit;
            entityOld.idMarque = entityNew.idMarque;
            entityOld.stockReel = entityNew.stockReel;
            entityOld.stockMin = entityNew.stockMin;
            entityOld.stockMax = entityNew.stockMax;
            await SaveAsync();
        }
    }
}
