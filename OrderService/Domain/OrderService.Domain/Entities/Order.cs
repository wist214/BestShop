namespace OrderService.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<OrderItem> Items { get; set; } = new();

        public void AddItem(int productId, int quantity, decimal price)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive.");
            }

            Items.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = price
            });
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Order already cancelled.");
            }
               
            Status = OrderStatus.Cancelled;
        }
    }

    public enum OrderStatus
    {
        Created,
        Paid,
        Shipped,
        Delivered,
        Cancelled
    }
}
