using OrderService.Domain.Entities;

namespace OrderService.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
