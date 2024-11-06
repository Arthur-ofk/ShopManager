using Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUpdateOrderCommandHandler
    {
        Task<bool> Handle(UpdateOrderCommand command);
    }
}
