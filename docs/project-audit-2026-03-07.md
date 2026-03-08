# Project Audit — 2026-03-07

## Scope

Audit performed after the Bulgaria-focused atlas UI overhaul.

## Current strengths

- The solution builds cleanly.
- The console export produces a canonical atlas in `exports/atlas/index.html`.
- The V5 alias is regenerated from the same exporter output.
- The atlas now keeps the map constrained around Bulgaria and surfaces lot-level provenance.
- The domain/infrastructure split remains simple and understandable.

## Confirmed issues

### 1. Legacy artifact duplication

The `exports/atlas` folder still contains multiple historical frontends:

- `index.html` — current canonical atlas
- `bulgarian-motorway-atlas-v5.html` — alias of canonical atlas
- `bulgarian-motorways-dashboard.html` — older standalone dashboard
- `hemus-viz-4.5.html` — older story-heavy prototype

This is manageable, but it creates ambiguity about which output is production.

### 2. Documentation drift

Several root and export documents still describe the older Hemus-only cinematic product instead of the current all-motorways atlas:

- `PROJECT-COMPLETE.md`
- `EXECUTIVE-SUMMARY.md`
- `exports/atlas/README-HEMUS-VIZ-4.5.md`
- `exports/atlas/UPGRADES-SUMMARY.md`

These should be archived, renamed as legacy, or rewritten.

### 3. Data maturity is still seed-based

The project now shows provenance fields, but route geometry and lot metadata still come from curated seed data in `src/Motorway.Infrastructure/NetworkData.cs`.

That is good enough for product and UI iteration, but not yet a fully authoritative data pipeline.

### 4. Canonical launcher ambiguity

The workspace still contains two launcher scripts:

- `Open Motorway Atlas.command`
- `Open Hemus VIZ 4.5.command`

The first matches the current product direction; the second is legacy-oriented.

## Recommended next actions

1. Archive or move Hemus-specific legacy files into a `legacy/` folder.
2. Keep only one user-facing launcher for the canonical atlas.
3. Replace or supplement seed geometry with officially reconciled imported data.
4. Add a small smoke-test checklist for export validation:
   - build succeeds
   - `index.html` opens
   - route filters work
   - lot clicks update inspector
   - Bulgaria bounds remain enforced
5. Consider removing `bulgarian-motorway-atlas-v5.html` once users are fully pointed to `index.html`.

## Validation snapshot

- Build status: passed
- Problems panel: no errors
- Export regeneration: passed

## Conclusion

The project is in a much better product state than the older cinematic branch, but repository cleanup is still needed.
The main technical risk is not compilation — it is ambiguity between canonical and legacy outputs, plus the remaining gap between seeded and officially maintained data.
