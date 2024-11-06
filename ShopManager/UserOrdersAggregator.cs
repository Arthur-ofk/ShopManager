using Newtonsoft.Json;
using Ocelot.Configuration.File;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Ocelot.Responses;
using System.Net;
using System.Text;

namespace APIGateway
{
    public class UserOrdersAggregator : IDefinedAggregator
    {
        private readonly AggregateRouteConfig _config = new AggregateRouteConfig
        {
            RouteKey = "user",
            JsonPath = "$[*].userId",
            Parameter = "userId"
        };
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            ////var userResponse = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            ////var ordersResponse = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

            //var responses = resp.Select(x => x.Items.DownstreamResponse()).ToArray();

            //var combinedResponse = $"{{\"user\": {responses[0]}, \"orders\": {responses[1]}}}";
            //var content = new StringContent(JsonConvert.SerializeObject(combinedResponse), Encoding.UTF8, "application/json");
            //return new DownstreamResponse(content,HttpStatusCode.OK,responses.SelectMany(x=>x.Headers).ToList(),"reason");
            var userResponse = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            var ordersResponse = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

            // Формування об’єднаної відповіді
            var aggregatedResponse = $"{{\"user\": {userResponse}, \"orders\": {ordersResponse}}}";

            return new DownstreamResponse(
                new StringContent(aggregatedResponse),
                System.Net.HttpStatusCode.OK,
                responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList(),
                "application/json"
            );
        }
    }
}
