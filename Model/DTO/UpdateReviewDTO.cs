using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateReviewDTO
{
    [OpenApiProperty(Default = "This is a wonderful product", Description = "The text of the review to update", Nullable = false)]
    public string Content { get; set; }
}