using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class ProductDTO
{
    [OpenApiProperty(Default = "JohnnyD#1", Description = "The productname of a product", Nullable = false)]
    public string ProductName { get; set; }

    [OpenApiProperty(Default = "9.99", Description = "The price of a product", Nullable = false)]
    public decimal Price { get; set; }

    public ProductDTO()
    {
    }

    public ProductDTO(string productName, decimal price)
    {
        ProductName = productName;
        Price = price;
    }
}
