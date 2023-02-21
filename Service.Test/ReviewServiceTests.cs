using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;
using Xunit;
namespace Service.Test;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _mockReviewRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _mockReviewRepository = new();
        _mockProductRepository = new();
        _reviewService = new ReviewService(new LoggerFactory(), _mockReviewRepository.Object, _mockProductRepository.Object);
    }

    [Fact]
    public async Task Get_All_Reviews_Should_Return_An_Array_Of_Reviews()
    {
        Review[] mockReviews = new[] {
            new Review("1", "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", DateTime.Parse("2022-11-05 11:10:01"), null!),
            new Review("2", "This product is awful, would not buy again", DateTime.UtcNow, "3"),
        };

        _mockReviewRepository.Setup(r => r.GetAllAsync()).Returns(mockReviews.ToAsyncEnumerable());

        ICollection<Review> reviews = await _reviewService.GetReviews();

        Assert.Equal(2, reviews.Count);
    }

    [Fact]
    public void Get_All_Reviews_Should_Throw_Not_Found_Exception()
    {
        _mockReviewRepository.Setup(r => r.GetAllAsync()).Returns(() => null!);

        Assert.ThrowsAsync<NotFoundException>(async () => await _reviewService.GetReviews());
    }

    [Fact]
    public async Task Get_By_Review_Id_Should_Return_Review_With_Given_Id()
    {
        _mockReviewRepository.Setup(o => o.GetByIdAsync("1")).ReturnsAsync(() => new Review("1", "This product is awful, would not buy again", DateTime.UtcNow, "3"));
        Review review = await _reviewService.GetReviewById("1");

        Assert.Equal("1", review.ReviewId);
    }

    [Fact]
    public void Get_By_Review_Id_Should_Throw_Not_Found_Exception()
    {
        _mockReviewRepository.Setup(o => o.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _reviewService.GetReviewById("naN"));
    }

    [Fact]
    public async Task Create_Review_Should_Return_A_Review_With_Review_Id()
    {
        //setup review
        _mockReviewRepository.Setup(r => r.InsertAsync(It.IsAny<Review>())).Verifiable();
        _mockReviewRepository.Setup(r => r.SaveChanges()).Verifiable();

        //setup products
        _mockProductRepository.Setup(p => p.GetByIdAsync("32698064-986w-98d1-dk8p-ef6587a4oye6")).ReturnsAsync(() => new Product("32698064-986w-98d1-dk8p-ef6587a4oye6", "Apple EarPods Lightning", 10m, null!));
        _mockProductRepository.Setup(p => p.GetByIdAsync("56487016-541l-845p-ol5f-pe86f2am98n1")).ReturnsAsync(() => new Product("56487016-541l-845p-ol5f-pe86f2am98n1", "USB-C to HDMI cable 5m", 15.99m, null!));


        ReviewDTO reviewDTO = new("This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", "32698064-986w-98d1-dk8p-ef6587a4oye6");
        Review review = await _reviewService.CreateReview(reviewDTO);

        Assert.NotNull(review.ReviewId);

        _mockReviewRepository.Verify(r => r.InsertAsync(It.IsAny<Review>()), Times.Once);
        _mockReviewRepository.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Review_Should_Have_Properties_Changed()
    {
        Review review = new("1", "This product is awful, would not buy again", DateTime.UtcNow, "3");

        _mockReviewRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => review);
        _mockReviewRepository.Setup(r => r.SaveChanges()).Verifiable();

        UpdateReviewDTO changes = new() { Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", ProductId = "226d555f-b0b0-4b6c-9649-5dd5eba67a74" };
        await _reviewService.UpdateReview("1", changes);

        Assert.Equal("1", review.ReviewId);
        Assert.Equal("This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", review.Content);

        Assert.NotNull(review.ProductId);

        _mockReviewRepository.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_Review_Should_Throw_Not_Found_Exception()
    {
        _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("review"));

        UpdateReviewDTO changes = new() { Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", ProductId = "226d555f-b0b0-4b6c-9649-5dd5eba67a74" };

        await Assert.ThrowsAsync<NotFoundException>(() => _reviewService.UpdateReview("1", changes));
    }

    [Fact]
    public async Task Delete_Review_Should_Delete_The_Review()
    {
        Review review = new("1", "This product is awful, would not buy again", DateTime.UtcNow, "3");

        _mockReviewRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => review);
        _mockReviewRepository.Setup(r => r.SaveChanges()).Verifiable();

        await _reviewService.DeleteReview("1");

        Assert.DoesNotContain(_mockReviewRepository.Object.ToString(), review.ReviewId);
        Assert.Null(_mockReviewRepository.Object.GetAllAsync());

        _mockReviewRepository.Verify(o => o.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_Review_Should_Throw_Not_Found_Exception()
    {
        _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _reviewService.DeleteReview("naN"));
    }
}