using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Exceptions;
using Repository;

namespace Service;

public class ReviewService : IReviewService
{
    private readonly ILogger<ReviewService> _logger;
    private readonly IReviewRepository _reviewRepository;
    private readonly IProductRepository _productRepository;

    public ReviewService(ILoggerFactory loggerFactory, IReviewRepository reviewRepository, IProductRepository productRepository)
    {
        _logger = loggerFactory.CreateLogger<ReviewService>();
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
    }

    // get reviews
    public async Task<ICollection<Review>> GetReviews()
    {
        return await _reviewRepository.GetAllAsync().ToArrayAsync() ?? throw new NotFoundException("reviews");
    }

    // get review
    public async Task<Review> GetReviewById(string reviewId)
    {
        return await _reviewRepository.GetByIdAsync(reviewId) ?? throw new NotFoundException("review");
    }

    // create a new review
    public async Task<Review> CreateReview(ReviewDTO reviewDTO)
    {
        //removed/commented this out because this is now already done by the ReviewConverter mapper... (it literally does the same exact thing so why do it again here? pretty pointless...)

        // ensure the product exists (or throw an exception if it doesn't)
        /*Product reviewProduct = await _productRepository.GetByIdAsync(reviewDTO.ProductId) ?? throw new NotFoundException("product to review");

        Review review = new();

        review.Product = reviewProduct;*/

        Review review = new();

        review.ReviewId = Guid.NewGuid().ToString();

        review.Content = reviewDTO.Content;
        review.Product = await _productRepository.GetByIdAsync(reviewDTO.ProductId) ?? throw new NotFoundException("product to review");

        review.PostedOn = DateTime.UtcNow;

        await _reviewRepository.InsertAsync(review);
        await _reviewRepository.SaveChanges();

        return review;
    }

    // update a review (using a UpdateReviewDTO to only update the text/content of a review)
    public async Task<Review> UpdateReview(string reviewId, UpdateReviewDTO changes)
    {
        Review review = await GetReviewById(reviewId) ?? throw new NotFoundException("review to update");

        review.Content = changes.Content;

        review.PostedOn = DateTime.UtcNow;

        await _reviewRepository.SaveChanges();
        return review;
    }

    // delete a review
    public async Task DeleteReview(string reviewId)
    {
        // retrieve and then remove a review
        Review review = await GetReviewById(reviewId) ?? throw new NotFoundException("review to delete");

        _reviewRepository.Remove(review);

        await _reviewRepository.SaveChanges();

        _logger.LogInformation($"Delete order function deleted review: {review.ReviewId}");
    }
}
