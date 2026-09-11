using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Produit>> PostProduit(Produit entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await manager.AddAsync(entity);
            return CreatedAtAction("GetUtilisateur", new { id = entity.idProduit}, entity);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
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
        public async Task<ActionResult<IEnumerable<Produit>>> GetAllProduit()
        {
            var res = await manager.GetAllAsync();
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetProduitsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetProduitById(int id)
        {
            var result = await manager.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{string}")]
        [ActionName("GetProduitsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetProduitByString(string str)
        {
            var result = await manager.GetByStringAsync(str);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{string}")]
        [ActionName("GetProduitsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Produit>> GetByMarque(int idMarque)
        {
            var result = await manager.GetByMarqueAsync(idMarque);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProduit(int id, Produit entity)
        {
            if (id != entity.idProduit) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userToUpdate = await manager.GetByIdAsync(id);
            if (userToUpdate == null) return NotFound();
            else await manager.UpdateAsync(userToUpdate.Value, entity);

            return NoContent();
        }
    }
}
