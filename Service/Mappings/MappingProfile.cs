using AutoMapper;
using Model;
using Model.DTO;
using Model.Response;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderDTO, Task<Order>>().ConvertUsing<OrderConverter>();
        CreateMap<Order, OrderResponse>();
        CreateMap<ProductDTO, Product>().ConvertUsing<ProductConverter>();
        CreateMap<Product, ProductResponse>();
        CreateMap<ReviewDTO, Review>().ConvertUsing<ReviewConverter>();
        CreateMap<User, UserResponse>();
        CreateMap<UserDTO, User>().ConvertUsing<UserConverter>();
    }
}