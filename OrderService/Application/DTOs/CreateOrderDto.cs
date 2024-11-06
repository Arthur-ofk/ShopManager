using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public string ShippingAddress { get; set; }  
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
