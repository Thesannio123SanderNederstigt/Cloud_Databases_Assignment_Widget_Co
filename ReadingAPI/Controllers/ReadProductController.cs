using System.Net;
using API.Attributes;
using API.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Service.Interfaces;

namespace ReadingAPI.Controllers;

public class ReadProductController
{
    private readonly ILogger _logger;
    private readonly IProductService _productService;

    public ReadProductController(ILoggerFactory loggerFactory, IProductService productService)
    {
        _logger = loggerFactory.CreateLogger<ReadProductController>();
        _productService = productService;
    }

    // Get products

    [Function(nameof(GetProducts))]
    [OpenApiOperation(operationId: nameof(GetProducts), tags: new[] { "Products" }, Summary = "A list of products", Description = "Will return a list of products.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product[]), Description = "A list of products.", Example = typeof(ProductExample))]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of products.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetProducts request.");

        ICollection<Product> products = await _productService.GetProducts();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(products);

        return res;
    }

    // Get product

    [Function(nameof(GetProductById))]
    [OpenApiOperation(operationId: nameof(GetProductById), tags: new[] { "Products" }, Summary = "A single product", Description = "Will return a specified product.")]
    [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The product id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "A single retrieved product.", Example = typeof(ProductExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve the product.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the product.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{productId}")] HttpRequestData req,
        string productId)
    {

        _logger.LogInformation("C# HTTP trigger function processed the GetProductById request.");

        Product product = await _productService.GetProductById(productId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(product);

        return res;
    }
}
