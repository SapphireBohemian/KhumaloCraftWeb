using KhumaloCraftWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrderProcessingFunction
{
    public class OrderHttpTrigger
    {
        [FunctionName("OrderHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            // Start a new instance of the orchestration with the provided order details
            string instanceId = await starter.StartNewAsync("OrderOrchestrator", order);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return new OkObjectResult($"Started order processing with ID = '{instanceId}'.");
        }
    }
}
