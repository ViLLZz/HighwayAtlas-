# Architecture Notes

## Phase 1 (current)

Console-first architecture focused on correctness, diagnostics, and export.

- `Motorway.Domain` keeps pure records and network aggregate state.
- `Motorway.Engine` contains geometry math (`GeoMath.HaversineKm`, `BearingDegrees`), statistics, and validation.
- `Motorway.Infrastructure` handles seed data (curated 9-route, 20-segment network), lot descriptors, GeoJSON and HTML exports.
- `Motorway.Console` acts as the debug runner generating `exports/atlas/index.html`.
- `Motorway.Tests` provides automated regression coverage for seed data, validators, GeoJSON export, and HTML atlas smoke checks.

Recent product-grade additions:

- route continuity diagnostics for adjacent sections of the same corridor
- source-quality classification per segment (`official`, `mixed`, `modeled`)
- atlas-side source coverage controls for provenance-aware browsing
- CI artifact publishing of generated atlas outputs

### Key design decisions

| Decision | Detail |
|----------|--------|
| Shape cumulative km | Computed via Haversine between consecutive waypoints, then scaled to authoritative `lengthKm` |
| Lot path clipping | JS `buildLotPath` uses `point.cumulativeKm` brackets; C# `InterpolatePoint` mirrors the same basis |
| Localization | All user-facing strings are `LocalizedText { Bg, En }` selected via `pick()` in JS |
| Lot data | A2 subroutes use curated `BuildLots` cases; all other segments use `BuildGeneratedLots` |
| Source quality | Segment source quality is derived from lot provenance: all verified lots = `official`, all modeled spans = `modeled`, mixed lot sets = `mixed` |
| Continuity diagnostics | Engine evaluates adjacent route sections using endpoint gap and directional delta to flag transitions for review |
| Alpha verification | `scripts/verify-alpha.sh` builds, tests, exports, and checks output artifacts in one pass |

## Alpha verification scope

The repository now has a repeatable alpha-quality gate:

- solution build
- automated tests
- atlas regeneration
- generated output existence checks

The detailed release workflow is described in `docs/alpha-readiness.md`.

## Phase 2

Potential UI layers:

- WPF + Mapsui 4.x
- .NET MAUI
- Blazor Hybrid

## Suggested next engine features

- Import dense Overpass API shape geometry (replace hand-placed waypoints)
- Live МРРБ / API Bulgaria status synchronization adapter
- Ramer-Douglas-Peucker simplification for rendering performance
- Sharp-turn detection and snapping
- Segment splitting and merge tools
- Undo / redo stack
- OpenElevation cache
