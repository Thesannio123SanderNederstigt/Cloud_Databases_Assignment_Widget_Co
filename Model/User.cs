using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model;

public class User
{
    // model attributes and/or properties
    [OpenApiProperty(Default = "06698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of a user", Nullable = false)]
    public string UserId { get; set; }

    [OpenApiProperty(Default = "mail@gmail.com", Description = "The email address of a user", Nullable = false)]
    public string Email { get; set; }

    [OpenApiProperty(Default = "JohnnyD#1", Description = "The username of a user", Nullable = false)]
    public string UserName { get; set; }

    [OpenApiProperty(Default = "J0hnny#123!", Description = "The password of a user", Nullable = false)]
    public string Password { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    //public virtual ICollection<Review>? Reviews { get; set; }

    // empty constructor
    public User()
    {
    }

    // full constructor
    public User(string userId, string email, string userName, string password, ICollection<Order> orders)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
        Password = password;
        Orders = orders;
    }
}