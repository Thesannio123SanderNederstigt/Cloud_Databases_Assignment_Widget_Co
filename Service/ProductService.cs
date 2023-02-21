using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Exceptions;
using AutoMapper;
using Model.Response;

namespace Service;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(ILoggerFactory loggerFactory, IProductRepository productRepository, IMapper mapper)
    {
        _logger = loggerFactory.CreateLogger<ProductService>();
        _productRepository = productRepository;
        _mapper = mapper;
    }

    //query aggregation in order to include the reviews of the products
    private IIncludableRepository<Product, Review> Query() => _productRepository
    .Include(p => p.Reviews);

    // get product responses
    public async Task<ICollection<ProductResponse>> GetProducts()
    {
        return await Query().GetAll().Select(p => _mapper.Map<ProductResponse>(p)).ToArrayAsync() ?? throw new NotFoundException("products");
    }

    // get product response 
    public async Task<ProductResponse> GetProductResById(string productId)
    {
        Product product = await Query().GetBy(p => p.ProductId == productId) ?? throw new NotFoundException("product");
        return _mapper.Map<ProductResponse>(product);
    }

    // get product
    public async Task<Product> GetProductById(string productId)
    {
        return await _productRepository.GetByIdAsync(productId) ?? throw new NotFoundException("product");
    }

    // create a new product
    public async Task<Product> CreateProduct(Product product)
    {
        //Product product = new();

        product.ProductId = Guid.NewGuid().ToString();

        /*product.ProductName = productDTO.ProductName;
        product.Price = productDTO.Price;*/

        await _productRepository.InsertAsync(product);
        await _productRepository.SaveChanges();

        return product;
    }

    // update a product (using a different UpdateProductDTO, so a name or price can be altered seperately
    public async Task<Product> UpdateProduct(string productId, UpdateProductDTO changes)
    {
        Product product = await GetProductById(productId) ?? throw new NotFoundException("product to update");

        product.ProductName = changes.ProductName ?? product.ProductName;
        product.Price = changes.Price ?? product.Price;

        await _productRepository.SaveChanges();
        return product;
    }

    // delete a product
    public async Task DeleteProduct(string productId)
    {
        // retrieve and then remove a product
        Product product = await GetProductById(productId) ?? throw new NotFoundException("product to delete");

        _productRepository.Remove(product);

        await _productRepository.SaveChanges();

        _logger.LogInformation($"Delete order function deleted order: {product.ProductId}");
    }
}
