# Single shared Azure Function App for all portfolio apps

All portfolio apps' backend code compiles into one Azure Function App (`host/`), rather than each app deploying its own Function App resource. This keeps hosting cost and operational overhead low for a personal portfolio site, at the cost of a shared blast radius — a bug in one app's function can affect every other app's backend endpoints.

We mitigate the blast radius two ways: the full cross-app backend test suite must pass before every deploy (not just the touched app's tests), and app-scoped route prefixes (`/api/{app-name}/...`) are enforced via an architecture test to prevent route collisions between apps.

## Consequences

- A change to any single app's backend triggers a full rebuild and redeploy of the combined host.
- There is no way to deploy or roll back one app's backend independently of the others.
