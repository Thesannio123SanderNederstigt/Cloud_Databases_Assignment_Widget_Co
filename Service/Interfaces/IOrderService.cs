using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IOrderService
{
    public Task<ICollection<Order>> GetOrders();

    Task<Order> GetOrderById(string orderId);

    Task<Order> CreateOrder(OrderDTO order);

    Task<Order> UpdateOrder(UpdateOrderDTO updateOrderDTO);

    Task DeleteOrder(string orderId);

    Task<Order> GetunprocessedOrder();
}