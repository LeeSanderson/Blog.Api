# 02 — First working endpoint: REPR base + apps/example wired into host

**What to build:** A shared REPR base abstraction, and one real endpoint (`apps/example`) built on it, referenced by `host/` and reachable over HTTP. This is the first genuine vertical slice: proves cross-assembly function discovery and the app-name route-prefix convention work together, end to end.

**Blocked by:** 01

**Status:** done

- [x] `shared/backend/` contains an `Endpoint<TRequest, TResponse>`-style REPR base class
- [x] `apps/example/backend/src/` contains a `GET /api/example/ping` endpoint built on the REPR base
- [x] `apps/example/backend/tests/` contains a direct (non-HTTP) unit test exercising the REPR base via the example endpoint
- [x] `host/` references `apps/example/backend/src` and discovers the endpoint via the Functions SDK's generated metadata provider — no manual registration
- [x] Running `host/` locally and issuing a `GET /api/example/ping` request returns the expected response
- [x] The endpoint's route is prefixed `/api/example/...`, per the app-name convention

## Comments

- `shared/backend/Blog.Portfolio.Shared.Backend` holds the REPR base: `Endpoint<TRequest, TResponse>` with a single abstract `HandleAsync(TRequest, CancellationToken)`. Kept framework-agnostic (no Azure Functions types) so the unit test seam stays non-HTTP, per ADR-0005.
- `apps/example/backend/src/Blog.Portfolio.Apps.Example.Backend` has `PingEndpoint : Endpoint<PingRequest, PingResponse>` (returns `{ Message = "pong" }`), plus a thin `[Function("ExamplePing")]` adapter (`PingFunctions.PingAsync`) that maps the `HttpTrigger` (`GET`, `AuthorizationLevel.Anonymous`, route `example/ping`) onto it and writes the response as JSON.
- `apps/example/backend/tests/Blog.Portfolio.Apps.Example.Backend.Tests` has `PingEndpointTests` calling `PingEndpoint.HandleAsync` directly (no HTTP), written first as a TDD red step against not-yet-existing types, then turned green.
- `host/src/Blog.Portfolio.Host` takes a `ProjectReference` to the app's backend project only — no manual function registration. Verified via `dotnet build` that `functions.metadata` under `host`'s output lists `ExamplePing` (entry point `Blog.Portfolio.Apps.Example.Backend.PingFunctions.PingAsync`) alongside the OpenAPI-package functions, and via `func start` that `GET http://localhost:7241/api/example/ping` returns `{"message":"pong"}`.
- Solution (`Blog.Portfolio.slnx`) updated with `/shared/backend/` and `/apps/example/backend/` folders for the three new projects.
- A couple of Sonar/StyleCop rules needed a deliberate, commented suppression rather than a design change: `S1694` on `Endpoint<TRequest, TResponse>` (ADR-0005 explicitly wants a base class, not an interface) and `S2094` on the empty `PingRequest` marker record (the ping endpoint genuinely takes no input).
- Added a short `apps/example/README.md` per the `apps/{app-name}/` folder convention in `apps/README.md`.
