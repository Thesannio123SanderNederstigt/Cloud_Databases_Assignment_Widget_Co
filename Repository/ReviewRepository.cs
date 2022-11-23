using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class ReviewRepository : Repository<Review, string>, IReviewRepository
{
    public ReviewRepository(DataContext context) : base(context, context.Reviews)
    {

    }
}
