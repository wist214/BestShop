using CatalogService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data
{
    public class CatalogDbContext
    {   
        private readonly IMongoDatabase _database;

        public CatalogDbContext(IOptions<CatalogDbOptions> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _database = mongoClient.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");

        public IMongoDatabase Database => _database;
    }
}
