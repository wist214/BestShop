using CatalogService.Domain.Entities;

namespace CatalogService.Application.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);   
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default);    
        Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
