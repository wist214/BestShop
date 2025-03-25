using CatalogService.Application.DTO.Mappers;
using CatalogService.Application.DTO.Mappers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICategoryMapper, CategoryMapper>();
            services.AddSingleton<IProductMapper, ProductMapper>();

            return services;
        }
    }
}
