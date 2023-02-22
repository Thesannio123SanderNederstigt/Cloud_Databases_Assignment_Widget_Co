using AutoMapper;
using Model;
using Model.DTO;

namespace API.Mappings;

public class ProductConverter : ITypeConverter<ProductDTO, Product>
{
    public Product Convert(ProductDTO source, Product destination, ResolutionContext context)
    {
        return new()
        {
            ProductId = Guid.NewGuid().ToString(),
            ProductName = source.ProductName,
            Price = source.Price
        };
    }
}