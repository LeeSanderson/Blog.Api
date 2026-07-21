# aspire/

Local orchestration for the whole app suite: `Blog.Portfolio.AppHost` and `Blog.Portfolio.ServiceDefaults`.

`Blog.Portfolio.AppHost` registers the shared `host/` project via `AddAzureFunctionsProject`, backed by an
Azurite storage emulator (`AddAzureStorage(...).RunAsEmulator()` + `WithHostStorage`). Every app's backend
endpoints are already reachable through `host` once that app's `backend/src` project is referenced from
`host/src/Blog.Portfolio.Host/Blog.Portfolio.Host.csproj` — no separate AppHost registration is needed for a
new app's backend.

A new app's `frontend/` (or any standalone resource that isn't part of the shared `host`) is added to local
orchestration with one explicit call in `Blog.Portfolio.AppHost/AppHost.cs`, e.g. `builder.AddNpmApp(...)` for
a Next.js app. There's no folder auto-discovery — comment out or remove a resource's call to skip it locally.

`Blog.Portfolio.ServiceDefaults` has no consumer yet — `host/`'s composition-root wiring (OpenAPI, CORS,
Application Insights) is already established separately, and there's no other standalone .NET service project
in local orchestration today. It exists as the conventional Aspire home for cross-cutting service wiring
(telemetry, health checks, resilience, service discovery), ready for whichever future .NET service project
needs it first.
