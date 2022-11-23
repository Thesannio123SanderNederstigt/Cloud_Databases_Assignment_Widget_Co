using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model;

public class Product
{
    [OpenApiProperty(Default = "32698064-986w-98d1-dk8p-ef6587a4oye6", Description = "The id of a product", Nullable = false)]
    public string ProductId { get; set; }

    [OpenApiProperty(Default = "USB-C to HDMI cable 5m", Description = "The productname of a product", Nullable = false)]
    public string ProductName { get; set; }

    //public string? ImageId { get; set; } //voor nu enkele image (handmatig uploaden naar blob storage lokaal?), bij meerdere images dan apart Image model maken en hier een ICollection van maken zodat een image een productId krijgt!

    [OpenApiProperty(Default = "14.99", Description = "The price of a product", Nullable = false)]
    public decimal Price { get; set; }

    // A product can be part of an order (or not) and can exist in MANY different orders at the same time (so adding a back ref collection here to ensure a connecting table can be created for this N:N relationship between products and orders)
    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }

    public Product()
    {
    }

    public Product(string productId, string productName, decimal price, ICollection<Review> reviews)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Reviews = reviews;
    }

    //collection van reviews (en collection van orders, want ook al kan dit null zijn, een product kan in meerdere orders voor komen, of in geen) een product kan in VEEL orders voorkomen!
}
