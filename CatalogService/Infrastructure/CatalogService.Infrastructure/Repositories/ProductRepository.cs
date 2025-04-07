using CatalogService.Application.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class ProductRepository(CatalogDbContext context) : IProductRepository
    {
        private readonly IMongoCollection<Product> _products = context.Products;
        private readonly IMongoCollection<Category> _categories = context.Categories;
        private readonly IMongoCollection<Counter> _counters = context.Database.GetCollection<Counter>("Counters");

        public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(x => x.Id, productId),
                Builders<Product>.Filter.Eq(x => x.IsDeleted, false)
            );
            var product = await _products.Find(filter).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return null;
            }

            var categoryFilter = Builders<Category>.Filter.Eq(c => c.Id, product.CategoryId);
            var category = await _categories.Find(categoryFilter).FirstOrDefaultAsync(cancellationToken);

            product.Category = category;

            return product;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            var insertOneOptions = new InsertOneOptions();
            product.Id = await GetNextSequenceValue("products");
            await _products.InsertOneAsync(product, insertOneOptions, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
            await _products.ReplaceOneAsync(filter, product, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
            await _products.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allCategories = await _categories.Find(_ => true).ToListAsync(cancellationToken);
            var categoryDict = allCategories.ToDictionary(c => c.Id, c => c);

            var products = await _products.Find(_ => true).ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                if (categoryDict.TryGetValue(product.CategoryId, out var cat))
                {
                    product.Category = cat;
                }
                else
                {
                    throw new InvalidOperationException($"Product {product.Id} has a category {product.CategoryId} that does not exist.");
                }
            }

            return products;
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
    }
}
