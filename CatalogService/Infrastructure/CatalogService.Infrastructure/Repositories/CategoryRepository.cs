using System.Diagnostics.Metrics;
using CatalogService.Application.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class CategoryRepository(CatalogDbContext context) : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories = context.Categories;
        private readonly IMongoCollection<Counter> _counters = context.Database.GetCollection<Counter>("Counters");

        public async Task<Category?> GetByIdAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, categoryId);
            return await _categories.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            category.Id = await GetNextSequenceValue("categories");
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

        public async Task<int> GetNextSequenceValue(string sequenceName)
        {
            // Build the filter to find the counter document with the specified sequence name.
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, sequenceName);
            // Build the update to increment the sequence value by 1.
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            // Options: upsert (create if not exists) and return the updated document.
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            // Atomically update the counter and retrieve the new sequence value.
            var counter = await _counters.FindOneAndUpdateAsync(filter, update, options);
            return counter.SequenceValue;
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _categories.Find(_ => true).ToListAsync(cancellationToken);
        }
    }

    public class Counter
    {
        // The sequence name, e.g., "products"
        public string Id { get; set; } = string.Empty;
        // The current sequence value
        public int SequenceValue { get; set; }
    }
}
