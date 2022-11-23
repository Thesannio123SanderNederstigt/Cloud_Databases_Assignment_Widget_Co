using API.Attributes;
using API.Examples;
using AutoMapper;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Exceptions;
using Service.Interfaces;
using Service;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using Azure.Data.Tables;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace PostingAPI.Controllers;

public class WriteOrderController
{
    private readonly ILogger _logger;
    private readonly IOrderService _orderService;

    public WriteOrderController(ILoggerFactory loggerFactory, IOrderService orderService)
    {
        _logger = loggerFactory.CreateLogger<WriteOrderController>();
        _orderService = orderService;
    }

    //use this function to process an orderDTO and post it to a queue
    [return: Queue("Orders", Connection = "AzureWebJobsStorage")]
    [FunctionName(nameof(CreateOrder))]
    [OpenApiOperation(operationId: nameof(CreateOrder), tags: new[] { "Orders" }, Summary = "Create a new order", Description = "Will create the new order.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(OrderDTO), Required = true, Description = "Data for the order that has to be created.", Example = typeof(OrderExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "The OK response")]
    public async Task<IActionResult> CreateOrder(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        [Queue("Orders", Connection = "AzureWebJobsStorage")] QueueClient queue,
        [Table("OrderLogs", Connection = "AzureWebJobsStorage")] TableClient table
            )
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateOrder request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        //OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(body)!;

        await queue.SendMessageAsync(body);

        string responseMessage = $"The order was placed successfully, order placed: {body}";

        //await table.AddEntityAsync(OrderDTO.Processing());

        //return JsonConvert.SerializeObject(orderDTO);

        return new OkObjectResult(responseMessage);
    }

    // Delete order

    [Function(nameof(DeleteOrder))]
    [OpenApiOperation(operationId: nameof(DeleteOrder), tags: new[] { "Orders" }, Summary = "Delete an order", Description = "Allows for the deletion of an order.")]
    [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The order id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The order has been deleted.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the order.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteOrder(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "orders/{orderId}")] HttpRequestData req,
        string orderId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the DeleteOrder request.");

        await _orderService.DeleteOrder(orderId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}