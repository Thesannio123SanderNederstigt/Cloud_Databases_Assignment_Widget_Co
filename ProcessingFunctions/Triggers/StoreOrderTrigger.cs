using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Model.DTO;
using Service.Interfaces;
using System.Threading.Tasks;
using System;

namespace ProcessingFunctions.Triggers
{
    public class StoreOrderTrigger
    {
        private readonly IOrderService _orderService;
        public StoreOrderTrigger(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [FunctionName("StoreOrderTrigger")]
        public async Task Run([QueueTrigger("ordersqueue", Connection = "AzureWebJobsStorage")] string orderQueueString, ILogger logger)
        {
            logger.LogInformation($"C# Queue trigger function processed: StoreOrderTrigger");

            logger.LogInformation($"new orderDTO: {orderQueueString}");

            try
            {
                OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(orderQueueString)!;

                await _orderService.CreateOrder(orderDTO);
            }
            catch(Exception e) 
            {
                logger.LogInformation($"an exception occurred: {e.Message}");
            }
        }
    }
}