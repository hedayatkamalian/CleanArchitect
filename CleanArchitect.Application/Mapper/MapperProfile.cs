using AutoMapper;
using CleanArchitect.Application.Dtos.Customers;
using CleanArchitect.Application.Dtos.Orders;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Domain.Entities;
using CleanArchitect.Domain.ValueObjects;

namespace CleanArchitect.Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(dst => dst.CustomerName, src => src.MapFrom(p => $"{p.Customer.FirstName} {p.Customer.LastName}"));
    }
}
