# Data Provenance

This atlas uses a seeded motorway dataset in `src/Motorway.Infrastructure/NetworkData.cs`, but the 3.1 upgrade no longer treats provenance as a single free-text source field.

## 3.1 provenance model

Each `RouteSegment` now carries two parallel evidence tracks:

- official record:
   - `OfficialSourceName`
   - `OfficialSourceUrl`
   - `OfficialSourceKind`
   - `OfficialSourceVerifiedOn`
- secondary narrative support:
   - `SecondarySourceName`
   - `SecondarySourceUrl`

For compatibility, `SourceName` and `SourceUrl` continue to expose the primary source shown in legacy surfaces, but the canonical provenance model is now official-plus-secondary.

## Evidence grading

Exporter and diagnostics classify sections into evidence grades:

- `official-route`: route-specific official source
- `official-network`: network-wide official source
- `official-network-plus-secondary`: official source supplemented by secondary narrative
- `secondary-only`: secondary reference exists without official backing
- `unattributed`: neither source track is populated

The current seed defaults all segments to an official API Bulgaria bulletin entrypoint, then preserves historical Wikipedia-style references as secondary narrative support when present.

## Accuracy note

The dataset is still a curated atlas seed, not a legal engineering register.
The 3.1 work raises traceability and official-source visibility substantially, but route-specific official records still need to be filled in section by section for the highest confidence level.

## Diagnostics

`network-diagnostics.json` now exports:

- geometry diagnostics
- continuity diagnostics
- provenance diagnostics

The provenance diagnostics report:

- segments with official sources
- route-specific official references
- network-wide official references
- secondary narrative references
- unattributed sections

## Recommended verification workflow

1. Pull the latest API Bulgaria bulletins and route-specific notices.
2. Replace generic official URLs with route-specific official records where available.
3. Keep secondary narrative references only as supporting context, not primary evidence.
4. Cross-check geometry with OSM or official GIS where possible.
5. Regenerate and verify with:

```bash
./scripts/verify-alpha.sh
```

6. Review the outputs in:
    - `exports/atlas/index.html`
    - `exports/atlas/bulgarian-motorway-atlas-v5.html`
    - `exports/atlas/network-diagnostics.json`
