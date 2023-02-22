using API.Attributes;
using API.Examples;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.DTO;
using Newtonsoft.Json;
using Service.Interfaces;
using HttpTriggerAttribute = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
using Azure.Storage.Queues;

namespace PostingAPI.Controllers;

public class WriteOrderController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly IProductService _productService;

    public WriteOrderController(ILoggerFactory loggerFactory, IMapper mapper, IOrderService orderService, IUserService userService, IProductService productService)
    {
        _logger = loggerFactory.CreateLogger<WriteOrderController>();
        _mapper = mapper;
        _orderService = orderService;
        _userService = userService;
        _productService = productService;
    }

    //use this function to process an orderDTO and post it to a queue
    //[return: Queue("ordersqueue", Connection = "StorageConnection")]
    [Function(nameof(CreateOrder))]
    [OpenApiOperation(operationId: nameof(CreateOrder), tags: new[] { "Orders" }, Summary = "Create a new order", Description = "Will create the new order.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(OrderDTO), Required = true, Description = "Data for the order that has to be created.", Example = typeof(OrderExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(OrderDTO), Description = "The OK response")]
    public async Task<HttpResponseData> CreateOrder(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")] HttpRequestData req
            )
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateOrder request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(body)!;
        
        orderDTO.OrderId = Guid.NewGuid().ToString();

        //await _orderService.CreateOrder(orderDTO);

        string conn = Environment.GetEnvironmentVariable("StorageConnection");

        // Create queue client and send message
        QueueClient queueClient = new QueueClient(conn, "ordersqueue", new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        // Create the queue if it doesn't already exist
        queueClient.CreateIfNotExists();

        //string encodedOrderDTO = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderDTO)));

        //await queueClient.SendMessageAsync(encodedOrderDTO);

        await queueClient.SendMessageAsync(JsonConvert.SerializeObject(orderDTO));

        string responseMessage = $"The order was placed successfully, order: {JsonConvert.SerializeObject(orderDTO)}";

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(responseMessage);

        return res;
    }

    // Delete order

    [Function(nameof(DeleteOrder))]
    [OpenApiOperation(operationId: nameof(DeleteOrder), tags: new[] { "Orders" }, Summary = "Delete an order", Description = "Allows for the deletion of an order.")]
    [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The order id parameter.")]
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