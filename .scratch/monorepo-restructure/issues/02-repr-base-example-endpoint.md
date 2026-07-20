# 02 — First working endpoint: REPR base + apps/example wired into host

**What to build:** A shared REPR base abstraction, and one real endpoint (`apps/example`) built on it, referenced by `host/` and reachable over HTTP. This is the first genuine vertical slice: proves cross-assembly function discovery and the app-name route-prefix convention work together, end to end.

**Blocked by:** 01

**Status:** ready-for-agent

- [ ] `shared/backend/` contains an `Endpoint<TRequest, TResponse>`-style REPR base class
- [ ] `apps/example/backend/src/` contains a `GET /api/example/ping` endpoint built on the REPR base
- [ ] `apps/example/backend/tests/` contains a direct (non-HTTP) unit test exercising the REPR base via the example endpoint
- [ ] `host/` references `apps/example/backend/src` and discovers the endpoint via the Functions SDK's generated metadata provider — no manual registration
- [ ] Running `host/` locally and issuing a `GET /api/example/ping` request returns the expected response
- [ ] The endpoint's route is prefixed `/api/example/...`, per the app-name convention
