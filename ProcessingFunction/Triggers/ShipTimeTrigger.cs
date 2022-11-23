using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Service.Interfaces;

namespace ProcessingFunction.Triggers;

public class ShipTimeTrigger
{

    private readonly ILogger _logger;
    private readonly IOrderService _orderService;

    public ShipTimeTrigger(ILoggerFactory loggerFactory, IOrderService orderService)
    {
        _logger = loggerFactory.CreateLogger<ShipTimeTrigger>();
        _orderService = orderService;
    }

    //timetrigger every month? (2 weeks?) that posts a message to a queue (and table storage?)
    //to ensure that a shipment for orders is done (select like 5 or something?) (and then the Shipment queuetrigger can update orders, using UpdateOrderDTO sent from here)
    [return: Queue("Shipments", Connection = "connString")]
    [FunctionName("ShipOrdersTimeTrigger")]
    public async void ShipOrdersTimeTrigger(
    [TimerTrigger("0 */15 * * * *")] TimerInfo timerInfo,
    [Queue("Shipments", Connection = "AzureWebJobsStorage")] QueueClient shipmentQueue, ILogger log)
    {
        //get list of orders which haven't shipped yet (so create .where repo linq query where isProcessed = false
        //foreach(/*however many orders are shipped at a time (max of 10 or something? idk or care anymore with this dude)*/)
        Order unshippedOrder = await _orderService.GetunprocessedOrder();

        //await _orderService.GetOrderById(unshippedOrder.OrderId);

        UpdateOrderDTO updateOrderDTO = new UpdateOrderDTO() { OrderId = unshippedOrder.OrderId, ShippingDate = DateTime.UtcNow, IsProcessed = true };

        string body = JsonConvert.SerializeObject(updateOrderDTO);

        await shipmentQueue.SendMessageAsync(body);

        log.LogInformation($"The order was placed successfully, order placed: {body}");
    }
}
