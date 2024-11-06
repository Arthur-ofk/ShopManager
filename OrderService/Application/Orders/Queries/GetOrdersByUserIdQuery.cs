using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<List<OrderDto>>
    {
        public Guid UserId { get; set; }

        public GetOrdersByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
