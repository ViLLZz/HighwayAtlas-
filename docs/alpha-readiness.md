# Alpha Readiness

This repository now has a single repeatable validation flow for alpha builds.

## Quality gate

Run:

```bash
./scripts/verify-alpha.sh
```

The script performs four checks:

1. Builds the full solution.
2. Runs automated regression tests.
3. Regenerates atlas outputs through the console app.
4. Confirms the expected generated files exist, including geometry diagnostics.

## What is covered

- Seed-network totals and route counts remain stable.
- Geometry and cumulative km stay internally consistent.
- Validator warnings remain empty for the default network.
- Exporter output still contains the key alpha UI controls and Hemus geometry fixes.
- GeoJSON export still produces one feature per segment.
- Geometry diagnostics are exported for route-density and precision review.

## Current alpha constraints

- Route geometry is still curated by hand for several corridors and is not yet sourced from dense official GIS polylines.
- Browser-level visual regression testing is still manual.
- The CI workflow validates build, test, and export integrity, but it does not yet publish preview artifacts.

## Automation

The repository now includes a CI workflow at `.github/workflows/alpha-verification.yml`.

It runs the same `./scripts/verify-alpha.sh` flow used locally, keeping local and hosted validation aligned.

## Recommended release routine

1. Run `./scripts/verify-alpha.sh`.
2. Start a local preview with `dotnet run --project src/Motorway.Console -- --serve`.
3. Smoke-test BG and EN modes, map presets, selection clearing, and Hemus east alignment.
4. Only then publish or archive the generated atlas files.
