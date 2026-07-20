# 05 — Infrastructure as code (Bicep via Aspire + azd)

**What to build:** Real Azure infrastructure for the shared Function App, provisioned as Bicep generated/maintained from the Aspire AppHost model and deployed via `azd`, per ADR-0004 — chosen deliberately so infra can be expanded or recreated easily as apps are added.

**Blocked by:** 04

**Status:** ready-for-agent

- [ ] An `infra/` folder contains Bicep generated/maintained from the Aspire AppHost model
- [ ] An `azure.yaml` exists at the repo root describing the deployment targets for `azd`
- [ ] Running `azd provision` (or equivalent) successfully creates the Azure Function App, its storage account, and Application Insights in Lee's Azure subscription
- [ ] The provisioned resources are confirmed to exist (e.g. via Azure Portal or `az` CLI) after provisioning
- [ ] Re-running provisioning is idempotent (no errors, no duplicate resources) — supports the "expand or recreate infra easily" goal from ADR-0004
