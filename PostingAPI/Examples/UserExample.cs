using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UserExample : OpenApiExample<UserDTO>
{
    public override IOpenApiExample<UserDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UserDTO() { Email = "John@gmail.com", UserName = "JohnD#1", Password = "J0nh#001!" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UserDTO() { Email = "Mary@gmail.com", UserName = "MarySue#22", Password = "M4rySu3san#22!" }, namingStrategy));

        return this;
    }
}