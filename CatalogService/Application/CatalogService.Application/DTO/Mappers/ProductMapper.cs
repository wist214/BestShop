using CatalogService.Application.DTO.Mappers.Interfaces;
using CatalogService.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CatalogService.Application.DTO.Mappers
{
    [Mapper]
    public partial class ProductMapper : IProductMapper
    {
        public partial ProductDTO ToDto(Product product);
        public partial Product ToEntity(ProductDTO productDto);
    }
}
