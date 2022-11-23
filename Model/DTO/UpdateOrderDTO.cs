using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateOrderDTO
{
    [OpenApiProperty(Default = "69698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of an order", Nullable = false)]
    public string OrderId { get; set; }

    [OpenApiProperty(Default = "2022-12-12 15:00:00", Description = "The shipment date of an order", Nullable = false)]
    public DateTime ShippingDate { get; set; }

    [OpenApiProperty(Default = false, Description = "the processing status of the order", Nullable = false)]
    public bool IsProcessed { get; set; }
}
