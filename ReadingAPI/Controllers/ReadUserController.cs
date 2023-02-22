using API.Attributes;
using API.Examples;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Service.Interfaces;
using System.Net;

namespace ReadingAPI.Controllers;

public class ReadUserController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ReadUserController(ILoggerFactory loggerFactory, IMapper mapper, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<ReadUserController>();
        _mapper = mapper;
        _userService = userService;
    }

    // Get users

    [Function(nameof(GetUsers))]
    [OpenApiOperation(operationId: nameof(GetUsers), tags: new[] { "Users" }, Summary = "A list of users", Description = "Will return a list of users.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse[]), Description = "A list of users.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of users.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<User> users = await _userService.GetUsers();
        IEnumerable<UserResponse> userResponses = users.Select(u => _mapper.Map<UserResponse>(u));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponses);

        return res;
    }

    // Get user

    [Function(nameof(GetUserById))]
    [OpenApiOperation(operationId: nameof(GetUserById), tags: new[] { "Users" }, Summary = "A single user", Description = "Will return a specified user's info")]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "A single retrieved user.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve the user.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUser request.");

        User user = await _userService.GetUserById(userId);

        // map retrieved user to the UserRepsonse model
        UserResponse userResponse = _mapper.Map<UserResponse>(user);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }
}