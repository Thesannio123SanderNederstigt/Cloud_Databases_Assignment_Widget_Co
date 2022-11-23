using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IProductService
{
    public Task<ICollection<Product>> GetProducts();

    Task<Product> GetProductById(string productId);

    Task<Product> CreateProduct(Product product);

    Task<Product> UpdateProduct(string productId, UpdateProductDTO updateProductDTO);

    Task DeleteProduct(string productId);
}
