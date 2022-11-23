using AutoMapper;
using Model;
using Model.Response;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>();
    }
}