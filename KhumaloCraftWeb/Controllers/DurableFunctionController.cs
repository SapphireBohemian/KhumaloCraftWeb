using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KhumaloCraftWeb.Controllers
{
    public class DurableFunctionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DurableFunctionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Action to trigger the durable function
        [HttpPost]
        public async Task<IActionResult> TriggerDurableFunction()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://your-durable-function-url");

            // Example of data to pass to durable function
            var requestData = new
            {
                // Provide any necessary data for the function
                // Example: { orderId: 123 }
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // Process success
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else
            {
                // Handle failure
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
