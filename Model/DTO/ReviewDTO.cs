using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class ReviewDTO
{
    [OpenApiProperty(Default = "This is a wonderful product", Description = "The text and content of the review", Nullable = false)]
    public string Content { get; set; }

    [OpenApiProperty(Default = "32698064-986w-98d1-dk8p-ef6587a4oye6", Description = "The id of a product the review is of or for", Nullable = false)]
    public string ProductId { get; set; }

    public ReviewDTO()
    {
    }

    public ReviewDTO(string content, string productId)
    {
        Content = content;
        ProductId = productId;
    }
}
