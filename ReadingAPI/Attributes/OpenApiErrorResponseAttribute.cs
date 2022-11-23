using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Model.Response;

namespace API.Attributes;

public class OpenApiErrorResponseAttribute : OpenApiResponseWithBodyAttribute
{
    public OpenApiErrorResponseAttribute(HttpStatusCode statusCode)
        : base(statusCode, "application/json", typeof(ErrorResponse))
    {
    }
}