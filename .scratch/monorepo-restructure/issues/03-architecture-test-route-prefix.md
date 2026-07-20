# 03 — Architecture test: route-prefix enforcement

**What to build:** The "secondary seam" from the spec — a reflection-based test that scans every app's compiled backend assembly and fails the build if an endpoint's route doesn't start with `/api/{app-name}/`. Deliberately independent of any running host or Aspire, so it stays fast and holds for every app, present and future (ADR-0001).

**Blocked by:** 02

**Status:** ready-for-agent

- [ ] A reflection-based test scans the compiled `host` assembly and every referenced app `backend` assembly for `[Function]`-attributed methods
- [ ] The test asserts every discovered route starts with `/api/{app-name}/`
- [ ] The test fails when a deliberately misconfigured endpoint (missing/incorrect prefix) is introduced, proving it actually catches violations
- [ ] The test lives in `host/tests/` and does not require starting the Functions host or Aspire to run
- [ ] The test runs as part of the normal `dotnet test` cycle for the solution
