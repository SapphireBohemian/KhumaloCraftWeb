using KhumaloCraftWeb.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrderProcessingFunction
{
    public class OrderOrchestrator
    {
        [FunctionName("OrderOrchestrator")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var order = context.GetInput<Order>(); // Get input data (order details)

            // Sequence of activities to execute
            await context.CallActivityAsync("UpdateInventory", order);
            await context.CallActivityAsync("ProcessPayment", order);
            await context.CallActivityAsync("SendOrderConfirmation", order);

            // Notify user about order completion
            await context.CallActivityAsync("SendOrderCompletedNotification", order.UserId);
        }
    }
}
