using CatalogService.GrpcContracts;

namespace WebClient.Services
{
    public interface ICatalogServiceClient
    {
        Task<Product> GetProductAsync(int productId);
        Task<GetAllProductsResponse> GetAllProductsAsync();
        Task<CreateProductResponse> CreateProductAsync(Models.CreateProductRequest request);
        Task<DeleteProductResponse> DeleteProductAsync(int productId);
    }

    public class CatalogServiceClient : ICatalogServiceClient
    {
        private readonly Catalog.CatalogClient _client;

        public CatalogServiceClient(Catalog.CatalogClient client)
        {
            _client = client;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            var request = new GetProductRequest { ProductId = productId };
            return await _client.GetProductAsync(request);
        }

        public async Task<GetAllProductsResponse> GetAllProductsAsync()
        {
            return _client.GetAllProducts(new GetAllProductsRequest());
        }

        public async Task<CreateProductResponse> CreateProductAsync(Models.CreateProductRequest request)
        {
            var grpcRequest = new CreateProductRequest
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            };
            return await _client.CreateProductAsync(grpcRequest);
        }

        public async Task<DeleteProductResponse> DeleteProductAsync(int productId)
        {
            var grpcRequest = new CatalogService.GrpcContracts.DeleteProductRequest() { ProductId = productId };
            return await _client.DeleteProductAsync(grpcRequest);
        }
    }
}
