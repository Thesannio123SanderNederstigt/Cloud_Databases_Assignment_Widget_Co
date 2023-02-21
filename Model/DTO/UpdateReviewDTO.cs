using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateReviewDTO
{
    [OpenApiProperty(Default = "This is a wonderful product", Description = "The text of the review to update", Nullable = false)]
    public string Content { get; set; }

    [OpenApiProperty(Default = "226d555f-b0b0-4b6c-9649-5dd5eba67a74", Description = "The id of the product the review is for", Nullable = false)]
    public string ProductId { get; set; }
}