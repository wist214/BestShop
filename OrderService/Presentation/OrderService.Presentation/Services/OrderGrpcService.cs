using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrderService.Application.Repositories;
using OrderService.GrpcContracts;

namespace OrderService.Presentation.Services
{
    public class OrderGrpcService : GrpcContracts.OrderService.OrderServiceBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderGrpcService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Retrieves an order by its ID.
        public override async Task<Order> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            var dbOrder = await _orderRepository.GetByUserIdAsync(request.UserId);
            if (dbOrder == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Order with UserID {request.UserId} not found."));
            }

            var order = new Order
            {
                Id = dbOrder.Id,
                UserId = dbOrder.UserId,
                Status = (OrderStatus)(int)dbOrder.Status,
                CreatedAt = Timestamp.FromDateTime(dbOrder.CreatedAt.ToUniversalTime()),
            };

            order.Items.AddRange(dbOrder.Items.Select(x => new OrderItem
            {
                Id = x.Id,
                ProductId = x.ProductId,
                OrderId = x.OrderId,
                Price = (double)x.Price,
                Quantity = x.Quantity
            }));

            return order;
        }

        // Creates a new order and returns its generated ID.
        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var order = new Domain.Entities.Order
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Status = Domain.Entities.OrderStatus.Created,
                Items = request.Items.Select(x => new Domain.Entities.OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = (decimal)x.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            return new CreateOrderResponse
            {   
                OrderId = order.Id,
                Success = true,
                Message = "Order created successfully.",
            };
        }

        // Updates an existing order.
        public override async Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
           var order = new Domain.Entities.Order
           {
               Id = request.Order.Id,
               UserId = request.Order.UserId,
               CreatedAt = request.Order.CreatedAt.ToDateTime(),
               Status = (Domain.Entities.OrderStatus)(int)request.Order.Status,
               Items = request.Order.Items.Select(x => new Domain.Entities.OrderItem
               {
                   Id = x.Id,
                   ProductId = x.ProductId,
                   OrderId = x.OrderId,
                   Price = (decimal)x.Price,
                   Quantity = x.Quantity
               }).ToList()
           };

            await _orderRepository.UpdateAsync(order);

            return new UpdateOrderResponse
            {
                Success = true,
                Message = "Order updated successfully."
            };
        }

        public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Order with ID {request.OrderId} not found."));
            }

            order.Cancel();

            await _orderRepository.UpdateAsync(order);

            return new CancelOrderResponse
            {
                Success = true,
                Message = "Order cancelled successfully."
            };
        }

        public override async Task<AddOrderItemResponse> AddOrderItem(AddOrderItemRequest request, ServerCallContext context)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Order with ID {request.OrderId} not found."));
            }

            order.AddItem(request.OrderItem.ProductId, request.OrderItem.Quantity, (decimal)request.OrderItem.Price);

            await _orderRepository.UpdateAsync(order);

            return new AddOrderItemResponse
            {
                Success = true,
                Message = "Order updated successfully."
            };
        }

    }
}
