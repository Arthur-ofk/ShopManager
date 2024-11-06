using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; }

        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
