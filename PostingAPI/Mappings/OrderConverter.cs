using AutoMapper;
using Model;
using Model.DTO;

namespace API.Mappings;

public class OrderConverter : ITypeConverter<OrderDTO, Order>
{
    public OrderConverter()
    {
    }

    public Order Convert(OrderDTO source, Order destination, ResolutionContext context)
    {
        return new() {
            OrderId = Guid.NewGuid().ToString(),
            OrderDate = DateTime.UtcNow,
            IsProcessed = false,
        };
    }
}