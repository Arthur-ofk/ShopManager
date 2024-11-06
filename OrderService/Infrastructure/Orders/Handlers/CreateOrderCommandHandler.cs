using Application.Interfaces;
using Application.Orders.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

using Domain.ValueObjects;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using SharedLib.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Orders.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper ,IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);
            await _orderRepository.AddAsync(order);
            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                ItemId = request.Items.FirstOrDefault().ItemId,
                Quantity = request.Items.FirstOrDefault().Quantity
            };

            await _publishEndpoint.Publish(orderCreatedEvent);

            return order.Id;
        }
    }
}
