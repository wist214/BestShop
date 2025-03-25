using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTO.Mappers.Interfaces
{
    public interface ICategoryMapper
    {
        CategoryDTO ToDto(Category category);
        Category ToEntity(CategoryDTO categoryDto);
    }
}
