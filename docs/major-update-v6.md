# Major Update V6 — Premium Bulgaria Atlas

## Intent

This update moves the project further away from a prototype map and closer to a premium operational atlas experience.

## UI direction

The atlas now aims for:

- contained dashboard composition
- stronger motorway naming (`Hemus`, `Trakia`, `Struma`, etc.)
- richer atlas chrome and premium ambient texture
- persistent on-map detail popups for lots and sections
- explicit data-audit surfacing instead of hidden assumptions

## What changed in this wave

- Route chips now emphasize motorway identity, not just route code.
- The map uses a tighter Bulgaria-only framing.
- Lot clicks open persistent on-map detail cards.
- Audit messaging distinguishes explicit lots from derived lots.
- Top-level interface copy is more product-grade and monitoring-oriented.

## Data maturity model

The atlas currently uses three levels of confidence:

1. **Segment-level curated seed**
2. **Explicit lot modeling**
3. **Derived lot modeling**

The UI should always expose which layer the user is seeing.

## Next repository expansion targets

1. Add explicit lot definitions for non-Hemus corridors.
2. Add source snapshots / provenance references per corridor.
3. Separate legacy prototype outputs from canonical atlas exports.
4. Add a minimal export smoke-test checklist.
5. Introduce a structured data folder for future imported corridor datasets.

## Product goal

The target is a Bulgaria motorway atlas that feels closer to a premium command center than a demo page.
