using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Exceptions;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UserService(ILoggerFactory loggerFactory, IUserRepository userRepository)
        {
            _logger = loggerFactory.CreateLogger<UserService>();
            _userRepository = userRepository;
        }

        // get users
        public async Task<ICollection<User>> GetUsers()
        {
            return await _userRepository.GetAllAsync().ToArrayAsync() ?? throw new NotFoundException("users");
        }

        // get user
        public async Task<User> GetUserById(string userId)
        {
            return await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("user");
        }

        // create a new user
        public async Task<User> CreateUser(User user)
        {
            user.UserId = Guid.NewGuid().ToString();

            await _userRepository.InsertAsync(user);
            await _userRepository.SaveChanges();

            return user;
        }

        // update a user
        public async Task<User> UpdateUser(string userId, UserDTO changes)
        {
            User user = await GetUserById(userId) ?? throw new NotFoundException("user to update");

            user.Email = changes.Email ?? user.Email;
            user.UserName = changes.UserName ?? user.UserName;
            user.Password = changes.Password ?? user.Password;

            await _userRepository.SaveChanges();
            return user;
        }

        // delete a user
        public async Task DeleteUser(string userId)
        {
            // retrieve user
            User user = await GetUserById(userId) ?? throw new NotFoundException("user to delete");
            
            _userRepository.Remove(user);
                    
            await _userRepository.SaveChanges();

            _logger.LogInformation($"Delete user function deleted user {user.UserName} with id: {user.UserId}");
        }

    }
}