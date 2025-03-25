using Microsoft.EntityFrameworkCore;
using OrderService.Application.Repositories;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default)
        {
            return await context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await context.Orders.AddAsync(order, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            var existingOrder = await GetByIdAsync(order.Id, cancellationToken);

            if (existingOrder == null)
            {
                throw new InvalidOperationException($"Order with id {order.Id} not found");
            }

            context.Entry(existingOrder).State = EntityState.Detached;

            context.Orders.Update(order);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
