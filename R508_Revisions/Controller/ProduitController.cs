using Microsoft.AspNetCore.Mvc;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository;
using R508_Revisions.Model.Repository.Implementation;
using System.Net.Http.Headers;

namespace R508_Revisions.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduitController (IProduitRepository manager): ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProduitDto>> PostProduit(Produit entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await manager.AddAsync(entity);

            var createdProduit = await manager.GetByIdWithDetailsAsync(entity.idProduit);

            var dto = new ProduitDto
            {
                Id = createdProduit!.idProduit,
                Nom = createdProduit.nomProduit,
                Type = createdProduit.idTypeProduitNavigation?.nomTypeProduit,
                Marque = createdProduit.idMarqueNavigation?.nomMarque
            };

            return CreatedAtAction(nameof(GetProduitById), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            ActionResult<Produit> utilisateur = await manager.GetByIdAsync(id);
            if (utilisateur == null) return NotFound();
            await manager.DeleteAsync(utilisateur.Value);
            return NoContent();
        }

        [HttpGet]
        [ActionName("GetProduits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetAllProduit()
        {
            var produits = await manager.GetAllWithDetailsAsync();
            var res = produits.Select(p => new ProduitDto
            {
                Id= p.idProduit,
                Nom = p.nomProduit,
                Type = p.idTypeProduitNavigation.nomTypeProduit,
                Marque = p.idMarqueNavigation.nomMarque,
            });

            return Ok(res);
        }

        [HttpGet]
        [Route("[action]/{idProduit}")]
        [ActionName("GetProduitsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDto>> GetProduitById(int id)
        {
            var produit = await manager.GetByIdWithDetailsAsync(id);
            if (produit == null) return NotFound();

            var dto = new ProduitDetailDto
            {
                Id = produit.idProduit,
                Nom = produit.nomProduit,
                Description = produit.description,
                Nomphoto = produit.nomPhoto,
                Uriphoto = produit.uriPhoto,
                Type = produit.idTypeProduitNavigation?.nomTypeProduit,
                Marque = produit.idMarqueNavigation?.nomMarque,
                Stock = produit.stockReel,
                EnReappro = produit.stockReel < produit.stockMin
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("[action]/{nomProduit}")]
        [ActionName("GetProduitsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetProduitByString(string str)
        {
            var result = await manager.GetByStringAsync(str);
            var produit = result.Value;
            if (result == null || produit == null) return NotFound();

            var dto = new ProduitDetailDto
            {
                Id = produit.idProduit,
                Nom = produit.nomProduit,
                Description = produit.description,
                Nomphoto = produit.nomPhoto,
                Uriphoto = produit.uriPhoto,
                Type = produit.idTypeProduitNavigation?.nomTypeProduit,
                Marque = produit.idMarqueNavigation?.nomMarque,
                Stock = produit.stockReel,
                EnReappro = produit.stockReel < produit.stockMin
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("[action]/{idMarque}")]
        [ActionName("GetProduitsByMarque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetByMarque(int idMarque)
        {
            var produits = await manager.GetByMarqueAsync(idMarque);

            if (produits == null || !produits.Any()) return NotFound();

            var dtos = produits.Select(p => new ProduitDto
            {
                Id = p.idProduit,
                Nom = p.nomProduit,
                Type = p.idTypeProduitNavigation?.nomTypeProduit,
                Marque = p.idMarqueNavigation?.nomMarque
            });

            return Ok(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProduit(int id, ProduitUpdateDto dto)
        {
            if (id != dto.IdProduit) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingResult = await manager.GetByIdAsync(id);
            var userToUpdate = existingResult.Value;

            if (userToUpdate == null) return NotFound();

            var entity = new Produit
            {
                idProduit = dto.IdProduit,
                nomProduit = dto.NomProduit,
                description = dto.Description,
                nomPhoto = dto.NomPhoto,
                uriPhoto = dto.UriPhoto,
                idTypeProduit = dto.IdTypeProduit,
                idMarque = dto.IdMarque,
                stockReel = dto.StockReel,
                stockMin = dto.StockMin,
                stockMax = dto.StockMax
            };

            await manager.UpdateAsync(userToUpdate, entity);

            return NoContent();
        }
    }
}
