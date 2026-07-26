using AutoMapper;
using SmartOrderManagement.Application.Dtos;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Mappings;
public class Profiler : Profile
{
    public Profiler()
    {
        CreateMap<CreateOrderRequest, Order>();
        CreateMap<Order, OrderResponse>();

        CreateMap<CreateMenuRequest, Menu>();
        CreateMap<Menu, MenuResponse>();
    }
}