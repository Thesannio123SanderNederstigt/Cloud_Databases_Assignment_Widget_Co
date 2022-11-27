using System;
using System.Net;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using API.Attributes;
using API.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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

    // Get product image
    [FunctionName(nameof(GetProductImage))]
    [OpenApiOperation(nameof(GetProductImage), tags: new[] { "Products" })]
    [OpenApiParameter("productImageId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The product image identifier (guid)")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Redirect)]
    public static IActionResult GetProductImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/images/{productImageId}")] HttpRequest req, string productImageId,
        [Blob(blobPath: "productImagesContainer/{productImageId}", Connection = "AzureWebJobsStorage")] BlobClient blob,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger GetProductImage function processed an image retrieval request.");

        // create a blob Shared Access Signature so the image can be requested for about an hour
        BlobSasBuilder builder = new()
        {
            StartsOn = DateTime.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTime.UtcNow.AddHours(1),
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            ContentType = "image/png",
        };

        // set the permissions for the access to read only
        builder.SetPermissions(BlobAccountSasPermissions.Read);

        // create a credential to get access to the storage account shared blob file(s)
        StorageSharedKeyCredential sasKey = new(
            Environment.GetEnvironmentVariable("StorageAccountName", EnvironmentVariableTarget.Process),
            Environment.GetEnvironmentVariable("StorageAccountKey", EnvironmentVariableTarget.Process)
            );

        // create and apply the blob sas query params using the credentials and build an accessable url to get access to the product image
        BlobSasQueryParameters sas = builder.ToSasQueryParameters(sasKey);
        string url = $"{blob.Uri}?{sas}";

        // return the new redirection url to the product image file in blob storage
        return new RedirectResult(url);
    }
}
