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

        public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, productId);
            var product = await _products.Find(filter).FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                return null;

            var categoryFilter = Builders<Category>.Filter.Eq(c => c.Id, product.CategoryId);
            var category = await _categories.Find(categoryFilter).FirstOrDefaultAsync(cancellationToken);

            product.Category = category;

            return product;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            var insertOneOptions = new InsertOneOptions();
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
    }
}
