#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

echo "==> Building solution"
dotnet build Motorway.sln -v minimal

echo "==> Running automated tests"
dotnet test Motorway.sln -v minimal --no-build

echo "==> Regenerating atlas outputs"
dotnet run --project src/Motorway.Console --no-build

echo "==> Verifying generated files"
test -f exports/atlas/index.html
test -f exports/atlas/bulgarian-motorway-atlas-v5.html
test -f exports/atlas/network.geojson
test -f exports/atlas/network-diagnostics.json

echo "Alpha verification passed."
