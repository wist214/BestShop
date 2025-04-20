using OrderService.GrpcContracts;
using WebClient.Models;
using OrderItem = OrderService.GrpcContracts.OrderItem;

namespace WebClient.Services
{
    public interface IOrderServiceClient
    {
        Task<List<Order>> GetOrdersAsync(int userId);
        Task<CreateOrderResponse> CreateOrderAsync(Models.CreateOrderRequest request);
        Task<AddOrderItemResponse> AddOrderItemAsync(Models.AddOrderItemRequest request);
        Task<CancelOrderResponse> CancelOrderAsync(int orderId);
        Task<UpdateOrderResponse> UpdateOrderAsync(Order order);
    }

    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly OrderService.GrpcContracts.OrderService.OrderServiceClient _client;

        public OrderServiceClient(OrderService.GrpcContracts.OrderService.OrderServiceClient client)
        {
            _client = client;
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            var result = await _client.GetOrdersAsync(new GetOrdersRequest(){UserId = userId });

            return result.Orders.ToList();
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(Models.CreateOrderRequest request)
        {
            var grpcRequest = new OrderService.GrpcContracts.CreateOrderRequest
            {
                UserId = request.UserId,
            };

            grpcRequest.Items.AddRange(request.Items.Select(x => new OrderService.GrpcContracts.OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price
            }));

            return await _client.CreateOrderAsync(grpcRequest);
        }

        public async Task<AddOrderItemResponse> AddOrderItemAsync(Models.AddOrderItemRequest request)
        {
            var grpcRequest = new OrderService.GrpcContracts.AddOrderItemRequest()
            {
                UserId = request.UserId,
                OrderId = request.OrderId,
                OrderItem = new OrderService.GrpcContracts.OrderItem()
                {
                    ProductId = request.OrderItem.ProductId,
                    Quantity = request.OrderItem.Quantity,
                    Price = request.OrderItem.Price
                }
            };

            return await _client.AddOrderItemAsync(grpcRequest);
        }

        public async Task<CancelOrderResponse> CancelOrderAsync(int orderId)
        {
            return await _client.CancelOrderAsync(new CancelOrderRequest(){OrderId = orderId});
        }

        public async Task<UpdateOrderResponse> UpdateOrderAsync(Order order)
        {
            return await _client.UpdateOrderAsync(new UpdateOrderRequest() { Order = order});
        }
    }
}
