using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Service.Interfaces;

namespace ProcessingFunction.Triggers;

public class ShipTimeTrigger
{
    private readonly IOrderService _orderService;

    public ShipTimeTrigger(IOrderService orderService)
    {
        _orderService = orderService;
    }

    //timetrigger that checks every 15th minute and when there is an unprocessed (unshipped) order it will post a message to a queue
    //to ensure that a shipment for orders is done (and then the ShipOrderTrigger queuetrigger will update the order using the UpdateOrderDTO sent to the queue from here after which 5 seconds is waited to ensure the same order can be processed before checking again)
    [return: Queue("shipmentsqueue", Connection = "StorageConnection")]
    [FunctionName("ShipOrdersTimeTrigger")]
    public async Task ShipOrdersTimeTrigger(
    [TimerTrigger("0 */15 * * * *")] TimerInfo timerInfo,
    [Queue("shipmentsqueue", Connection = "StorageConnection")] QueueClient shipmentQueue, ILogger logger)
    {
        try
        {
            logger.LogInformation($"ShipOrdersTimeTrigger function, timer trigger schedule: {timerInfo.Schedule}");

            for(int i = 1; i < int.Parse(Environment.GetEnvironmentVariable("ShipmentQuantity")); i++)
            {
                Order unshippedOrder = await _orderService.GetunprocessedOrder();

                UpdateOrderDTO updateOrderDTO = new UpdateOrderDTO() { OrderId = unshippedOrder.OrderId, ShippingDate = DateTime.UtcNow, IsProcessed = true };

                string body = JsonConvert.SerializeObject(updateOrderDTO);

                await shipmentQueue.SendMessageAsync(body);

                logger.LogInformation($"The order was placed successfully, order placed: {body}");

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
        catch(Exception e)
        {
            logger.LogInformation($"an exception occurred: {e.Message}");
        }
    }
}