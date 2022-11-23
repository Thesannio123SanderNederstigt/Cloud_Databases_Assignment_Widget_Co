using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class OrderDTO
{
    [OpenApiProperty(Default = "69698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of an order", Nullable = false)]
    public string OrderId { get; set; }

    [OpenApiProperty(Default = "29.99", Description = "The total cost amount of the order", Nullable = false)]
    public decimal Total { get; set; }

    [OpenApiProperty(Default = new string[] { "32698064-986w-98d1-dk8p-ef6587a4oye6", "56487016-541l-845p-ol5f-pe86f2am98n1" }, Description = "The list of products (ProductId's) in the order", Nullable = false)]
    public string[] ProductIds { get; set; }

    [OpenApiProperty(Default = "06698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of the user placing the order", Nullable = false)]
    public string UserId { get; set; }

    public OrderDTO()
    {
    }

    public OrderDTO(string orderId, decimal total, string[] productIds, string userId)
    {
        OrderId = orderId;
        Total = total;
        ProductIds = productIds;
        UserId = userId;
    }
}