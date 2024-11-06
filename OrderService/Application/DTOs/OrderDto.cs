using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public AddressDto ShippingAddress { get; set; }  

        public int TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
