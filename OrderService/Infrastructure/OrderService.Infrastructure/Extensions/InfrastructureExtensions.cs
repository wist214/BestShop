using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.DTOs.Mappers;
using OrderService.Application.DTOs.Mappers.Interfaces;
using OrderService.Application.Repositories;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrderDb"));
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderMapper, OrderMapper>();

            return services;
        }
    }
}
