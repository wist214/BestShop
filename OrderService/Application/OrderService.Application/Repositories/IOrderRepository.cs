using OrderService.Domain.Entities;

namespace OrderService.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task<Order> GetByUserIdAsync(int orderId, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    }
}
