using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; } 
        public string ShippingAddress { get; set; }
        public string Status { get; set; } 
        public int Amount {  get; set; }
    }
}
