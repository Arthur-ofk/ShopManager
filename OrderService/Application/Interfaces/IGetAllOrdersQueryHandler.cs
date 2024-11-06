using Application.DTOs;
using Application.Orders.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGetAllOrdersQueryHandler
    {
        Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery query);
    }
}
