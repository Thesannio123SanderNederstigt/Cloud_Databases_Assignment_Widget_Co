using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response;
public class OrderResponse
{
    // attributes/properties
    [OpenApiProperty(Default = "69698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of an order", Nullable = false)]
    public string OrderId { get; set; }

    [OpenApiProperty(Default = "2022-11-05 11:00:00", Description = "The date an order was placed on", Nullable = false)]
    public DateTime OrderDate { get; set; }

    [OpenApiProperty(Default = "2022-12-12 15:00:00", Description = "The shipment date of an order", Nullable = true)]
    public DateTime? ShippingDate { get; set; }

    [OpenApiProperty(Default = "29.99", Description = "The total cost amount of the order", Nullable = false)]
    public decimal Total { get; set; }

    [OpenApiProperty(Default = false, Description = "the processing status of the order", Nullable = false)]
    public bool IsProcessed { get; set; }

    public string UserId { get; set; }

    public virtual ICollection<ProductResponse> Products { get; set; }

    // empty constructor
    public OrderResponse()
    {
    }

    // full constructor
    public OrderResponse(string orderId, DateTime orderDate, DateTime? shippingDate, decimal total, bool isProcessed, string userId, ICollection<ProductResponse> products)
    {
        OrderId = orderId;
        OrderDate = orderDate;
        ShippingDate = shippingDate;
        Total = total;
        IsProcessed = isProcessed;
        UserId = userId;
        Products = products;
    }
}