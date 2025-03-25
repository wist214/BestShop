using System.ComponentModel.DataAnnotations;

namespace OrderService.Presentation.Models
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }

        [MinLength(1)]
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [MinLength(1)]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
