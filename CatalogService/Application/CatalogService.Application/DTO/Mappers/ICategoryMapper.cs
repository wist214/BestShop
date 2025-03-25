using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTO.Mappers
{
    public interface ICategoryMapper
    {
        CategoryDTO ToDto(Category category);
        Category ToEntity(CategoryDTO categoryDto);
    }
}
