using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;


namespace API.Examples;

public class UpdateProductExample : OpenApiExample<UpdateProductDTO>
{
    public override IOpenApiExample<UpdateProductDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("only the name", new UpdateProductDTO() { ProductName = "Apple EarPods Lightning" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("only the price", new UpdateProductDTO() { ProductName = "USB-C to HDMI cable 2m", Price = 9.99m }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("both the name and price", new UpdateProductDTO() { ProductName = "USB-C to HDMI cable 2m", Price = 9.99m }, namingStrategy));

        return this;
    }
}
