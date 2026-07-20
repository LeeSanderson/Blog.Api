# 07 — Frontend CI/CD reusable workflow template

**What to build:** The reusable frontend deploy workflow and the per-app template that calls it, so a new app's frontend pipeline is a copy of a small template rather than a fresh design each time (ADR-0002: direct push to `leesanderson.github.io`, no PR gate).

**Blocked by:** 01

**Status:** ready-for-agent

- [ ] A reusable workflow (e.g. `.github/workflows/_reusable-frontend-deploy.yml`) exists, encapsulating the common build-and-push-to-`leesanderson.github.io` steps
- [ ] A documented per-app workflow template shows how a new app's `.github/workflows/{app-name}.yml` calls the reusable workflow, path-filtered to `apps/{app-name}/frontend/**`
- [ ] The workflow YAML passes structural validation (e.g. `actionlint` or GitHub's own workflow syntax check)
- [ ] The template and its usage are documented for whoever adds the first frontend-having app
- [ ] Note: live execution isn't verified by this ticket, since no frontend app exists in this repo yet — that will happen naturally when the first frontend-having app is built
