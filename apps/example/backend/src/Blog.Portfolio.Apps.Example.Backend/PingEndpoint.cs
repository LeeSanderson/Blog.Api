using Blog.Portfolio.Shared.Backend;

namespace Blog.Portfolio.Apps.Example.Backend;

public sealed class PingEndpoint : Endpoint<PingRequest, PingResponse>
{
    public override Task<PingResponse> HandleAsync(PingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PingResponse("pong"));
    }
}
