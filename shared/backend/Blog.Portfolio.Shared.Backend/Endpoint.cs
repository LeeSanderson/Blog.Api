namespace Blog.Portfolio.Shared.Backend;

/// <summary>
/// REPR (Request-Endpoint-Response) base class. An app's <c>[Function]</c>-attributed method adapts its
/// trigger binding to <typeparamref name="TRequest"/> and <typeparamref name="TResponse"/> around a call to
/// <see cref="HandleAsync"/>, keeping the endpoint's own logic testable without an HTTP pipeline.
/// </summary>
#pragma warning disable S1694 // ADR-0005 specifies a hand-rolled base class (not an interface) for the REPR pattern
public abstract class Endpoint<TRequest, TResponse>
#pragma warning restore S1694
{
    public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
