using CatalogService.Application.DTO.Mappers.Interfaces;
using CatalogService.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace CatalogService.Application.DTO.Mappers
{
    [Mapper]
    public partial class CategoryMapper : ICategoryMapper
    {
        public partial CategoryDTO ToDto(Category category);
        public partial Category ToEntity(CategoryDTO categoryDto);
    }
}
