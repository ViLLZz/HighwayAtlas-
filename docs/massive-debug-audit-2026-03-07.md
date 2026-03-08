# Motorway Atlas — Massive Debug and Accuracy Audit

Date: 2026-03-07

## What was audited
- route visibility logic
- stage/status toggles
- map rendering order
- runtime stability of the hero panel
- first-read map clarity
- data accuracy messaging

## Bugs found

1. **Hero render runtime break**
   - `renderHero()` expected `metrics.statusCounts`, but `computeMetrics()` did not return it.
   - Result: downstream UI behavior could fail and make the map feel blank or partially broken.

2. **Stage filtering was too hidden**
   - Detailed status toggles existed, but there was no obvious user-facing model for:
     - built
     - building in process
     - to be built
   - Result: controls felt unclear and non-intuitive.

3. **Map could feel visually empty**
   - When filtering down to a narrow set, the map lost overall network context.
   - Result: the atlas looked under-rendered instead of intentionally filtered.

4. **Accuracy was implied more than surfaced**
   - The atlas already distinguished explicit vs derived lots, but this needed stronger visual explanation.
   - Result: users could over-trust modeled geometry as official parcel-accurate GIS.

## Fixes applied

1. Added `statusCounts` to computed metrics.
2. Added smart stage presets:
   - All stages
   - Built
   - Building
   - To build
3. Kept detailed status toggles for granular filtering.
4. Made KPI cards clickable for quick stage filtering.
5. Made legend pills clickable for single-stage focus.
6. Added a faint full-network context layer behind active routes.
7. Added visible accuracy messaging in the notes panel.
8. Preserved audit share display based on explicit vs derived lots.

## Accuracy position
This atlas is **audited and provenance-aware**, but it is **not yet a full official lot-level GIS mirror**.

Current accuracy sources:
- curated network seed data
- official public notices / bulletin snapshot references
- explicit vs derived lot tracking

What is still needed for true pinpoint accuracy:
- official structured per-lot API or GIS feed
- official geometry for every lot boundary and alignment update
- machine-readable construction status updates from the source authority

## Recommendation
Treat the current atlas as:
- highly usable for planning and visual comparison
- transparent about modeled vs explicit data
- not a substitute for an official cadastral / engineering GIS system
