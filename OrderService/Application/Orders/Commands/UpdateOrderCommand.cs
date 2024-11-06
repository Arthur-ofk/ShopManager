using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; }
        public UpdateOrderDto order { get; }

        public UpdateOrderCommand(Guid orderId, UpdateOrderDto updateOrderDto)
        {
            OrderId = orderId;
             order = updateOrderDto;
        }
    }
}
