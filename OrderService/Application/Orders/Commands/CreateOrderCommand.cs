using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string ShippingAddress { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
        public string Status { get; set; }
        public int TotalAmount { get; set; }


    }
}
