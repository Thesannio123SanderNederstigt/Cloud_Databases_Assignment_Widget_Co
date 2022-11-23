using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.WindowsAzure.Storage.Table;

namespace Model;

public class Order : TableEntity
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

    public User User { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    // empty constructor
    public Order()
    {
    }

    // full constructor
    public Order(string orderId, DateTime orderDate, DateTime? shippingDate, decimal total, bool isProcessed, User user, ICollection<Product> products)
    {
        OrderId = orderId;
        OrderDate = orderDate;
        ShippingDate = shippingDate;
        Total = total;
        IsProcessed = isProcessed;
        User = user;
        Products = products;

        PartitionKey = User.UserId;
        RowKey = orderId;
    }

}
