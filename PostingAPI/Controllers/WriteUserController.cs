using System.Net;
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
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;

namespace PostingAPI.Controllers;

public class WriteUserController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public WriteUserController(ILoggerFactory loggerFactory, IMapper mapper, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<WriteUserController>();
        _mapper = mapper;
        _userService = userService;
    }

    // Post user

    [Function(nameof(CreateUser))]
    [OpenApiOperation(operationId: nameof(CreateUser), tags: new[] { "Users" }, Summary = "Create a new user", Description = "Will create and return the new user.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDTO), Required = true, Description = "Data for the user that has to be created.", Example = typeof(UserExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The newly created user.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(body)!;
        User user = _mapper.Map<User>(userDTO);
        User newUser = await _userService.CreateUser(user);
        UserResponse userResponse = _mapper.Map<UserResponse>(newUser);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }

    // Update user

    [Function(nameof(UpdateUser))]
    [OpenApiOperation(operationId: nameof(UpdateUser), tags: new[] { "Users" }, Summary = "Edit a user", Description = "Allows for modification of a user.")]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDTO), Required = true, Description = "The edited user data.", Example = typeof(UserExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The updated user", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to update the user.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user to update.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the UpdateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UserDTO updateUserDTO = JsonConvert.DeserializeObject<UserDTO>(body)!;

        User updatedUser = await _userService.UpdateUser(userId, updateUserDTO);
        UserResponse updatedUserResponse = _mapper.Map<UserResponse>(updatedUser);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(updatedUserResponse);

        return res;
    }

    // Delete user

    [Function(nameof(DeleteUser))]
    [OpenApiOperation(operationId: nameof(DeleteUser), tags: new[] { "Users" }, Summary = "Delete a user", Description = "Allows for the deletion/removal of a user.")]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The user has been deleted.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the DeleteUser request.");

        await _userService.DeleteUser(userId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}