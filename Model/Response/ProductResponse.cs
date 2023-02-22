using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.Response;
public class ProductResponse
{
    [OpenApiProperty(Default = "32698064-986w-98d1-dk8p-ef6587a4oye6", Description = "The id of a product", Nullable = false)]
    public string ProductId { get; set; }

    [OpenApiProperty(Default = "USB-C to HDMI cable 5m", Description = "The productname of a product", Nullable = false)]
    public string ProductName { get; set; }

    [OpenApiProperty(Default = "14.99", Description = "The price of a product", Nullable = false)]
    public decimal Price { get; set; }

    // a healthy alternative to this would be to only return review ids or no reviews at all (or limit this in any other way) in order to prevent a ton of data being returned when a product gets a lot of reviews later on
    public virtual ICollection<Review> Reviews { get; set; }

    public ProductResponse()
    {
    }

    public ProductResponse(string productId, string productName, decimal price, ICollection<Review> reviews)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Reviews = reviews;
    }
}