using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UserDTO
{
    [OpenApiProperty(Default = "mail@gmail.com", Description = "The email address of a user", Nullable = false)]
    public string Email { get; set; }

    [OpenApiProperty(Default = "JohnnyD#1", Description = "The username of a user", Nullable = false)]
    public string UserName { get; set; }

    [OpenApiProperty(Default = "J0hnny#123!", Description = "The password of a user", Nullable = false)]
    public string Password { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(string email, string userName, string password)
    {
        Email = email;
        UserName = userName;
        Password = password;
    }
}