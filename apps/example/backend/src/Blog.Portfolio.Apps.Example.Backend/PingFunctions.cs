using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Blog.Portfolio.Apps.Example.Backend;

public sealed class PingFunctions
{
    private readonly PingEndpoint _endpoint = new();

    [Function("ExamplePing")]
    public async Task<HttpResponseData> PingAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "example/ping")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var result = await _endpoint.HandleAsync(new PingRequest(), cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result, cancellationToken);
        return response;
    }
}
