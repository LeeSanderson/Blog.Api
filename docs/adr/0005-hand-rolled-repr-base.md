# REPR pattern via a hand-rolled base class, not a third-party library

Backend endpoints follow the REPR (Request-Endpoint-Response) pattern via a small custom `Endpoint<TRequest, TResponse>`-style base class in `shared/backend`, rather than adopting an existing REPR library such as FastEndpoints or ApiEndpoints.

Those libraries get their value from ASP.NET Core's own endpoint-routing pipeline for auto-discovery and DI wiring, but the Azure Functions isolated worker model discovers endpoints through its own metadata system (`[Function]` attribute plus trigger bindings) instead. A third-party library's main benefit therefore wouldn't transfer, and adopting one would mean fighting assumptions it makes about a hosting model this project doesn't use.
