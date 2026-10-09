using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _repository.GetAllWithDetailsAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        [HttpGet("GetProduitById/{id:int}")]
        [ActionName("GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDto>> GetById(int id)
        {
            var product = await _repository.GetByIdWithDetailsAsync(id);
            if (product == null) return NotFound();

            var dto = _mapper.Map<ProductDetailDto>(product);
            return Ok(dto);
        }

        [HttpGet("by-name/{name}")]
        [HttpGet("GetProduitByString/{name}")]
        [ActionName("GetProductsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDto>> GetByName(string name)
        {
            var product = await _repository.GetByNameAsync(name);
            if (product == null) return NotFound();

            var dto = _mapper.Map<ProductDetailDto>(product);
            return Ok(dto);
        }

        [HttpGet("by-brand/{brandId:int}")]
        [HttpGet("GetProduitsByMarque/{brandId:int}")]
        [ActionName("GetProductsByBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByBrand(int brandId)
        {
            var products = await _repository.GetByBrandAsync(brandId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(dtos);
        }

        [HttpGet("search/{term}")]
        [HttpGet("SearchProduits/{term}")]
        [ActionName("SearchProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchByName(string term)
        {
            var products = string.IsNullOrWhiteSpace(term)
                ? await _repository.GetAllWithDetailsAsync()
                : await _repository.SearchByNameAsync(term);

            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(dtos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Product>(dto);
            await _repository.AddAsync(entity);

            var createdProduct = await _repository.GetByIdWithDetailsAsync(entity.idProduit);
            var resultDto = _mapper.Map<ProductDto>(createdProduct ?? entity);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (id != dto.ProductId) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            await _repository.DeleteAsync(existingEntity);
            return NoContent();
        }
    }
}

