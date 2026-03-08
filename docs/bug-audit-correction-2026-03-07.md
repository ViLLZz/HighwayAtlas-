# Motorway Atlas Bug / Audit Correction Log — 2026-03-07

This pass records a focused 20-step correction cycle for the canonical Bulgaria motorway atlas.

## 20-step correction list

1. **Popup refresh path audited** — verified the selected lot state and popup lifecycle inside the atlas renderer.
2. **Marker popup binding added** — each lot marker now owns its own popup payload instead of relying only on a detached map popup.
3. **Active popup lifecycle normalized** — previous popup instances are explicitly closed before a new lot or segment popup is opened.
4. **Lot popup refresh fixed** — selected lots now refresh popup content per lot selection instead of feeling visually stale.
5. **Selected marker emphasis added** — active lot markers now receive stronger scale, ring, and shadow styling.
6. **Viewport churn reduced** — repeated re-renders no longer re-run the same camera animation when the logical focus has not changed.
7. **Default basemap upgraded** — the atlas now opens on CARTO Voyager for a cleaner, more premium road-style cartographic base.
8. **Tile rendering refined** — map tiles now receive slightly improved saturation, contrast, and brightness for clearer visual separation.
9. **Map frame elevated** — the contained map panel now has stronger chrome, shadow, and border treatment.
10. **Topbar upgraded** — header now behaves as a premium sticky navigation strip with stronger visual hierarchy.
11. **Sidebar contrast improved** — key information panels now use more consistent elevated panel styling.
12. **Headline strip refined** — top summary cards now better match the premium glass-and-chrome visual language.
13. **Floating detail cards upgraded** — inspector and timeline cards now use stronger elevated panel treatment.
14. **Route chip readability improved** — route pills now feel more tactile and legible.
15. **Lot marker legibility improved** — markers are larger and easier to identify on the map.
16. **Popup card styling upgraded** — popup cards now use better spacing, chrome, and content density.
17. **Map control styling improved** — zoom and map controls now visually match the atlas shell.
18. **Performance UX improved** — repeated language and filter refreshes should feel less sluggish due to reduced camera motion.
19. **Canonical exporter preserved** — all changes remain inside the single canonical exporter path.
20. **Validation pending after export** — build and export must be rerun after this correction pass to confirm final output integrity.

## Remaining high-value follow-up items

- Replace more derived lots with explicit corridor-level lot definitions.
- Add corridor-specific popup imagery or schematic badges if asset support is introduced later.
- Split large inline CSS/JS from the exporter into dedicated template assets when the project stabilizes.
- Add automated browser validation for popup behavior and filter transitions.
