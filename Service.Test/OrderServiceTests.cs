using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {

        ServiceProvider services = new ServiceCollection()
        .AddScoped<API.Mappings.OrderConverter>()
        .BuildServiceProvider();

        IMapper mapper = new MapperConfiguration(c => {
            c.ConstructServicesUsing(s => services.GetService(s));
            c.AddMaps(typeof(API.Mappings.MappingProfile));
        }).CreateMapper();

        _mockOrderRepository = new();
        _mockUserRepository = new();
        _mockProductRepository = new();
        _orderService = new OrderService(new LoggerFactory(), _mockOrderRepository.Object, _mockUserRepository.Object, _mockProductRepository.Object, mapper);
    }

    [Fact]
    public async Task Get_All_Orders_Should_Return_An_Array_Of_Orders()
    {
        Order[] mockOrders = new[] {
            new Order("1", DateTime.Parse("2022-11-05 11:00:00"), DateTime.Parse("2022-09-12 15:25:00"), 30m, false, new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!), new Product[] { new Product("1", "Apple EarPods Lightning", 10m, new Review[] { new Review("1", "This product is awful, would not buy again", DateTime.UtcNow, "1") }) }),
            new Order("2", DateTime.Parse("2022-12-05 10:56:12"),DateTime.Parse("2022-12-12 15:00:00"), 25m, false, new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!), new Product[] { new Product("2", "Apple EarPods Lightning", 10m, null!) }),
        };

        _mockOrderRepository.Setup(o => o.Include(o => o.User).Include(o => o.Products).ThenInclude(p => p.Reviews).GetAll()).Returns(mockOrders.ToAsyncEnumerable());

        ICollection<OrderResponse> orders = await _orderService.GetOrders();

        Assert.Equal(2, orders.Count);
    }

    [Fact]
    public void Get_All_Orders_Should_Throw_Not_Found_Exception()
    {
        _mockOrderRepository.Setup(o => o.GetAllAsync()).Returns(() => null!);

        Assert.ThrowsAsync<NotFoundException>(async () => await _orderService.GetOrders());
    }

    [Fact]
    public async Task Get_By_Order_Id_Should_Return_Order_With_Given_Id()
    {
        _mockOrderRepository.Setup(o => o.GetByIdAsync("1")).ReturnsAsync(() => new Order("1", DateTime.Parse("2022-11-05 11:00:00"), DateTime.Parse("2022-09-12 15:25:00"), 30m, true, new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!), null!));
        Order order = await _orderService.GetOrderById("1");

        Assert.Equal("1", order.OrderId);
    }

    [Fact]
    public void Get_By_Order_Id_Should_Throw_Not_Found_Exception()
    {
        _mockOrderRepository.Setup(o => o.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrderById("naN"));
    }

    [Fact]
    public async Task Create_Order_Should_Return_An_Order_With_Order_Id()
    {
        //setup order
        _mockOrderRepository.Setup(o => o.InsertAsync(It.IsAny<Order>())).Verifiable();
        _mockOrderRepository.Setup(o => o.SaveChanges()).Verifiable();

        //setup user
        User user = new("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!);

        _mockUserRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => user);
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        //setup products
        _mockProductRepository.Setup(p => p.GetByIdAsync("32698064-986w-98d1-dk8p-ef6587a4oye6")).ReturnsAsync(() => new Product("32698064-986w-98d1-dk8p-ef6587a4oye6", "Apple EarPods Lightning", 10m, null!));
        _mockProductRepository.Setup(p => p.GetByIdAsync("56487016-541l-845p-ol5f-pe86f2am98n1")).ReturnsAsync(() => new Product("56487016-541l-845p-ol5f-pe86f2am98n1", "USB-C to HDMI cable 5m", 15.99m, null!));

        OrderDTO newOrderDTO = new(null, new string[] { "32698064-986w-98d1-dk8p-ef6587a4oye6", "56487016-541l-845p-ol5f-pe86f2am98n1" }, "1");
        Order order = await _orderService.CreateOrder(newOrderDTO);

        Assert.NotNull(order.OrderId);

        _mockOrderRepository.Verify(o => o.InsertAsync(It.IsAny<Order>()), Times.Once);
        _mockOrderRepository.Verify(o => o.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Order_Should_Have_Properties_Changed()
    {
        Order order = new("1", DateTime.Parse("2022-12-05 10:56:12"), DateTime.Parse("2022-12-12 15:00:00"), 25m, false, new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!), null!);

        _mockOrderRepository.Setup(o => o.GetByIdAsync("1")).ReturnsAsync(() => order);
        _mockOrderRepository.Setup(o => o.SaveChanges()).Verifiable();

        UpdateOrderDTO changes = new() { OrderId = "1", ShippingDate = DateTime.Parse("2023-01-05 17:04:36"), IsProcessed = true };
        await _orderService.UpdateOrder(changes);

        Assert.Equal("1", order.OrderId);
        Assert.Equal(DateTime.Parse("2023-01-05 17:04:36"), order.ShippingDate);

        Assert.True(order.IsProcessed);

        _mockOrderRepository.Verify(o => o.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Order_Should_Throw_Not_Found_Exception()
    {
        _mockOrderRepository.Setup(o => o.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("order"));

        UpdateOrderDTO changes = new() { ShippingDate = DateTime.Parse("2023-01-05 17:04:36"), IsProcessed = true };

        await Assert.ThrowsAsync<NotFoundException>(() => _orderService.UpdateOrder(changes));
    }

    [Fact]
    public async Task Delete_Order_Should_Delete_The_Order()
    {
        Order order = new("1", DateTime.Parse("2022-12-05 10:56:12"), DateTime.Parse("2022-12-12 15:00:00"), 24.99m, false, new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!), null!);

        _mockOrderRepository.Setup(o => o.GetByIdAsync("1")).ReturnsAsync(() => order);
        _mockOrderRepository.Setup(o => o.SaveChanges()).Verifiable();

        await _orderService.DeleteOrder("1");

        Assert.DoesNotContain(_mockOrderRepository.Object.ToString(), order.OrderId);
        Assert.Null(_mockOrderRepository.Object.GetAllAsync());

        _mockOrderRepository.Verify(o => o.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Order_Should_Throw_Not_Found_Exception()
    {
        _mockOrderRepository.Setup(o => o.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _orderService.DeleteOrder("naN"));
    }
}
