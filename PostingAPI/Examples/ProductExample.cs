using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ProductExample : OpenApiExample<ProductResponse>
{
    public override IOpenApiExample<ProductResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Earpods", new ProductResponse() { ProductId = "43def14f-8bb6-4e69-b524-194fded56053", ProductName = "Apple EarPods Lightning", Price = 10m }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Cable", new ProductResponse() { ProductId = "20698004-498a-45l8-1f1b-ef3554a4abe64", ProductName = "USB-C to HDMI cable 5m", Price = 15.99m }, namingStrategy));

        return this;
    }
}