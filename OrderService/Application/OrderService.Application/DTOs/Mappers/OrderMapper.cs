using OrderService.Application.DTOs.Mappers.Interfaces;
using OrderService.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace OrderService.Application.DTOs.Mappers
{
    [Mapper]
    public partial class OrderMapper : IOrderMapper
    {
        public partial OrderDTO ToDto(Order order);

        public partial Order ToEntity(OrderDTO orderDto);
    }
}
