#!/usr/bin/env python3
"""
Apply all HtmlAtlasExporter.cs changes:
1. Add openYear to segment serialization (C#)
2. Fix getPlaybackOperationalYear in JS
3. Add progressive km playback rendering
4. Fix passesPlaybackFilter  
5. Add cinematic year overlay + CSS
6. Add km counter in playback
7. Improve playback speed and UX
"""

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/HtmlAtlasExporter.cs'

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

changes = []

# ── CHANGE 1: Add openYear to C# segment serialization ───────────────────────
old1 = '                    milestones = segment.Milestones.OrderBy(item => item.Year).Select(item => new { year = item.Year, label = new { bg = item.Label.Bg, en = item.Label.En }, state = item.State }).ToArray(),\n                    shape = segment.Shape.Select(ToPoint).ToArray(),'
new1 = ('                    openYear = segment.Status == SegmentStatus.Open\n'
        '                        ? (segment.Milestones.OrderBy(m => m.Year).LastOrDefault(m => m.State == "success")?.Year ?? segment.StartYear)\n'
        '                        : (int?)null,\n'
        '                    milestones = segment.Milestones.OrderBy(item => item.Year).Select(item => new { year = item.Year, label = new { bg = item.Label.Bg, en = item.Label.En }, state = item.State }).ToArray(),\n'
        '                    shape = segment.Shape.Select(ToPoint).ToArray(),')
changes.append(('ADD openYear to segment serialization', old1, new1))

# ── CHANGE 2: Fix getPlaybackOperationalYear ──────────────────────────────────
old2 = ('        function getPlaybackOperationalYear(segment) {\n'
        '            if (segment.status !== \'Open\') return new Date(atlas.generatedAtUtc).getFullYear();\n'
        '            const successYears = (segment.milestones || []).filter(item => item.state === \'success\').map(item => item.year).sort((a, b) => a - b);\n'
        '            return segment.startYear || successYears.at(-1) || segment.forecastOpenYear || new Date(atlas.generatedAtUtc).getFullYear();\n'
        '        }')
new2 = ('        function getPlaybackOperationalYear(segment) {\n'
        '            if (segment.status !== \'Open\') return new Date(atlas.generatedAtUtc).getFullYear();\n'
        '            // openYear = last success milestone year (when this section was FULLY operational)\n'
        '            // startYear = construction start (first showing during playback)\n'
        '            return segment.openYear || segment.startYear || segment.forecastOpenYear || new Date(atlas.generatedAtUtc).getFullYear();\n'
        '        }\n\n'
        '        function getSegmentConstructionStartYear(segment) {\n'
        '            return segment.startYear || getPlaybackOperationalYear(segment);\n'
        '        }\n\n'
        '        // Returns 0.0 (not yet) to 1.0 (complete) for the segment at a given playback year\n'
        '        function getSegmentPlaybackFraction(segment) {\n'
        '            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();\n'
        '            if (state.playbackYear >= currentYear) return segment.completionPercent / 100;\n'
        '            if (segment.status === \'Open\') {\n'
        '                const opYear = getPlaybackOperationalYear(segment);\n'
        '                const startY = getSegmentConstructionStartYear(segment);\n'
        '                if (state.playbackYear >= opYear) return 1.0;\n'
        '                if (state.playbackYear < startY) return 0.0;\n'
        '                // Linear interpolation during construction phase\n'
        '                const span = Math.max(1, opYear - startY);\n'
        '                return Math.min(0.98, (state.playbackYear - startY) / span);\n'
        '            }\n'
        '            if (segment.status === \'UnderConstruction\') {\n'
        '                const startY = getSegmentConstructionStartYear(segment);\n'
        '                if (state.playbackYear < startY) return 0.0;\n'
        '                const yearsTotal = Math.max(1, currentYear - startY);\n'
        '                const yearsPlayed = state.playbackYear - startY;\n'
        '                return Math.min(segment.completionPercent / 100, (yearsPlayed / yearsTotal) * (segment.completionPercent / 100));\n'
        '            }\n'
        '            return 0; // Planned – never shown in historical playback\n'
        '        }\n\n'
        '        // Truncate a lat/lon polyline to a fraction [0..1] of its total arc length\n'
        '        function truncatePolylineToFraction(latLngs, fraction) {\n'
        '            if (!latLngs || latLngs.length < 2) return latLngs || [];\n'
        '            if (fraction <= 0) return [];\n'
        '            if (fraction >= 1.0) return latLngs;\n'
        '            let totalLen = 0;\n'
        '            const lengths = [];\n'
        '            for (let i = 1; i < latLngs.length; i++) {\n'
        '                const dy = latLngs[i][0] - latLngs[i-1][0];\n'
        '                const dx = latLngs[i][1] - latLngs[i-1][1];\n'
        '                const len = Math.sqrt(dx*dx + dy*dy);\n'
        '                lengths.push(len);\n'
        '                totalLen += len;\n'
        '            }\n'
        '            const target = totalLen * fraction;\n'
        '            let cum = 0;\n'
        '            const result = [latLngs[0]];\n'
        '            for (let i = 0; i < lengths.length; i++) {\n'
        '                if (cum + lengths[i] >= target) {\n'
        '                    const t = (target - cum) / Math.max(lengths[i], 1e-9);\n'
        '                    result.push([\n'
        '                        latLngs[i][0] + t * (latLngs[i+1][0] - latLngs[i][0]),\n'
        '                        latLngs[i][1] + t * (latLngs[i+1][1] - latLngs[i][1])\n'
        '                    ]);\n'
        '                    break;\n'
        '                }\n'
        '                cum += lengths[i];\n'
        '                result.push(latLngs[i+1]);\n'
        '            }\n'
        '            return result;\n'
        '        }')
