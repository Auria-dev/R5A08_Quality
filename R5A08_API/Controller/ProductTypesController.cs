using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeRepository _repository;
        private readonly IMapper _mapper;

        public ProductTypesController(IProductTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAll()
        {
            var types = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductTypeDto>>(types);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductTypeDto>> GetById(int id)
        {
            var productType = await _repository.GetByIdAsync(id);
            if (productType == null) return NotFound();

            var dto = _mapper.Map<ProductTypeDto>(productType);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductTypeDto>> Create([FromBody] ProductTypeCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<ProductType>(dto);
            await _repository.AddAsync(entity);

            var resultDto = _mapper.Map<ProductTypeDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductTypeUpdateDto dto)
        {
            if (id != dto.ProductTypeId) return BadRequest();
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

