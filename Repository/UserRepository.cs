using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class UserRepository : Repository<User, string>, IUserRepository
{
    public UserRepository(DataContext context) : base(context, context.Users)
    {
    }
}