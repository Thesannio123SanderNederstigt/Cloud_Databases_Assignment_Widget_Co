using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ReviewDTOExample : OpenApiExample<ReviewDTO>
{
    public override IOpenApiExample<ReviewDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John's Review", new ReviewDTO() { Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommand" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary's Review", new ReviewDTO() { Content = "This product is awful, would not buy again" }, namingStrategy));

        return this;
    }
}