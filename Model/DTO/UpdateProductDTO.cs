using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateProductDTO
{
    [OpenApiProperty(Default = "JohnnyD#1", Description = "The productname of a product to update", Nullable = true)]
    public string? ProductName { get; set; }

    [OpenApiProperty(Default = "9.99", Description = "The price of a product to update", Nullable = true)]
    public decimal? Price { get; set; }
}