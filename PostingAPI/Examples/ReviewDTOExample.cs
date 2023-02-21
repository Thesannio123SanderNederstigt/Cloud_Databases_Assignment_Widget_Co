using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ReviewDTOExample : OpenApiExample<ReviewDTO>
{
    public override IOpenApiExample<ReviewDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John's Review", new ReviewDTO() { Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommend", ProductId = "754c03df-a87e-4f1d-8637-59b8f30e6664" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary's Review", new ReviewDTO() { Content = "This product is awful, would not buy again", ProductId = "43ccb5a5-56a6-4182-b318-243d200d9a30" }, namingStrategy));

        return this;
    }
}