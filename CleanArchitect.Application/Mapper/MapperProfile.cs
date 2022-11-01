using AutoMapper;
using CleanArchitect.Application.Dtos.Products;
using CleanArchitect.Domain.Entities;

namespace CleanArchitect.Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}
