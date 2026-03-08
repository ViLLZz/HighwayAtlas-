# Motorway Atlas — 10-Step Refining Process

Date: 2026-03-07

## Goal
Make the atlas feel calmer, more symmetrical, and more premium, with a Tesla / Apple-like hierarchy: map first, data second, clutter last.

## 10-step pass

1. **Reduce map obstruction**
   - Keep overlays compact.
   - Preserve clear empty space around the motorway network on first load.

2. **Enforce spacing symmetry**
   - Use one spacing rhythm across top bar, floating cards, sidebar cards, and headlines.
   - Avoid mixed padding values that make the layout feel improvised.

3. **Normalize card geometry**
   - Make headline cards, route cards, lot cards, and floating windows feel cut from the same design system.
   - Keep corner radius, border opacity, and shadow depth consistent.

4. **Refine route tab shelf**
   - Keep route pills compact, evenly aligned, and horizontally scrollable without visual noise.
   - Prevent oversized gaps or broken wrapping.

5. **Improve first-read hierarchy**
   - The user should understand, within 3 seconds:
     - what the map shows,
     - what is built,
     - what is under construction,
     - what is planned.

6. **Tighten KPI storytelling**
   - Only show the most decision-useful metrics in the hero panel.
   - Remove decorative duplication and repeated signals.

7. **Debug interaction states**
   - Re-check route, segment, and lot selection states.
   - Ensure active, hover, and filtered states feel distinct and deliberate.

8. **Polish glass UI treatment**
   - Keep blur subtle.
   - Use lighter borders, cleaner gradients, and restrained glow rather than heavy effects.

9. **Balance the right-side overlays**
   - Keep selection and timeline cards equal in visual weight.
   - Prevent one card from dominating the map more than the other.

10. **Validate and export after each visual pass**
    - Build the solution.
    - Regenerate the atlas.
    - Review for regressions in map visibility, card overlap, and data readability.

## Immediate audit focus
- Map dominance on first impression
- Symmetry between left and right overlay masses
- Cleaner route navigation shelf
- Less visual heaviness in secondary panels
- Better premium feel without losing density
