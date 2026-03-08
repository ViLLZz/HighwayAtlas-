# Motorway Atlas v3.0 Upgrade Plan

This document defines the version 3.0 upgrade path as a 20-step program.

## v3.0 goals

- Stabilize the interactive atlas UI so every panel behaves predictably.
- Increase route-geometry fidelity and track precision with diagnostics.
- Harden the engine, validation, export, and release workflow.
- Prepare the repository for alpha-quality iteration with measurable gates.

## 20-step plan

1. Fix all panel and overlay regressions in the generated atlas.
2. Make every map interaction reversible without hidden UI state.
3. Reduce low-zoom map clutter and prioritize route legibility.
4. Standardize top-level controls for language, map style, and route scope.
5. Refine the selection panel into passive, compact, and detailed modes.
6. Densify synthetic geometry on the most visibly approximate corridors.
7. Add engine-level geometry diagnostics for span density and turn quality.
8. Export diagnostics artifacts next to the atlas outputs.
9. Extend automated tests to cover geometry regressions and exporter behavior.
10. Keep the alpha verification script as the single local quality gate.
11. Mirror the same verification flow in CI.
12. Add route continuity checks across adjacent segments of the same corridor.
13. Add diagnostics for sparse long segments and anomalous directional jumps.
14. Create per-route accuracy notes in the documentation.
15. Add artifact publishing in CI for atlas preview review.
16. Introduce import pipelines for denser OSM or official geometry sources.
17. Add richer provenance metadata per segment and per lot.
18. Add browser-level smoke scenarios for the most critical UI states.
19. Prepare a curated alpha release checklist and change log workflow.
20. Freeze a tagged v3.0 alpha build after geometry, UI, and diagnostics review.

## Status snapshot

Completed or active in the current repository state:

- Steps 1 through 11 are now materially underway or completed.
- Steps 12 through 20 remain the next major tranche.

## Immediate next tranche

The next highest-value implementation batch for v3.0 is:

1. Route continuity validation across shared corridor boundaries.
2. Per-route geometry accuracy reporting in docs and diagnostics.
3. CI artifact publishing of generated atlas files.
4. Denser source geometry import for remaining approximate routes.
