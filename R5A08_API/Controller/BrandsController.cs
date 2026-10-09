using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _repository;
        private readonly IMapper _mapper;

        public BrandsController(IBrandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll()
        {
            var brands = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BrandDto>>(brands);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null) return NotFound();

            var dto = _mapper.Map<BrandDto>(brand);
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BrandDto>> Create([FromBody] BrandCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Brand>(dto);
            await _repository.AddAsync(entity);

            var resultDto = _mapper.Map<BrandDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] BrandUpdateDto dto)
        {
            if (id != dto.BrandId) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            var newEntity = _mapper.Map<Brand>(dto);
            await _repository.UpdateAsync(newEntity);

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

