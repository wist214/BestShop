using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Repositories;
using OrderService.Domain.Entities;
using OrderService.Presentation.Models;

namespace OrderService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        public OrdersController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow, 
                Status = OrderStatus.Created
            };

            foreach (var item in request.Items)
            {
                order.AddItem(item.ProductId, item.Quantity, item.Price);
            }

            await _orderRepo.AddAsync(order);
            return Ok(order.Id);
        }
    }
}
