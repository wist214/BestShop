using System.ComponentModel.DataAnnotations;

namespace OrderService.Presentation.Models
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
