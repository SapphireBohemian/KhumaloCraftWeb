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
    public static class OrderActivities
    {
        [FunctionName("UpdateInventory")]
        public static async Task UpdateInventory([ActivityTrigger] Order order, ILogger log)
        {
            // Update inventory logic
            log.LogInformation($"Updating inventory for order {order.OrderId}...");
            await Task.Delay(1000); // Simulating inventory update process
            log.LogInformation($"Inventory updated for order {order.OrderId}.");
        }

        [FunctionName("ProcessPayment")]
        public static async Task ProcessPayment([ActivityTrigger] Order order, ILogger log)
        {
            // Payment processing logic
            log.LogInformation($"Processing payment for order {order.OrderId}...");
            await Task.Delay(2000); // Simulating payment processing
            log.LogInformation($"Payment processed for order {order.OrderId}.");
        }

        [FunctionName("SendOrderConfirmation")]
        public static async Task SendOrderConfirmation([ActivityTrigger] Order order, ILogger log)
        {
            // Send order confirmation logic
            log.LogInformation($"Sending order confirmation for order {order.OrderId}...");
            await Task.Delay(500); // Simulating order confirmation sending
            log.LogInformation($"Order confirmation sent for order {order.OrderId}.");
        }

        [FunctionName("SendOrderCompletedNotification")]
        public static async Task SendOrderCompletedNotification([ActivityTrigger] string userId, ILogger log)
        {
            // Send order completed notification logic
            log.LogInformation($"Sending order completed notification to user {userId}...");
            await Task.Delay(300); // Simulating notification sending
            log.LogInformation($"Order completed notification sent to user {userId}.");
        }
    }
}
