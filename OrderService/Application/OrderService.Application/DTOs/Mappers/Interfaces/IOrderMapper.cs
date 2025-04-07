using OrderService.Domain.Entities;

namespace OrderService.Application.DTOs.Mappers.Interfaces
{
    public interface IOrderMapper
    {
        OrderDTO ToDto(Order order);
        Order ToEntity(OrderDTO orderDto);
    }
}
