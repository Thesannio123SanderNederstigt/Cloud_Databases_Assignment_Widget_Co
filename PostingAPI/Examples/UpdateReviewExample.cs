using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.DTO;
using Newtonsoft.Json.Serialization;
using System;

namespace API.Examples;

public class UpdateReviewExample : OpenApiExample<UpdateReviewDTO>
{
    public override IOpenApiExample<UpdateReviewDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John's Review", new UpdateReviewDTO() { Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommand" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary's Review", new UpdateReviewDTO() { Content = "This product is awful, would not buy again" }, namingStrategy));

        return this;
    }
}
