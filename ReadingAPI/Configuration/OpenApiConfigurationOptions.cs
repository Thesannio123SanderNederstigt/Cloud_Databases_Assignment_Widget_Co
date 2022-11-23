using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System;

namespace API.Configuration;
class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo {
        Version = "1.0",
        Title = "Widget & Co Read API Specification",
        Description = "<h3>Description</h3> This contains the API models, object schemas and endpoint specifications of the Widget & Co Cloud Databases assignment platform Application. This function app contains the read endpoints in an attempt to seperate the write and read functionalities of this online store application. <br><br> This project was made by Sander Nederstigt (577208) as an assignment for the Cloud Databases Course (first assignment: Online Store)",
        License = new OpenApiLicense {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };
    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
}
