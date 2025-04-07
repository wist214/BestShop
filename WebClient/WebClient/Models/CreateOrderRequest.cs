using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
    }
}
