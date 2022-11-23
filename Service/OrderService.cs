using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Exceptions;

namespace Service;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(ILoggerFactory loggerFactory, IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
    {
        _logger = loggerFactory.CreateLogger<OrderService>();
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    // get orders
    public async Task<ICollection<Order>> GetOrders()
    {
        return await _orderRepository.GetAllAsync().ToArrayAsync() ?? throw new NotFoundException("orders");
    }

    // get order
    public async Task<Order> GetOrderById(string orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId) ?? throw new NotFoundException("order");
    }

    // create a new order
    public async Task<Order> CreateOrder(OrderDTO orderDTO)
    {
        Order order = new();

        // ensure the user exists (or throw an exception if it doesn't) and add it to the order
        User user = await _userRepository.GetByIdAsync(orderDTO.UserId) ?? throw new NotFoundException("user to create the order");

        order.User = user;

        order.OrderId = Guid.NewGuid().ToString();

        //get all the actual products and fill the collection with them (I guess?)
        ICollection<Product> orderedProducts = new List<Product>();

        decimal totalPrice = 0.0m;

        foreach (string productId in orderDTO.ProductIds)
        {
            Product product = await _productRepository.GetByIdAsync(productId) ?? throw new NotFoundException($"product with id {productId} in the order");
            orderedProducts.Add(product);

            totalPrice += product.Price;
        }

        order.OrderDate = DateTime.UtcNow;
        order.Total = totalPrice;

        await _orderRepository.InsertAsync(order);
        await _orderRepository.SaveChanges();

        return order;
    }

    // update an order
    public async Task<Order> UpdateOrder(UpdateOrderDTO changes)
    {
        Order order = await GetOrderById(changes.OrderId) ?? throw new NotFoundException("order to update");

        order.ShippingDate = changes.ShippingDate;
        order.IsProcessed = changes.IsProcessed;

        await _orderRepository.SaveChanges();

        return order;
    }

    // delete an order
    public async Task DeleteOrder(string orderId)
    {
        // retrieve and then remove the order
        Order order = await GetOrderById(orderId) ?? throw new NotFoundException("order to delete");

        _orderRepository.Remove(order);

        await _orderRepository.SaveChanges();

        _logger.LogInformation($"Delete order function deleted order: {order.OrderId}");
    }


    // retrieve an unshipped order
    public async Task<Order> GetunprocessedOrder()
    {
        return await _orderRepository.GetByAsync(o => o.IsProcessed == false) ?? throw new NotFoundException("an unshipped order");
    }
}