changes.append(('Fix getPlaybackOperationalYear and add progressive rendering helpers', old2, new2))

# ── CHANGE 3: Fix passesPlaybackFilter to show segments from construction start ─
old3 = ('        function passesPlaybackFilter(segment) {\n'
        '            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();\n'
        '            if (state.playbackYear >= currentYear) return true;\n'
        '            return segment.status === \'Open\' && getPlaybackOperationalYear(segment) <= state.playbackYear;\n'
        '        }')
new3 = ('        function passesPlaybackFilter(segment) {\n'
        '            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();\n'
        '            if (state.playbackYear >= currentYear) return true;\n'
        '            if (segment.status === \'Planned\') return false; // Never in historical mode\n'
        '            const startY = getSegmentConstructionStartYear(segment);\n'
        '            return startY <= state.playbackYear;\n'
        '        }')
changes.append(('Fix passesPlaybackFilter to show from construction start', old3, new3))

# ── CHANGE 4: Add cinematic year overlay CSS ──────────────────────────────────
old4 = '        .playback-meta {'
new4 = ('        .year-overlay {\n'
        '            position: absolute;\n'
        '            bottom: 90px;\n'
        '            left: 50%;\n'
        '            transform: translateX(-50%);\n'
        '            background: rgba(3, 8, 18, 0.82);\n'
        '            color: #fff;\n'
        '            font-size: clamp(42px, 8vw, 96px);\n'
        '            font-weight: 900;\n'
        '            letter-spacing: -3px;\n'
        '            line-height: 1;\n'
        '            padding: 8px 32px 6px;\n'
        '            border-radius: 14px;\n'
        '            pointer-events: none;\n'
        '            z-index: 1200;\n'
        '            display: none;\n'
        '            opacity: 0;\n'
        '            transition: opacity 0.35s ease;\n'
        '            font-variant-numeric: tabular-nums;\n'
        '            backdrop-filter: blur(12px) saturate(1.5);\n'
        '            -webkit-backdrop-filter: blur(12px) saturate(1.5);\n'
        '            border: 1px solid rgba(255,255,255,0.1);\n'
        '            text-shadow: 0 2px 24px rgba(0,0,0,.6);\n'
        '        }\n'
        '        .year-overlay.visible {\n'
        '            display: block;\n'
        '            opacity: 1;\n'
        '        }\n'
        '        .year-overlay .year-km-sub {\n'
        '            display: block;\n'
        '            font-size: 0.22em;\n'
        '            font-weight: 500;\n'
        '            letter-spacing: 0.5px;\n'
        '            opacity: 0.72;\n'
        '            margin-top: 2px;\n'
        '            text-align: center;\n'
        '        }\n'
        '        .playback-km-badge {\n'
        '            display: inline-flex;\n'
        '            align-items: center;\n'
        '            gap: 6px;\n'
        '            background: var(--surface-2);\n'
        '            border: 1px solid var(--border);\n'
        '            border-radius: 8px;\n'
        '            padding: 5px 12px;\n'
        '            font-size: 12px;\n'
        '            font-weight: 600;\n'
        '            color: var(--accent-2);\n'
        '            margin-top: 8px;\n'
        '            transition: all 0.3s;\n'
        '        }\n'
        '        .playback-meta {')
changes.append(('Add cinematic year overlay CSS', old4, new4))

# ── CHANGE 5: Add year-overlay DOM element in map ────────────────────────────
old5 = '                <div id="map"></div>'
new5 = '                <div id="map"></div>\n                <div class="year-overlay" id="year-overlay"><span id="year-overlay-year"></span><span class="year-km-sub" id="year-overlay-km"></span></div>'
changes.append(('Add year-overlay div to map DOM', old5, new5))

# ── CHANGE 6: Add el references for year overlay and km badge ────────────────
old6 = '            el.playbackRange = $('playback-range'),'
new6 = "            el.playbackRange = $('playback-range'), el.yearOverlay = $('year-overlay'), el.yearOverlayYear = $('year-overlay-year'), el.yearOverlayKm = $('year-overlay-km'),"
changes.append(('Add year-overlay el references', old6, new6))

# ── CHANGE 7: Faster playback interval (600ms instead of 900ms) + km display ─
old7 = '                    state.playbackYear += 1;\n                    clearFilteredSelection();\n                    render();\n                }, 900);'
new7 = '                    state.playbackYear += 1;\n                    clearFilteredSelection();\n                    render();\n                }, 600);'
changes.append(('Speed up playback to 600ms', old7, new7))

print(f"Loaded file: {len(content)} chars")
print(f"Applying {len(changes)} changes...\n")

for name, old, new in changes:
    if old in content:
        content = content.replace(old, new, 1)
        print(f"  ✓ {name}")
    else:
        print(f"  ✗ FAILED: {name}")
        # Show what's actually at that location
        key = old[:80]
        idx = content.find(key)
        if idx >= 0:
            print(f"    Partial match found at {idx}:")
            print(f"    {repr(content[idx:idx+200])}")
        else:
            print(f"    No partial match for: {repr(key)}")

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

print(f"\nDone. File is now {len(content)} chars")
