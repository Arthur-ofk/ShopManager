using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public Address  ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public int TotalAmount {  get; set; }
    }
}
