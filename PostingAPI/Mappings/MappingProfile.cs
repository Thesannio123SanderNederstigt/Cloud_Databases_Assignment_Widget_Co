using AutoMapper;
using Model;
using Model.DTO;
using Model.Response;
using System.Threading.Tasks;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderDTO, Task<Order>>().ConvertUsing<OrderConverter>();
        CreateMap<ProductDTO, Product>().ConvertUsing<ProductConverter>();
        CreateMap<ReviewDTO, Task<Review>>().ConvertUsing<ReviewConverter>();
        CreateMap<User, UserResponse>();
        CreateMap<UserDTO, User>().ConvertUsing<UserConverter>();
    }
}