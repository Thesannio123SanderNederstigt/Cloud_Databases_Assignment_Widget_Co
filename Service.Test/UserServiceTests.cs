using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        ServiceProvider services = new ServiceCollection()
        .AddScoped<API.Mappings.UserConverter>()
        .BuildServiceProvider();

        _mockRepository = new();
        _userService = new UserService(new LoggerFactory(), _mockRepository.Object);
    }

    [Fact]
    public async Task Get_All_Users_Should_Return_An_Array_Of_Users()
    {
        User[] mockUsers = new[] {
            new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!),
            new User("2", "mruisberg@mail.com", "Mjanneke34", "MJ2U#2", null!),
        };

        _mockRepository.Setup(u => u.GetAllAsync()).Returns(mockUsers.ToAsyncEnumerable());

        ICollection<User> users = await _userService.GetUsers();

        Assert.Equal(2, users.Count);
    }

    [Fact]
    public void Get_All_Users_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetAllAsync()).Returns(() => null!);

        Assert.ThrowsAsync<NotFoundException>(async () => await _userService.GetUsers());
    }

    [Fact]
    public async Task Get_By_User_Id_Should_Return_User_With_Given_Id()
    {
        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!));
        User user = await _userService.GetUserById("1");

        Assert.Equal("1", user.UserId);
    }

    [Fact]
    public void Get_By_User_Id_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _userService.GetUserById("naN"));
    }

    [Fact]
    public async Task Create_User_Should_Return_A_User_With_User_Id()
    {
        _mockRepository.Setup(u => u.InsertAsync(It.IsAny<User>())).Verifiable();
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        User newUser = new();
        User user = await _userService.CreateUser(newUser);

        Assert.NotNull(user.UserId);

        _mockRepository.Verify(u => u.InsertAsync(It.IsAny<User>()), Times.Once);
        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_User_Should_Have_Properties_Changed()
    {
        User user = new ("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!);

        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => user);
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        UserDTO changes = new() { Email = "testemail@mail.com", UserName = "HarryG#205!", Password = "H4rr1Dman#28" };
        await _userService.UpdateUser("1", changes);

        Assert.Equal("testemail@mail.com", user.Email);
        Assert.Equal("HarryG#205!", user.UserName);
        Assert.Equal("H4rr1Dman#28", user.Password);

        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_User_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("user"));

        UserDTO changes = new() { Email = "testemail@mail.com", UserName = "HarryG#205!", Password = "H4rr1Dman#28" };

        await Assert.ThrowsAsync<NotFoundException>(() => _userService.UpdateUser("1", changes));
    }

    [Fact]
    public async Task Delete_User_Should_Delete_The_User()
    {
        User user = new User("1", "hdevries@mail.com", "HFreeze#902", "HFr33zing#1!", null!);

        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => user);
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        await _userService.DeleteUser("1");

        Assert.DoesNotContain(_mockRepository.Object.ToString(), user.UserId);
        Assert.Null(_mockRepository.Object.GetAllAsync());

        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_User_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _userService.DeleteUser("naN"));
    }
}