using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace API.Mappings;

public class ReviewConverter : ITypeConverter<ReviewDTO, Task<Review>>
{
    private readonly IProductService _productService;

    public ReviewConverter(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Review> Convert(ReviewDTO source, Task<Review> destination, ResolutionContext context)
    {
        return new()
        {
            ReviewId = Guid.NewGuid().ToString(),
            Content = source.Content,
            PostedOn = DateTime.UtcNow,
            Product = await _productService.GetProductById(source.ProductId)
        };
    }
}