using System.IO;
using System;
using System.Net;
using System.Threading.Tasks;
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
using System.Collections.Generic;
using System.Linq;
using Service;

namespace ReadingAPI.Controllers;

public class ReadReviewController
{
    private readonly ILogger _logger;
    private readonly IReviewService _reviewService;

    public ReadReviewController(ILoggerFactory loggerFactory, IReviewService reviewService)
    {
        _logger = loggerFactory.CreateLogger<ReadReviewController>();
        _reviewService = reviewService;
    }

    // Get reviews

    [Function(nameof(GetReviews))]
    [OpenApiOperation(operationId: nameof(GetReviews), tags: new[] { "Reviews" }, Summary = "A list of product reviews", Description = "Will return a list of product reviews.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Review[]), Description = "A list of reviews.", Example = typeof(ReviewExample))]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of product reviews.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetReviews([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetReviews request.");

        ICollection<Review> reviews = await _reviewService.GetReviews();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(reviews);

        return res;
    }

    // Get review

    [Function(nameof(GetReviewById))]
    [OpenApiOperation(operationId: nameof(GetReviewById), tags: new[] { "Reviews" }, Summary = "A single product review", Description = "Will return a specified product review.")]
    [OpenApiParameter(name: "reviewId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The product id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Review), Description = "A single retrieved product review.", Example = typeof(ReviewExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve the product review.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the product review.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetReviewById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{reviewId}")] HttpRequestData req,
        string reviewId)
    {

        _logger.LogInformation("C# HTTP trigger function processed the GetReviewById request.");

        Review review = await _reviewService.GetReviewById(reviewId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(review);

        return res;
    }
}