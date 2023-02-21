using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Model.DTO;
using Newtonsoft.Json;
using Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProcessingFunction.Triggers;

public class ShipOrderTrigger
{
    private readonly IOrderService _orderService;

    public ShipOrderTrigger(IOrderService orderService)
    {
        _orderService = orderService;
    }

    //queuetrigger reading messages inserted into the shipmentsqueue by the ShipTimeTrigger, interprets these as UpdateOrderDTO's and updates the orders (create the shipment if you will)
    [FunctionName("shipOrderTrigger")]
    public async Task Run([QueueTrigger("shipmentsqueue", Connection = "AzureWebJobsStorage")] string unshippedOrderSTring, ILogger logger)
    {
        logger.LogInformation($"C# Queue trigger function processed: {unshippedOrderSTring}");

        try
        {
            UpdateOrderDTO updateOrderDTO = JsonConvert.DeserializeObject<UpdateOrderDTO>(unshippedOrderSTring)!;

            await _orderService.UpdateOrder(updateOrderDTO);
        } 
        catch(Exception e)
        {
            logger.LogInformation($"an exception occurred: {e.Message}");
        }
    }
}