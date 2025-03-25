using CatalogService.Application.DTO;
using CatalogService.Application.DTO.Mappers;
using CatalogService.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryMapper _mapper;

        public CategoriesController(ICategoryRepository productRepo, ICategoryMapper mapper)
        {
            _categoryRepository = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(_mapper.ToDto).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CategoryDTO categoryDTO)
        {
            var product = _mapper.ToEntity(categoryDTO);
            await _categoryRepository.AddAsync(product);
            return Ok();
        }
    }
}
