# 04 — Aspire AppHost + primary end-to-end integration test

**What to build:** The Aspire AppHost that orchestrates the whole suite locally, and the spec's "primary seam" — an automated integration test that starts the real AppHost and drives the example endpoint over HTTP, proving discovery, routing, the REPR base, and the AppHost wiring all work together.

**Blocked by:** 02

**Status:** ready-for-agent

- [ ] `aspire/Blog.Portfolio.AppHost` and `aspire/Blog.Portfolio.ServiceDefaults` projects exist
- [ ] The AppHost registers `host/` via `AddAzureFunctionsProject`, not `AddProject`
- [ ] Running the AppHost starts `host/` together with any required emulated dependencies (e.g. Azurite) with no manual configuration
- [ ] An integration test using `Aspire.Hosting.Testing.DistributedApplicationTestingBuilder` builds and starts the AppHost, resolves an `HttpClient` for the `host` resource, and calls `GET /api/example/ping`
- [ ] That integration test asserts the expected response, proving discovery, routing, the REPR base, and the AppHost wiring all work together
- [ ] New apps can be added to local orchestration by registering them explicitly in AppHost code (documented or demonstrated)
