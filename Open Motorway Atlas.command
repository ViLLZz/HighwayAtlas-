#!/bin/zsh
set -e

cd "/Users/velikodimitrov/Desktop/Motorway"

echo "Generating latest atlas preview..."
dotnet run --project src/Motorway.Console -- --no-open

HTML_PATH="$PWD/exports/atlas/index.html"

if [[ ! -f "$HTML_PATH" ]]; then
  echo "Preview file was not created: $HTML_PATH"
  exit 1
fi

echo "Opening atlas in Chrome..."
if open -a "Google Chrome" "$HTML_PATH" 2>/dev/null; then
  exit 0
fi

echo "Chrome was not found. Opening in the default browser instead..."
open "$HTML_PATH"
