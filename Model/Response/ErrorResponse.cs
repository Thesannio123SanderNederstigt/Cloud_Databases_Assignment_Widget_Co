using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.Response;

public class ErrorResponse
{
    [JsonRequired]
    [OpenApiProperty(Default = "Object reference not set to an instance of an object", Description = "The message of an error", Nullable = false)]
    public string Message { get; set; }

    [OpenApiProperty(Default = "Service", Description = "The source of an occured error", Nullable = true)]
    public string? Source { get; set; }

    [OpenApiProperty(Default = "Service.UserService.<CreateUser>d__35.MoveNext() in S:\\CD_repo\\CloudDatabases\\Service\\UserService.cs:line 41", Description = "The stacktrace of the occured error", Nullable = true)]
    public string? StackTrace { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message)
    {
        Message = message;
    }

    public ErrorResponse(Exception ex)
    {
        Message = ex.Message;
        Source = ex.Source;
        StackTrace = ex.StackTrace;
    }
}
