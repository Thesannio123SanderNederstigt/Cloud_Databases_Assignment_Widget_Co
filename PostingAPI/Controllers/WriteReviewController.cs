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
using Newtonsoft.Json;
using Service.Interfaces;

namespace PostingAPI.Controllers;

public class WriteReviewController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IReviewService _reviewService;

    public WriteReviewController(ILoggerFactory loggerFactory, IMapper mapper, IReviewService reviewService)
    {
        _logger = loggerFactory.CreateLogger<WriteReviewController>();
        _mapper = mapper;
        _reviewService = reviewService;
    }

    // Post review

    [Function(nameof(CreateReview))]
    [OpenApiOperation(operationId: nameof(CreateReview), tags: new[] { "Reviews" }, Summary = "Create a new review", Description = "Will create and return the new review.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ReviewDTO), Required = true, Description = "Data for the review that has to be created.", Example = typeof(ReviewDTOExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Review), Description = "The newly created review.", Example = typeof(ReviewExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the review.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateReview([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "review")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateReview request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(body)!;

        //Review review = await _mapper.Map<Review>(reviewDTO);
        Review newReview = await _reviewService.CreateReview(reviewDTO);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(newReview);

        return res;
    }

    // Update review

    [Function(nameof(UpdateReview))]
    [OpenApiOperation(operationId: nameof(UpdateReview), tags: new[] { "Reviews" }, Summary = "Edit a review", Description = "Allows for modification of a review.")]
    [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The review id parameter.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateReviewDTO), Required = true, Description = "The edited review data.", Example = typeof(UpdateReviewExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Review), Description = "The updated review", Example = typeof(ReviewExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to update the review.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the review to update.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "review/{reviewId}")] HttpRequestData req,
        string reviewId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the UpdateReview request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateReviewDTO updateReviewDTO = JsonConvert.DeserializeObject<UpdateReviewDTO>(body)!;
        
        Review updatedReview = await _reviewService.UpdateReview(reviewId, updateReviewDTO);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(updatedReview);

        return res;
    }

    // Delete review

    [Function(nameof(DeleteReview))]
    [OpenApiOperation(operationId: nameof(DeleteReview), tags: new[] { "Reviews" }, Summary = "Delete a review", Description = "Allows for the deletion/removal of a review.")]
    [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The review id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The review has been deleted.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the review.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteReview(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "reviews/{reviewId}")] HttpRequestData req,
        string reviewId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the DeleteReview request.");

        await _reviewService.DeleteReview(reviewId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}