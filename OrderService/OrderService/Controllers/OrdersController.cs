using Application.DTOs;
using Application.Orders.Commands;
using Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [Route("api/orders/")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var command = new CreateOrderCommand
            {
                UserId = createOrderDto.UserId, 
                ShippingAddress = createOrderDto.ShippingAddress,
                Items = createOrderDto.Items,
                Status = "Pending",
                TotalAmount = 1
            };

            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            var command = new UpdateOrderCommand(orderId, updateOrderDto);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var command = new DeleteOrderCommand(orderId);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var order = await _mediator.Send(query);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var query =new  GetOrdersByUserIdQuery(userId);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
    }
}
