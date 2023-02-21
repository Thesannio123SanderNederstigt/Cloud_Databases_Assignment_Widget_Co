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

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {

        ServiceProvider services = new ServiceCollection()
        .AddScoped<API.Mappings.ProductConverter>()
        .BuildServiceProvider();

        IMapper mapper = new MapperConfiguration(c => {
            c.ConstructServicesUsing(s => services.GetService(s));
            c.AddMaps(typeof(API.Mappings.MappingProfile));
        }).CreateMapper();

        _mockRepository = new();
        _productService = new ProductService(new LoggerFactory(), _mockRepository.Object, mapper);
    }

    [Fact]
    public async Task Get_All_Products_Should_Return_An_Array_Of_Products()
    {
        Product[] mockProducts = new[] {
            new Product("1", "Apple EarPods Lightning", 10m, null!),
            new Product("2", "USB-C to HDMI cable 5m", 15.99m, null!),
        };

        _mockRepository.Setup(p => p.Include(p => p.Reviews).GetAll()).Returns(mockProducts.ToAsyncEnumerable());

        ICollection<ProductResponse> products = await _productService.GetProducts();

        Assert.Equal(2, products.Count);
    }

    [Fact]
    public void Get_All_Products_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(p => p.GetAllAsync()).Returns(() => null!);

        Assert.ThrowsAsync<NotFoundException>(async () => await _productService.GetProducts());
    }

    [Fact]
    public async Task Get_By_Product_Id_Should_Return_Product_With_Given_Id()
    {
        _mockRepository.Setup(p => p.GetByIdAsync("1")).ReturnsAsync(() => new Product("1", "Apple EarPods Lightning", 10m, null!));
        Product product = await _productService.GetProductById("1");

        Assert.Equal("1", product.ProductId);
    }

    [Fact]
    public void Get_By_Product_Id_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(p => p.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _productService.GetProductById("naN"));
    }

    [Fact]
    public async Task Create_Product_Should_Return_A_Product_With_Product_Id()
    {
        _mockRepository.Setup(p => p.InsertAsync(It.IsAny<Product>())).Verifiable();
        _mockRepository.Setup(p => p.SaveChanges()).Verifiable();

        Product newProduct = new();
        Product product = await _productService.CreateProduct(newProduct);

        Assert.NotNull(product.ProductId);

        _mockRepository.Verify(p => p.InsertAsync(It.IsAny<Product>()), Times.Once);
        _mockRepository.Verify(p => p.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Product_Should_Have_Properties_Changed()
    {
        Product product = new Product("1", "Apple EarPods Lightning", 10m, null!);

        _mockRepository.Setup(p => p.GetByIdAsync("1")).ReturnsAsync(() => product);
        _mockRepository.Setup(p => p.SaveChanges()).Verifiable();

        UpdateProductDTO changes = new() { ProductName = "Apple Earpods Lightning 5m", Price = 15.99m };
        await _productService.UpdateProduct("1", changes);

        Assert.Equal("1", product.ProductId);
        Assert.Equal("Apple Earpods Lightning 5m", product.ProductName);
        Assert.Equal(15.99m, product.Price);

        _mockRepository.Verify(p => p.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Product_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(p=> p.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("product"));

        UpdateProductDTO changes = new() { ProductName = "Apple Earpods Lightning 5m", Price = 15.99m };

        await Assert.ThrowsAsync<NotFoundException>(() => _productService.UpdateProduct("1", changes));
    }

    [Fact]
    public async Task Delete_Product_Should_Delete_The_Product()
    {
        Product product = new Product("1", "Apple EarPods Lightning", 10m, null!);

        _mockRepository.Setup(p => p.GetByIdAsync("1")).ReturnsAsync(() => product);
        _mockRepository.Setup(p => p.SaveChanges()).Verifiable();

        await _productService.DeleteProduct("1");

        Assert.DoesNotContain(_mockRepository.Object.ToString(), product.ProductId);
        Assert.Null(_mockRepository.Object.GetAllAsync());

        _mockRepository.Verify(p => p.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Product_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(p => p.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _productService.DeleteProduct("naN"));
    }

}
