using AutoMapper;
using Model;
using Model.Response;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderResponse>();
        CreateMap<Product, ProductResponse>();
        CreateMap<User, UserResponse>();
    }
}