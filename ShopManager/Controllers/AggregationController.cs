using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ocelot.Multiplexer;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/aggregate")]
    public class AggregatedOrdersController : ControllerBase
    {
        private readonly IDefinedAggregator _aggregator;
        private readonly HttpClient _httpClient;

        public AggregatedOrdersController(IDefinedAggregator aggregator, IHttpClientFactory httpClientFactory)
        {
            _aggregator = aggregator;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("user-orders/")]
        public async Task<IActionResult> GetUserOrdersAggregation()
        {
          

            var aggregatedResult = await _aggregator.Aggregate(new List<HttpContext> { HttpContext });
            return Content(await aggregatedResult.Content.ReadAsStringAsync(), "application/json");
        }
    }
}
