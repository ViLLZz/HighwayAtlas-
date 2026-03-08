# Data Provenance

This atlas currently uses a seeded motorway dataset in `src/Motorway.Infrastructure/NetworkData.cs` and exports provenance metadata into the interactive HTML panel.

## Current provenance model

Each `RouteSegment` carries:

- `SourceName`
- `SourceUrl`

If a segment does not provide an explicit source, defaults are applied in `NationalNetworkSeed.CreateSegment`:

- Source name: `API Bulgaria public road status + OSM geometry cross-check`
- Source URL: `https://www.api.bg/`

## Accuracy note

The present dataset is a curated seed meant for product UX and interaction validation.
Geometry and lot metadata should be continuously reconciled against official publications and GIS/OSM updates before being treated as authoritative for legal, procurement, or operational decisions.

## Recommended verification workflow

1. Pull latest public road agency updates (API Bulgaria pages and bulletins).
2. Cross-check motorway geometry with OSM / geospatial sources.
3. Update section lengths, lot statuses, and opening forecasts in `NetworkData.cs`.
4. Regenerate atlas with:

```bash
dotnet run --project src/Motorway.Console
```

5. Validate the updated output in:
   - `exports/atlas/index.html`
   - `exports/atlas/bulgarian-motorway-atlas-v5.html`
