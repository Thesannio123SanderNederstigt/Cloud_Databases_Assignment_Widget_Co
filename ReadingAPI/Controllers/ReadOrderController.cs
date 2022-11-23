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

namespace ReadingAPI.Controllers;

public class ReadOrderController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public ReadOrderController(ILoggerFactory loggerFactory, IMapper mapper, IOrderService orderService)
    {
        _logger = loggerFactory.CreateLogger<ReadOrderController>();
        _mapper = mapper;
        _orderService = orderService;
    }

    // Get orders

    [Function(nameof(GetOrders))]
    [OpenApiOperation(operationId: nameof(GetOrders), tags: new[] { "Orders" }, Summary = "A list of orders", Description = "Will return a list of orders.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order[]), Description = "A list of orders.", Example = typeof(OrderExample))]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of orders.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetOrders request.");

        ICollection<Order> orders = await _orderService.GetOrders();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(orders);

        return res;
    }

    // Get order

    [Function(nameof(GetOrderById))]
    [OpenApiOperation(operationId: nameof(GetOrderById), tags: new[] { "Orders" }, Summary = "A single order", Description = "Will return a specified order.")]
    [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The order id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "A single retrieved order.", Example = typeof(OrderExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve the order.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the order.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetOrderById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{orderId}")] HttpRequestData req,
        string orderId)
    {

        _logger.LogInformation("C# HTTP trigger function processed the GetOrderById request.");

        Order order = await _orderService.GetOrderById(orderId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(order);

        return res;
    }
}
