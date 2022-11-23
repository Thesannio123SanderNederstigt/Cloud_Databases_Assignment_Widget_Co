using System;
using System.Net;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Repository.Interfaces;
using Service.Interfaces;

namespace ProcessingFunction.Triggers;

public class StoreOrderTrigger
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;
    //private readonly IOrderRepository _orderRepository;
    //private readonly IProductRepository _productRepository;

    public StoreOrderTrigger(ILoggerFactory loggerFactory, IMapper mapper, IOrderService orderService/*IOrderRepository orderRepository, IProductRepository productRepository*/)
    {
        _logger = loggerFactory.CreateLogger<StoreOrderTrigger>();
        _mapper = mapper;
        _orderService = orderService;
        //_orderRepository = orderRepository;
       // _productRepository = productRepository;
    }

    // reads OrderDTO from Orders queue and use the repository to post them to the SQL database (this is supposed to be the model/setup used for all post/update endpoints...)

    [FunctionName("postOrderTrigger")]
    public async void Run([QueueTrigger("Orders", Connection = "AzureWebJobsStorage")] string orderQueueString)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {orderQueueString}");

        OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(orderQueueString)!;

        //Order order = await _mapper.Map<Task<Order>>(orderDTO);

        await _orderService.CreateOrder(orderDTO);

        /*HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(newReview);

        return res;*/
    }
}