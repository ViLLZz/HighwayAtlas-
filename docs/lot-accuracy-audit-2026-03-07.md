# Lot Accuracy Audit — 2026-03-07

## Current atlas snapshot

Generated from the canonical exporter after the latest Bulgaria-focused UI overhaul.

## Coverage summary

- Total routes: 9
- Total segments: 15
- Total visible lots in canonical seed: 31
- Lots with manually modeled boundaries and completion values: 10
- Lots with derived boundaries from section splits: 21
- Manual lot coverage: 32.3%

## What is currently strong

### Explicit lot modeling
The most detailed lot-level modeling currently exists on the Hemus corridor:

- `A2-01`
- `A2-02`
- `A2-03`
- `A2-04`

These lots have manually entered lot lengths, completion values, and forecast years in the exporter.

## What is still approximate

The remaining corridors still use generated lot splits derived from section length:

- `A1`
- `A3`
- `A4`
- `A5`
- `A6`
- `E79`
- `RVT`
- `MB`

For those routes, lot lengths are computed by dividing a route section into one, two, or three pieces depending on overall section length.
That makes the UI coherent, but it is not the same as using officially published procurement or construction-lot boundaries.

## Completion-value audit

Current completion behavior comes from two sources:

1. Explicit per-lot values for manually modeled Hemus lots.
2. Fallback inheritance from section status/completion for derived lots.

That means many non-Hemus lots still display completion values that are section-level approximations rather than official lot-level progress figures.

## Priority next step for accuracy

To make the atlas close to authoritative, the next data pass should replace generated lots with official lot boundaries and progress values for:

1. `A1 Trakia`
2. `A3 Struma`
3. `A4 Maritsa`
4. `A6 Europe`
5. `A5 Cherno More`
6. `E79 Vidin–Botevgrad`
7. `RVT Ruse–Veliko Tarnovo`
8. `MB Mezdra–Botevgrad`

## Conclusion

The atlas is now much stronger as a product UI, but lot accuracy is still mixed:

- Hemus lots: materially modeled
- Most other lots: derived for UX continuity

So the current atlas is suitable for exploration, storytelling, and product validation, but not yet a full official lot-register replacement.
