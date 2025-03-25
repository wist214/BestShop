using CatalogService.Application.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class CategoryRepository(CatalogDbContext context) : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories = context.Categories;

        public async Task<Category?> GetByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, categoryId);
            return await _categories.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _categories.InsertOneAsync(category, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);
            await _categories.ReplaceOneAsync(filter, category, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);
            await _categories.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _categories.Find(_ => true).ToListAsync(cancellationToken);
        }
    }
}
