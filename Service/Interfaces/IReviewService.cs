using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IReviewService
{
    public Task<ICollection<Review>> GetReviews();

    Task<Review> GetReviewById(string reviewId);

    Task<Review> CreateReview(ReviewDTO reviewDTO);

    Task<Review> UpdateReview(string reviewId, UpdateReviewDTO updateReviewDTO);

    Task DeleteReview(string reviewId);
}
