using AutoMapper;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Mapping
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.TotalAmount,
                    opt => opt.MapFrom(src=> src.Items.Sum(x=>x.Quantity*x.UnitPrice))
                );
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name)
                );
        }
    }
}
