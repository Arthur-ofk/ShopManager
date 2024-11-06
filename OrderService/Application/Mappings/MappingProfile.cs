using Application.DTOs;
using Application.Orders.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) 
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => ConvertToAddress(src.ShippingAddress)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => ConvertToAddress(src.ShippingAddress))); ;
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));
            CreateMap<Address, AddressDto>();

        }

        private Address ConvertToAddress(string shippingAddress)
        {
            if (string.IsNullOrEmpty(shippingAddress))
            {
                throw new ArgumentException("Shipping address cannot be null or empty", nameof(shippingAddress));
            }

            var parts = shippingAddress.Split(",");

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid shipping address format. Expected format: 'Street, City, Country'.", nameof(shippingAddress));
            }

            return new Address(parts[0].Trim(), parts[1].Trim(), parts[2].Trim());
        }
    }
}
