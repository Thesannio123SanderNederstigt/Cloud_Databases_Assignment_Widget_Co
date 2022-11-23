using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UserResponseExample : OpenApiExample<UserResponse>
{
    public override IOpenApiExample<UserResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UserResponse() { UserId = "a75e3fe7-f519-48de-a106-79f788a1b479", Email = "jan@gmail.com", UserName = "JanG#1" }, namingStrategy));

        return this;
    }
}
