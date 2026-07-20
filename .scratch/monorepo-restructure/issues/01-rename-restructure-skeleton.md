# 01 — Rename & restructure the repo skeleton

**What to build:** Rename the repo, solution, and root namespace from `Blog.Api` to `Blog.Portfolio`, and lay down the new top-level folder skeleton (`host/`, `apps/`, `shared/`, `aspire/`), retiring the old `src/`/`tests/` convention. Purely mechanical prefactoring — no new behaviour — so every later ticket has a real home to land in.

**Blocked by:** None — can start immediately

**Status:** ready-for-agent

- [ ] Repo/solution file, root namespace, and project names use `Blog.Portfolio` instead of `Blog.Api`
- [ ] Top-level `apps/`, `shared/`, `host/`, `aspire/` folders exist
- [ ] The relocated Functions project lives under `host/` and builds successfully
- [ ] `host/` starts locally (e.g. `func start`) with zero functions registered, with no errors
- [ ] The old `src/` and `tests/` folders and their projects are removed
- [ ] Existing OpenAPI, CORS, Application Insights, and JSON serialization configuration from `Blog.Api.Functions` is preserved in `host/`
