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

    [OpenApiProperty(Default = "226d555f-b0b0-4b6c-9649-5dd5eba67a74", Description = "The id of the product the review is for", Nullable = false)]
    public string ProductId { get; set; }

    public Review()
    {
    }

    public Review(string reviewId, string content, DateTime postedOn, string productId)
    {
        ReviewId = reviewId;
        Content = content;
        PostedOn = postedOn;
        ProductId = productId;
    }
}