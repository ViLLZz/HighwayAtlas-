# 🛣️ Bulgarian Motorways & Highways Atlas

**Interactive all-motorways atlas** for Bulgaria with lot-level click interactions, route status filtering, and source provenance in the info panel.

🗺️ **[Canonical Atlas Output](exports/atlas/index.html)** | 🧭 **[V5 Alias](exports/atlas/bulgarian-motorway-atlas-v5.html)**

---

## 🌟 Current Focus

The current production output focuses on a clean, operational motorway atlas:

- 🗺️ Bulgaria-first map bounds and route-level filtering
- 🎯 Clickable lots and segments with detailed on-map panel
- 📌 Route + lot statuses (open, construction, planned, closed)
- 📚 Source provenance field in selection details (name + link)
- 🧪 Source-coverage controls to isolate verified, mixed, and modeled sections
- 🌍 BG/EN language switch in the same interface

---

## Project goals

- ✅ Model the national motorway network with modern C# records
- ✅ Import and validate OSM / Overpass GeoJSON geometry
- ✅ Analyze distances, bearings, completion metrics, and route health
- ✅ Export to GeoJSON, HTML, TypeScript, KML, and more
- ✅ Deliver a Bulgaria-focused motorway dashboard with lot-level interactions
- ✅ Keep provenance visible inside the interactive inspector
- ✅ Support BG/EN atlas presentation from one export path
- ✅ Maintain a clean canonical output in `exports/atlas/index.html`

## Starter solution layout

- `src/Motorway.Domain` — core records and network aggregate
- `src/Motorway.Engine` — routing, geometry, statistics, validation
- `src/Motorway.Infrastructure` — seed network, GeoJSON import, exports
- `src/Motorway.Console` — console runner for debug, stats, and exports
- `tests/Motorway.Tests` — automated regression tests for seed data, validators, and exporters

## Diagnostics and provenance

The atlas now treats geometry quality and source fidelity as first-class product concerns:

- `network-diagnostics.json` contains both geometry diagnostics and route continuity diagnostics
- the atlas UI can filter sections by source coverage (`verified`, `mixed`, `modeled`)
- the selection panel surfaces source-quality context alongside the published source link
- CI uploads the generated atlas outputs as build artifacts for review

## Alpha quality gate

Run the full alpha verification flow with one command:

```bash
./scripts/verify-alpha.sh
```

This builds the solution, runs automated tests, regenerates the atlas, and verifies the expected outputs exist.

Detailed notes are in `docs/alpha-readiness.md`.
The broader roadmap is in `docs/v3-upgrade-plan.md`.

## Current seed coverage

The default seed covers all 9 flagship national corridors with curated geometry,
lot-level data, budget figures, contractor names, milestone timelines, and source
provenance. Each segment includes Haversine-calibrated shape waypoints so lot
boundary markers align with real geographic positions along the road.

| Route | Name | Length | Status |
|-------|------|--------|--------|
| A1 | Trakia | 374 km | 🟢 Open |
| A2 | Hemus | 418 km | 🟡 Partial UC |
| A3 | Struma | 169 km | 🟡 Partial Planned |
| A4 | Maritsa | 155 km | 🟢 Open |
| A5 | Cherno More | 39 km | 🟢 Open |
| A6 | Europe | 84 km | 🟢 Open |
| E79 | Vidin – Botevgrad | 180 km | 🟡 Partial UC |
| RVT | Ruse – Veliko Tarnovo | 67 km | 🔴 UC |
| MB | Mezdra – Botevgrad | 33 km | 🔴 UC |

**Network totals:** 1 506 km · 991 km open (65.8%) · 250 km UC · 265 km planned

## Recommended next steps

1. Run `dotnet run --project src/Motorway.Console` to regenerate the atlas.
2. Enrich shape geometry with dense Overpass API waypoints for higher-fidelity lot paths.
3. Wire in live status updates via an МРРБ / API Bulgaria feed adapter.
4. Add a WPF + Mapsui UI layer after the core engine is stable.

## Browser Preview

The console export now writes the same canonical atlas to both:

- `exports/atlas/index.html`
- `exports/atlas/bulgarian-motorway-atlas-v5.html`
- `exports/atlas/network-diagnostics.json`

Legacy story-heavy prototype remains available in `exports/atlas/hemus-viz-4.5.html`.

## GitHub Pages Preview

The repository includes a GitHub Pages deployment workflow in `.github/workflows/deploy-pages.yml`.
Once the project is pushed to a GitHub repository with Pages enabled, every push to `main` will:

- run the alpha verification flow
- regenerate the atlas in `exports/atlas`
- publish the static site to GitHub Pages

That gives you a public HTTPS URL you can open on an iPad or any other device.

### Quick Start

```bash
# Run the alpha quality gate
./scripts/verify-alpha.sh

# Generate atlas files only
dotnet run --project src/Motorway.Console

# Optional local server for browser preview
dotnet run --project src/Motorway.Console -- --serve
```

### Key Features (Canonical Atlas)

- All 9 motorway corridors with Haversine-calibrated shape geometry
- Route and status filters (open, construction, planned, closed)
- Source-coverage filters (verified, mixed, modeled)
- Clickable lot and section interactions from map and sidebar
- Lot-level budget, contractor, completion %, and forecast year
- BG/EN localized interface text and milestone timeline
- Source provenance link in the right-side detail panel
- Continuity and geometry diagnostics exported beside the HTML atlas

Useful commands:

- alpha verification: `./scripts/verify-alpha.sh`
- export files only: `dotnet run --project src/Motorway.Console`
- export and serve locally: `dotnet run --project src/Motorway.Console -- --serve`
- custom port: `dotnet run --project src/Motorway.Console -- --serve --port=5057`

When serving, the console prints a localhost link and opens the preview in the default browser.

For alpha release notes, see `docs/alpha-readiness.md`.
For the v3.0 upgrade process, see `docs/v3-upgrade-plan.md`.
For the larger product roadmap, see `docs/upgrade-roadmap.md`.

## Notes

The initial repository is dependency-light and keeps the mapping core free of external packages so you can evolve it in either Visual Studio or VS Code.
