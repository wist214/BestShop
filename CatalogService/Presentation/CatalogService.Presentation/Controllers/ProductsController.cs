using CatalogService.Application.DTO;
using CatalogService.Application.DTO.Mappers.Interfaces;
using CatalogService.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductMapper _mapper;

        public ProductsController(IProductRepository productRepo, IProductMapper mapper)
        {
            _productRepository = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(_mapper.ToDto).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDTO productDto)
        {
            var product = _mapper.ToEntity(productDto);
            await _productRepository.AddAsync(product);
            return Ok();
        }
    }
}
