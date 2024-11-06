using Application.Interfaces;
using Application.Orders.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Orders.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await  _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
            {
                return false; // Або киньте виняток, якщо замовлення не знайдено
            }

            
             await _orderRepository.DeleteAsync(order);
            return true;
        }

       
    }
}
