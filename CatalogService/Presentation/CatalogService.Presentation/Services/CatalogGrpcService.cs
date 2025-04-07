using CatalogService.Application.Repositories;
using CatalogService.Domain.Entities;
using CatalogService.GrpcContracts;
using Grpc.Core;

namespace CatalogService.Presentation.Services
{
    public class CatalogGrpcService : Catalog.CatalogBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CatalogGrpcService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
                }
            }

            var response = new GetProductResponse
            {
                ProductId = request.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return response;
        }

        public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            CreateProductResponse response;

            try
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

                if (category == null)
                {
                    response = new CreateProductResponse
                    {
                        Success = false,
                        Message = "Category was not found"
                    };

                    return response;
                }

                var newProduct = new Product()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    CategoryId = request.CategoryId
                };

                await _productRepository.AddAsync(newProduct);

                response = new CreateProductResponse
                {
                    Success = true,
                    Message = "Product created successfully"
                };
            }
            catch (Exception ex)
            {
                response = new CreateProductResponse
                {
                    Success = false,
                    Message = $"Error={ex.Message}"
                };
            }

            return response;
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            DeleteProductResponse response;

            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId);

                if (product == null)
                {
                    response = new DeleteProductResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    };

                    return response;
                }

                product.IsDeleted = true;
                await _productRepository.UpdateAsync(product);

                response = new DeleteProductResponse
                {
                    Success = true,
                    Message = "Product marked as deleted successfully"
                };
            }
            catch (Exception ex)
            {
                response = new DeleteProductResponse
                {
                    Success = false,
                    Message = $"Error={ex.Message}"
                };
            }

            return response;
        }
    }
}
