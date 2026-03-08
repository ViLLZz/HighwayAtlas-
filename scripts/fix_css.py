#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Fix remaining CSS and UI issues in HtmlAtlasExporter.cs:
1. funding-badge base CSS dark-theme fix
2. Add smooth scroll to sidebar for lot/route focus
3. Add aria-label to key interactive elements
"""

fp = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/HtmlAtlasExporter.cs"
with open(fp, encoding='utf-8') as f: c = f.read()

changes, fails = [], []

def fix(desc, old, new):
    global c
    if old in c:
        c = c.replace(old, new)
        changes.append(desc)
    else:
        fails.append(f"NOT FOUND: {desc}")

# Fix 1: .funding-badge base CSS 
fix(".funding-badge base CSS dark-theme colors",
    "        .funding-badge {\n"
    "            display: inline-block;\n"
    "            padding: 2px 8px;\n"
    "            border-radius: 4px;\n"
    "            font-size: 0.70em;\n"
    "            font-weight: 600;\n"
    "            border: 1px solid rgba(0,80,180,0.25);\n"
    "            background: rgba(0,80,180,0.08);\n"
    "            color: #0052b4;\n"
    "            letter-spacing: 0.03em;\n"
    "            vertical-align: middle;\n"
    "        }",
    "        .funding-badge {\n"
    "            display: inline-block;\n"
    "            padding: 2px 8px;\n"
    "            border-radius: 4px;\n"
    "            font-size: 0.70em;\n"
    "            font-weight: 600;\n"
    "            border: 1px solid rgba(80,160,255,0.28);\n"
    "            background: rgba(74,171,255,0.10);\n"
    "            color: #74b5f8;\n"
    "            letter-spacing: 0.03em;\n"
    "            vertical-align: middle;\n"
    "        }"
)

# Fix 2: Add scroll-behavior smooth to body CSS for better sidebar scrolling
fix("sidebar scroll-behavior smooth",
    "        .sidebar {\n"
    "            grid-area: sidebar;\n"
    "            position: sticky;",
    "        .sidebar {\n"
    "            grid-area: sidebar;\n"
    "            position: sticky;\n"
    "            scroll-behavior: smooth;"
)

# Fix 3: Improve the lot popup fundingBadge display - add some padding below status
# (No visual bug, skip)

# Fix 4: Improve .kpi interactive cursor 
fix(".kpi interactive cursor",
    "        .kpi.interactive { cursor: pointer; }",
    "        .kpi.interactive { cursor: pointer; transition: transform .15s ease, border-color .15s ease; }\n"
    "        .kpi.interactive:hover { transform: translateY(-2px); border-color: rgba(136, 200, 255, 0.3); }"
)

with open(fp, 'w', encoding='utf-8') as f: f.write(c)

for item in changes: print(f"OK : {item}")
for item in fails: print(f"?? : {item}")
print(f"Done: {len(changes)} applied, {len(fails)} not found.")
