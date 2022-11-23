using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Newtonsoft.Json;
using Service.Interfaces;

namespace ProcessingFunction.Triggers;

public class ShipOrderTrigger
{
    private readonly ILogger _logger;
    private readonly IOrderService _orderService;

    public ShipOrderTrigger(ILoggerFactory loggerFactory, IOrderService orderService)
    {
        _logger = loggerFactory.CreateLogger<ShipOrderTrigger>();
        _orderService = orderService;
    }

    //queuetrigger die vanuit de ShipTimeTrigger een message in de shipment queue leest (updateOrderDTO's) en op basis hiervan shipment doet (updaten van isProcessed en shipmentDate van orders dus)
    [FunctionName("shipOrderTrigger")]
    public async void Run([QueueTrigger("Shipments", Connection = "AzureWebJobsStorage")] string unshippedOrderSTring)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {unshippedOrderSTring}");

        UpdateOrderDTO updateOrderDTO = JsonConvert.DeserializeObject<UpdateOrderDTO>(unshippedOrderSTring)!;

        await _orderService.UpdateOrder(updateOrderDTO);
    }

}
