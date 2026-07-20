# Frontend deploys push directly to leesanderson.github.io, no PR gate

Each app's frontend builds to compiled/minified static output (JS bundles, Blazor WASM) that isn't meaningfully reviewable in a pull request diff. CI therefore pushes the built output directly to its target folder in the `leesanderson.github.io` repo on merge to `main`, matching the existing pattern used by BuzzerBee, rather than opening a PR for review. The safety net is the build/test gate in the app's own CI workflow, not a human diff review of compiled output.
