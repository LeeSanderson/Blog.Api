# 03 — Architecture test: route-prefix enforcement

**What to build:** The "secondary seam" from the spec — a reflection-based test that scans every app's compiled backend assembly and fails the build if an endpoint's route doesn't start with `/api/{app-name}/`. Deliberately independent of any running host or Aspire, so it stays fast and holds for every app, present and future (ADR-0001).

**Blocked by:** 02

**Status:** done

- [x] A reflection-based test scans the compiled `host` assembly and every referenced app `backend` assembly for `[Function]`-attributed methods
- [x] The test asserts every discovered route starts with `/api/{app-name}/`
- [x] The test fails when a deliberately misconfigured endpoint (missing/incorrect prefix) is introduced, proving it actually catches violations
- [x] The test lives in `host/tests/` and does not require starting the Functions host or Aspire to run
- [x] The test runs as part of the normal `dotnet test` cycle for the solution

## Comments

- New project `host/tests/Blog.Portfolio.Host.Tests` (xUnit + FluentAssertions, matching the stack from `apps/example/backend/tests/`), added to `Blog.Portfolio.slnx` under the `/host/` folder.
- `RoutePrefixArchitectureTests.EveryFunctionRoute_StartsWithApiAppNamePrefix` resolves the `host` assembly via a `HostAssemblyMarker` type (new, in `host/src/Blog.Portfolio.Host/HostAssemblyMarker.cs`) rather than a string-based `Assembly.Load`, so a future rename of the Host project fails the build instead of silently discovering zero endpoints. It reads `hostAssembly.GetReferencedAssemblies()`, filters to assembly names matching `Blog.Portfolio.Apps.{app}.Backend`, loads each, and reflects over their public methods for `[Function]` + `[HttpTrigger]` to build the full route (`/api/{route}`), asserting it starts with `/api/{app-name}/` (case-insensitive, matching Azure Functions' own case-insensitive route matching). A `NotBeEmpty` guard keeps the test from passing vacuously if discovery ever finds nothing.
- `GetReferencedAssemblies()` only walks direct references; this is sufficient because the spec requires `host/` to take a direct `ProjectReference` to every app's `backend/src` project (no transitive wiring) — noted as a comment at the call site.
- Proved the test catches violations two ways: (1) manually — temporarily changed `PingFunctions.cs`'s route from `example/ping` to `wrongprefix/ping`, ran `dotnet test`, observed a FAIL with a clear diagnostic message, then reverted (confirmed via `git diff` showing no residual change); (2) permanently — added `RoutePrefixArchitectureTests.DiscoverFunctionEndpoints_CatchesARouteMissingTheAppPrefix`, which reflects over a deliberately misconfigured `[Function]` fixture defined right in the test file and asserts the convention check throws. This second test is a regression guard: it keeps failing (and being noticed) if a future edit ever loosens the checker itself, which the one-off manual proof couldn't do on its own.
- Went through `/code-review` (Standards + Spec axes). Fixes applied: dropped a dead `.ToUpperInvariant()` call that didn't affect the (already case-insensitive) assertion but confused failure messages; switched to named constructor arguments for `FunctionEndpoint` to remove a same-type positional-argument data clump; added the marker-type fix and the persisted violation-catching test described above.
- Full solution (`dotnet build` + `dotnet test`) is green: 0 warnings, 0 errors, 3 tests passing across `Blog.Portfolio.Apps.Example.Backend.Tests` and `Blog.Portfolio.Host.Tests`.
