using AutoMapper;
using Model;
using Model.DTO;
using System;

namespace API.Mappings;

public class UserConverter : ITypeConverter<UserDTO, User>
{
    public User Convert(UserDTO source, User destination, ResolutionContext context)
    {
        return new() {
            UserId = Guid.NewGuid().ToString(),
            Email = source.Email,
            UserName = source.UserName,
            Password = source.Password
        };
    }
}