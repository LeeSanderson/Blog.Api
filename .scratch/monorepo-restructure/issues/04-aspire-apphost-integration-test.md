# 04 — Aspire AppHost + primary end-to-end integration test

**What to build:** The Aspire AppHost that orchestrates the whole suite locally, and the spec's "primary seam" — an automated integration test that starts the real AppHost and drives the example endpoint over HTTP, proving discovery, routing, the REPR base, and the AppHost wiring all work together.

**Blocked by:** 02

**Status:** done

- [x] `aspire/Blog.Portfolio.AppHost` and `aspire/Blog.Portfolio.ServiceDefaults` projects exist
- [x] The AppHost registers `host/` via `AddAzureFunctionsProject`, not `AddProject`
- [x] Running the AppHost starts `host/` together with any required emulated dependencies (e.g. Azurite) with no manual configuration
- [x] An integration test using `Aspire.Hosting.Testing.DistributedApplicationTestingBuilder` builds and starts the AppHost, resolves an `HttpClient` for the `host` resource, and calls `GET /api/example/ping`
- [x] That integration test asserts the expected response, proving discovery, routing, the REPR base, and the AppHost wiring all work together
- [x] New apps can be added to local orchestration by registering them explicitly in AppHost code (documented or demonstrated)

## Comments

- Three new projects, all flat under `aspire/` (no `src`/`tests` split, matching the literal paths this ticket names): `Blog.Portfolio.AppHost`, `Blog.Portfolio.ServiceDefaults`, and `Blog.Portfolio.AppHost.Tests`. Added to `Blog.Portfolio.slnx` under a new `/aspire/` folder, and to `Directory.Packages.props` (`Aspire.Hosting.Azure.Functions`, `Aspire.Hosting.Testing`, plus `ServiceDefaults`' OpenTelemetry/resilience/service-discovery packages), all pinned to Aspire 13.4.6.
- `AppHost.cs`: `builder.AddAzureStorage("storage").RunAsEmulator()` for Azurite, then `builder.AddAzureFunctionsProject<Projects.Blog_Portfolio_Host>("host").WithHostStorage(storage).WithExternalHttpEndpoints()` — matches the documented Aspire Azure Functions integration pattern (verified against `aspire.dev` docs and several real-world sample AppHosts using the same integration).
- `ServiceDefaults` is scaffolded (standard Aspire template content: OpenTelemetry, health checks, resilience, service discovery) but has no consumer yet — `host/`'s own composition-root wiring (OpenAPI/CORS/App Insights) is already established, and there's no other standalone .NET service in local orchestration today. Documented in `aspire/README.md` as scaffolded ahead of need, same pattern as `apps/example/` itself.
- New apps' backends need no separate AppHost registration (already reachable through `host`'s existing project reference); only a future app's `frontend/` would need one explicit call. Since `apps/example/` is backend-only, this is documented in `aspire/README.md` rather than demonstrated in code.
- `ExamplePingEndToEndTests.GetExamplePing_ReturnsPongThroughTheRunningAppHost` uses `DistributedApplicationTestingBuilder.CreateAsync<Projects.Blog_Portfolio_AppHost>()`, builds/starts the real AppHost, waits for the `host` resource to reach `KnownResourceStates.Running` (there's no health check to wait on — Azure Functions resources don't get one by default), then polls `GET /api/example/ping` with a 1-second retry loop bounded by the test's own 3-minute deadline until the Functions host is actually accepting connections, matching Aspire's own documented guidance for resources without health checks. Asserts `200 OK` and a `{"message":"pong"}` body.
- Verified against the real stack, not just compiled: ran with Docker Desktop actually up, pulling the real `azurite` image and starting the real `func` host via Azure Functions Core Tools — first request attempt failed with connection-refused while the Functions worker was still initializing, the retry loop caught it and succeeded on the second attempt ~2s later. Confirms the primary seam (cross-assembly discovery + routing + REPR base + AppHost wiring) actually works end-to-end, not just on paper.
- Went through `/code-review` (Standards + Spec axes, run in parallel). Fixes applied: removed `TargetFramework`/`IsPackable` re-declarations in the two new class-library/test `.csproj` files that `Directory.Build.props` already sets repo-wide; replaced the test's reliance on `AddStandardResilienceHandler()` alone (whose internal retry budget is shorter than Functions cold-start can take) with an explicit, test-owned retry loop bounded by the test's own timeout. Not changed: the test's coupling to the `example` app's route/payload (the spec explicitly names this exact HTTP call as the primary seam), `WithExternalHttpEndpoints()` (matches every real-world Aspire+Functions sample found), and a lambda-parameter shadow in `ServiceDefaults/Extensions.cs` (verbatim Microsoft template boilerplate).
- Full solution (`dotnet build` + `dotnet test`) is green: 0 warnings, 0 errors, all 4 test projects passing (including the new end-to-end AppHost test, run twice against a real Docker/Azurite/Functions stack).
