using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model;

public class Review
{
    [OpenApiProperty(Default = "56498712-876r-091a-lm7f-98ui92uypot2", Description = "The id of a review", Nullable = false)]
    public string ReviewId { get; set; }

    [OpenApiProperty(Default = "This is a wonderful product", Description = "The text and content of the review", Nullable = false)]
    public string Content { get; set; }

    [OpenApiProperty(Default = "2022-11-12 18:00:05", Description = "The date and time a review was posted", Nullable = false)]
    public DateTime PostedOn { get; set; }

    public Product Product { get; set; }

    //product id (and maybe a user id as well? but then it won't be anonymous so... no?) is implicitly added here to the SQL DB (1:N with 1 product having many reviews (potentially))

    public Review()
    {
    }

    public Review(string reviewId, string content, DateTime postedOn, Product product)
    {
        ReviewId = reviewId;
        Content = content;
        PostedOn = postedOn;
        Product = product;
    }
}
