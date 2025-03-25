using CatalogService.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CatalogService.Application.DTO.Mappers.Interfaces
{
    public interface IProductMapper
    {
        [MapProperty("Category.Name", "CategoryName")]
        ProductDTO ToDto(Product product);

        [MapProperty("CategoryName", "Category.Name")]
        Product ToEntity(ProductDTO productDto);

    }
}
