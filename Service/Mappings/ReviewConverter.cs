using AutoMapper;
using Model;
using Model.DTO;

namespace API.Mappings;

public class ReviewConverter : ITypeConverter<ReviewDTO, Review>
{
    public Review Convert(ReviewDTO source, Review destination, ResolutionContext context)
    {
        return new()
        {
            ReviewId = Guid.NewGuid().ToString(),
            Content = source.Content,
            PostedOn = DateTime.UtcNow
        };
    }
}