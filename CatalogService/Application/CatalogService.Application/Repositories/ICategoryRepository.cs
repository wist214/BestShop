using CatalogService.Domain.Entities;

namespace CatalogService.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task AddAsync(Category product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category product, CancellationToken cancellationToken = default);
        Task DeleteAsync(Category product, CancellationToken cancellationToken = default);
        Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
