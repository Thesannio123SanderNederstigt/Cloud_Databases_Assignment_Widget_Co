using Model;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;

public interface IProductService
{
    public Task<ICollection<ProductResponse>> GetProducts();

    Task<ProductResponse> GetProductResById(string productId);

    Task<Product> GetProductById(string productId);

    Task<Product> CreateProduct(Product product);

    Task<Product> UpdateProduct(string productId, UpdateProductDTO updateProductDTO);

    Task DeleteProduct(string productId);
}