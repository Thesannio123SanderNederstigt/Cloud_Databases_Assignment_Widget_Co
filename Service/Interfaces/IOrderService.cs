using Model;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;

public interface IOrderService
{
    public Task<ICollection<OrderResponse>> GetOrders();

    Task<OrderResponse> GetOrderResById(string orderId);

    Task<Order> GetOrderById(string orderId);

    Task<Order> CreateOrder(OrderDTO order);

    Task<Order> UpdateOrder(UpdateOrderDTO updateOrderDTO);

    Task DeleteOrder(string orderId);

    Task<Order> GetunprocessedOrder();
}