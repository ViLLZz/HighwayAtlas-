#!/bin/bash

# Open Hemus VIZ 4.5 in default browser
cd "$(dirname "$0")/exports/atlas"
open hemus-viz-4.5.html

echo "🎬 Hemus VIZ 4.5 opened in your default browser!"
echo ""
echo "Keyboard shortcuts:"
echo "  Space  - Play/Pause timeline"
echo "  S      - Story Mode"
echo "  C      - Comparison Mode"
echo "  Esc    - Close modals / Exit modes"
echo ""
echo "Enjoy exploring 52 years of Hemus Motorway history!"
