using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class OrderDTO
{
    public string? OrderId { get; set; }

    [OpenApiProperty(Default = "32698064-986w-98d1-dk8p-ef6587a4oye6", Description = "The list of products (ProductId's) in the order", Nullable = false)]
    public string[] ProductIds { get; set; }

    [OpenApiProperty(Default = "06698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of the user placing the order", Nullable = false)]
    public string UserId { get; set; }

    public OrderDTO()
    {
    }

    public OrderDTO(string? orderId, string[] productIds, string userId)
    {
        OrderId = orderId;
        ProductIds = productIds;
        UserId = userId;
    }
}