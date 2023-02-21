using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ProductDTOExample : OpenApiExample<ProductDTO>
{
    public override IOpenApiExample<ProductDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("EarPods", new ProductDTO() { ProductName = "Apple EarPods Lightning", Price = 14.99m }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Cable", new ProductDTO() { ProductName = "USB-C to HDMI cable 2m", Price = 9.99m }, namingStrategy));

        return this;
    }
}
