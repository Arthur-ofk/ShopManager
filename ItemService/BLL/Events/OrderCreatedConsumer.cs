using DAL.Abstractions;
using DAL.Repos;

using MassTransit;
using SharedLib.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Events
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IItemRepository _itemRepository;

        public OrderCreatedConsumer(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            
            await _itemRepository.UpdateQuantityAsync(message.ItemId, message.Quantity);
            
        }
    }
}
