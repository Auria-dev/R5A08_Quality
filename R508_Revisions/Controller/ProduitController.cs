using AutoMapper;
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
    public class ProduitController (IProduitRepository manager, IMapper mapper): ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProduitDto>> PostProduit(Produit entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await manager.AddAsync(entity);

            var createdProduit = await manager.GetByIdWithDetailsAsync(entity.idProduit);
            var dto = mapper.Map<ProduitDto>(createdProduit);

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
            var res = mapper.Map<IEnumerable<ProduitDto>>(produits);

            return Ok(res);
        }

        [HttpGet]
        [Route("[action]/{idProduit}")]
        [ActionName("GetProduitById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDetailDto>> GetProduitById(int idProduit)
        {
            var produit = await manager.GetByIdWithDetailsAsync(idProduit);
            if (produit == null) return NotFound();
            var dto = mapper.Map<ProduitDetailDto>(produit);

            return Ok(dto);
        }

        [HttpGet]
        [Route("[action]/{nomProduit}")]
        [ActionName("GetProduitsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDetailDto>> GetProduitByString(string nomProduit)
        {
            var result = await manager.GetByStringAsync(nomProduit);
            var produit = result.Value;
            if (result == null || produit == null) return NotFound();
            var dto = mapper.Map<ProduitDetailDto>(produit);

            return Ok(dto);
        }

        [HttpGet]
        [Route("[action]/{idMarque}")]
        [ActionName("GetProduitsByMarque")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetByMarque(int idMarque)
        {
            var produits = await manager.GetByMarqueAsync(idMarque);
            if (produits == null || !produits.Any()) return NotFound();
            var dtos = mapper.Map<IEnumerable<ProduitDto>>(produits);

            return Ok(dtos);
        }

        [HttpGet]
        [Route("[action]/{term}")]
        [ActionName("SearchProduits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> SearchByName(string term)
        {
            var produits = await manager.SearchByNameAsync(term);
            return Ok(mapper.Map<IEnumerable<ProduitDto>>(produits));
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
            var entity = mapper.Map<Produit>(dto);

            await manager.UpdateAsync(userToUpdate, entity);

            return NoContent();
        }
    }
}
