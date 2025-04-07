namespace WebClient.Models
{
    public class AddOrderItemRequest
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public OrderItem OrderItem { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
