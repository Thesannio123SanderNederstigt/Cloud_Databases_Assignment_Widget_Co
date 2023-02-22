using System.Net;
using API.Attributes;
using API.Examples;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
using AuthorizationLevel = Microsoft.Azure.Functions.Worker.AuthorizationLevel;
using HttpMultipartParser;

namespace PostingAPI.Controllers;

public class WriteProductController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public WriteProductController(ILoggerFactory loggerFactory, IMapper mapper, IProductService productService)
    {
        _logger = loggerFactory.CreateLogger<WriteProductController>();
        _mapper = mapper;
        _productService = productService;
    }

    // Post product

    [Function(nameof(CreateProduct))]
    [OpenApiOperation(operationId: nameof(CreateProduct), tags: new[] { "Products" }, Summary = "Create a new product", Description = "Will create and return the new product.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ProductDTO), Required = true, Description = "Data for the product that has to be created.", Example = typeof(ProductDTOExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The newly created product.", Example = typeof(ProductExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the product.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateProduct request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(body)!;

        Product product = _mapper.Map<Product>(productDTO);
        Product newProduct = await _productService.CreateProduct(product);

        ProductResponse productResponse = _mapper.Map<ProductResponse>(newProduct);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(productResponse);

        return res;
    }

    // Update product

    [Function(nameof(UpdateProduct))]
    [OpenApiOperation(operationId: nameof(UpdateProduct), tags: new[] { "Products" }, Summary = "Edit a product", Description = "Allows for modification of a product.")]
    [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The product id parameter.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateProductDTO), Required = true, Description = "The edited product data.", Example = typeof(UpdateProductExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Product), Description = "The updated product", Example = typeof(ProductExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to update the product.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the product to update.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{productId}")] HttpRequestData req,
        string productId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the UpdateProduct request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateProductDTO updateProductDTO = JsonConvert.DeserializeObject<UpdateProductDTO>(body)!;

        Product updatedProduct = await _productService.UpdateProduct(productId, updateProductDTO);
        ProductResponse updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(updatedProductResponse);

        return res;
    }

    // Delete product

    [Function(nameof(DeleteProduct))]
    [OpenApiOperation(operationId: nameof(DeleteProduct), tags: new[] { "Products" }, Summary = "Delete a product", Description = "Allows for the deletion/removal of a product.")]
    [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The product id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The product has been deleted.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the product.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteProduct(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{productId}")] HttpRequestData req,
        string productId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the DeleteProduct request.");

        await _productService.DeleteProduct(productId);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    // Upload product image

    [Function(nameof(UploadProductImage))]
    [OpenApiOperation(operationId: nameof(UploadProductImage), tags: new[] { "Products" }, Summary = "Upload a product image", Description = "Allows for an image of a product to be uploaded.")]
    [OpenApiParameter(name: "productId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The product id parameter.")]
    [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(ProductImageDTO), Required = true, Description = "A single png image to upload as data for the product image")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The OK response after uploading the product image.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public static async Task<IActionResult> UploadProductImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/imageUpload/{productId}")] HttpRequestData req,
        string productId)
    {
        //_logger.LogInformation("C# HTTP trigger UploadProductImage function processed a request.");
        Task<MultipartFormDataParser> parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);

        //string body = await new StreamReader(req.Body).ReadToEndAsync();

        if (parsedFormBody == null || parsedFormBody.Result.Files[0].ContentType != "image/png")
        {
            return new BadRequestObjectResult("please upload a png type image file");
        }

        /*Product product = await _productService.GetProductById(productId);
        if(product == null)
        {
            return new BadRequestObjectResult("the product id does not exist");
        }*/

        FilePart file = parsedFormBody.Result.Files[0];

        string conn = Environment.GetEnvironmentVariable("StorageConnection");

        // create the blob container client
        BlobContainerClient blobs = new BlobContainerClient(conn, "productimagescontainer");

        // create the blob client
        BlobClient blob = blobs.GetBlobClient(productId);

        // upload image to a blob in the container
        await blob.UploadAsync(file.Data, new BlobHttpHeaders { ContentType = file.ContentType });

        // return succesfull result message along with the created image id
        return new OkObjectResult($"image successfully uploaded. product id: {productId}");
    }
}