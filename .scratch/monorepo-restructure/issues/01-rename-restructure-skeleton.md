# 01 — Rename & restructure the repo skeleton

**What to build:** Rename the repo, solution, and root namespace from `Blog.Api` to `Blog.Portfolio`, and lay down the new top-level folder skeleton (`host/`, `apps/`, `shared/`, `aspire/`), retiring the old `src/`/`tests/` convention. Purely mechanical prefactoring — no new behaviour — so every later ticket has a real home to land in.

**Blocked by:** None — can start immediately

**Status:** done

- [x] Repo/solution file, root namespace, and project names use `Blog.Portfolio` instead of `Blog.Api`
- [x] Top-level `apps/`, `shared/`, `host/`, `aspire/` folders exist
- [x] The relocated Functions project lives under `host/` and builds successfully
- [x] `host/` starts locally (e.g. `func start`) with zero functions registered, with no errors
- [x] The old `src/` and `tests/` folders and their projects are removed
- [x] Existing OpenAPI, CORS, Application Insights, and JSON serialization configuration from `Blog.Api.Functions` is preserved in `host/`

## Comments

Implemented in commit 7ec292f on `main`.

- Solution renamed `Blog.Api.slnx` → `Blog.Portfolio.slnx`; GitHub repo renamed `LeeSanderson/Blog.Api` → `LeeSanderson/Blog.Portfolio` (local remote URL updated to match).
- `src/Blog.Api.Functions` moved to `host/src/Blog.Portfolio.Host`, namespace renamed throughout; OpenAPI/CORS/AppInsights/JSON config carried forward unchanged.
- Old `src/` and `tests/` (two empty placeholder test projects, no test code) removed entirely.
- Note on "zero functions registered": `func start` registers 4 endpoints (`RenderOAuth2Redirect`, `RenderOpenApiDocument`, `RenderSwaggerDocument`, `RenderSwaggerUI`) — these are built into the carried-forward OpenAPI extension package itself (same behaviour as the old `Blog.Api.Functions`), not app-specific `[Function]` endpoints. Zero *application* functions are registered; `apps/example`'s first real endpoint lands in ticket 02.
- The local working directory (`C:\Dev\Personal\Blog.Api`) was deliberately left un-renamed since it was this session's active working directory — rename it manually whenever convenient.
