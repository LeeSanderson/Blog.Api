# Coding Standards

This solution enforces most of its coding standards through tooling rather than
prose, so a reviewer (human or agent) should not re-litigate anything the build
already enforces — it fails the build via warnings-as-errors before a PR is even
opened.

## Enforced automatically — Opinionated.DotNet.CodingStandards

[`Opinionated.DotNet.CodingStandards`](https://github.com/LeeSanderson/OpinionatedDotNetCodingStandards)
is a `GlobalPackageReference` in `Directory.Packages.props`, applied to every
project in the solution via Central Package Management. It wires in five Roslyn
analyzer packages (StyleCop.Analyzers, Microsoft.CodeAnalysis.NetAnalyzers,
Microsoft.CodeAnalysis.CSharp.CodeStyle, SonarAnalyzer.CSharp, Meziantou.Analyzer)
plus a curated `.editorconfig`, with nullable reference types enabled and all
warnings treated as errors.

Full rule reference (rule ID, description, severity, help link):
https://github.com/LeeSanderson/OpinionatedDotNetCodingStandards/blob/main/docs/rule-reference.md

### Style and naming, at a glance

- **`var`** — use it everywhere the type is apparent or inferred.
- **Namespaces** — file-scoped (`namespace Foo;`), not block-scoped.
- **Braces** — required on every control-flow block, even single-line bodies.
- **Expression bodies** — preferred for properties/accessors; block bodies
  preferred for methods/constructors.
- **Wrapped binary expressions** — operator goes at the start of the
  continuation line.
- **Accessibility modifiers** — explicit on all non-interface members.
- **Naming** — `IPascalCase` interfaces, `PascalCase` types/methods/properties,
  `_camelCase` private fields, `PascalCase` for `const`/`static readonly`,
  `camelCase` parameters and locals.
- **`using` directives** — outside the namespace, `System.*` first, no blank
  lines between groups.
- **Keywords over BCL names** — `int`/`string`/`bool`, not `Int32`/`String`/`Boolean`.

### Banned APIs (default-on, opt-out per project)

| Banned | Reason |
|--------|--------|
| `DateTime.Now` / `DateTimeOffset.Now` | prefer explicit UTC |
| Culture-insensitive `string.Compare`/`IndexOf` | locale-sensitive comparison is non-deterministic |
| `Enum.TryParse` without `ignoreCase` | silently case-sensitive parsing is a common bug |
| `Math.Round` without `MidpointRounding` | banker's-rounding default surprises most developers |
| `new CultureInfo(...)` | prefer the cached `CultureInfo.GetCultureInfo` |
| `Tuple<T1, T2, ...>` | prefer value-tuple syntax `(T1, T2)` |
| `Newtonsoft.Json` types | prefer `System.Text.Json` |

Each ban can be toggled off per project (e.g. `<BanNonUtcDateApis>false</BanNonUtcDateApis>`)
if a project has a genuine reason to opt out — see the package README for the
full property list.

## Repo-specific conventions (not covered by the package)

These aren't tooling-enforced, so they're exactly what a **Standards** review
should be checking for:

- **Testing stack** — xUnit + FluentAssertions for assertions + Moq for mocking;
  coverlet for coverage. See the [`tdd` skill](.claude/skills/tdd/SKILL.md) for
  what a good test looks like on this stack.
- **Project layout** — one folder per app under `apps/{app-name}/`, each
  optionally with its own `backend/` and/or `frontend/`; cross-app code goes in
  `shared/backend/` or `shared/frontend/{framework}/`; `host/` is the single
  composition-root Azure Functions project and owns no app-specific business
  logic, only cross-cutting wiring. See `CONTEXT.md`.
- **REPR pattern** — backend endpoints follow Request-Endpoint-Response via the
  hand-rolled `Endpoint<TRequest, TResponse>`-style base class in
  `shared/backend`, not a third-party REPR library. See
  `docs/adr/0005-hand-rolled-repr-base.md` for why.

Architectural decisions that shaped these conventions are recorded in
`docs/adr/` — check there before proposing something an ADR already settled.
