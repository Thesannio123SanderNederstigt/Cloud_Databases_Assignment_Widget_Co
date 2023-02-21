using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Mappings;

public class OrderConverter : ITypeConverter<OrderDTO, Task<Order>>
{
    private readonly IUserService _userService;

    public OrderConverter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Order> Convert(OrderDTO source, Task<Order> destination, ResolutionContext context)
    {
        return new() {
            OrderId = Guid.NewGuid().ToString(),
            OrderDate = DateTime.UtcNow,
            //ShippingDate = null,
            IsProcessed = false,
            User = await _userService.GetUserById(source.UserId),
        };
    }
}