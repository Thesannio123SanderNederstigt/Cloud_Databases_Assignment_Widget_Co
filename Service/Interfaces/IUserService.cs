using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IUserService
{
    public Task<ICollection<User>> GetUsers();

    Task<User> GetUserById(string userId);

    Task<User> CreateUser(User user);

    Task<User> UpdateUser(string userId, UserDTO userDTO);

    Task DeleteUser(string userId);
}