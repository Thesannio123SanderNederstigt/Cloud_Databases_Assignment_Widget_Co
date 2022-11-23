using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Newtonsoft.Json.Serialization;
using System;

namespace API.Examples;

public class ReviewExample : OpenApiExample<Review>
{
    public override IOpenApiExample<Review> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John's Review", new Review() { ReviewId = "43def14f-8bb6-4e69-b524-194fded56053", PostedOn = DateTime.Parse("2022-11-05 11:10:01"), Content = "This is a very good product, it works surprisingly well all the time, 10/10 would totally recommand" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary's Review", new Review() { ReviewId = "20698004-498a-45l8-1f1b-ef3554a4abe64", PostedOn = DateTime.UtcNow, Content = "This product is awful, would not buy again" }, namingStrategy));

        return this;
    }
}