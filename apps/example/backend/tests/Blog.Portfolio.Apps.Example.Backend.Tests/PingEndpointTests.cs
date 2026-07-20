using FluentAssertions;

namespace Blog.Portfolio.Apps.Example.Backend.Tests;

public class PingEndpointTests
{
    [Fact]
    public async Task HandleAsync_ReturnsPongMessage()
    {
        var endpoint = new PingEndpoint();

        var response = await endpoint.HandleAsync(new PingRequest(), CancellationToken.None);

        response.Message.Should().Be("pong");
    }
}
