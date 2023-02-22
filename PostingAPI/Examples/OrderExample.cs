using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class OrderExample : OpenApiExample<OrderDTO>
{
    public override IOpenApiExample<OrderDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John's order", new OrderDTO() { ProductIds = new string[] { "43def14f-8bb6-4e69-b524-194fded56053", "56487016-541l-845p-ol5f-pe86f2am98n1" }, UserId = "20698004-498a-45l8-1f1b-ef3554a4abe64" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary's order", new OrderDTO() { ProductIds = new string[] { "32698064-986w-98d1-dk8p-ef6587a4oye6", "06698004-438a-46e8-b81b-ef3507a4abe5" }, UserId = "06698004-438a-46e8-b81b-ef3507a4abe5" }, namingStrategy));

        return this;
    }
}