using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.Response;

public class UserResponse
{
    [OpenApiProperty(Default = "06698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of a user", Nullable = false)]
    public string UserId { get; set; }

    [OpenApiProperty(Default = "mail@gmail.com", Description = "The email address of an user", Nullable = false)]
    public string Email { get; set; }

    [OpenApiProperty(Default = "JohnnyD#1", Description = "The username of a user", Nullable = false)]
    public string UserName { get; set; }

    public UserResponse()
    {
    }

    public UserResponse(string userId, string email, string userName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
    }
}
