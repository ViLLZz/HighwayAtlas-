#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Fix stale comments in NetworkData.cs for A2 segments"""

fp = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs"
with open(fp, encoding='utf-8') as f: c = f.read()

fixes = [
    ("// A2-01: Sofia → Boaza (43 km) – Highway Hemus, western section, Open",
     "// A2-01: Sofia → Ugarchin (103 km) – Hemus opened in phases 1974–2025, fully open Dec 2025"),
    ("// A2-02: Boaza → Dermantsi (63 km) – Open 2021",
     "// A2-02: Ugarchin → Letnitsa (69 km) – Under Construction 25%, forecast 2029"),
    ("// A2-03: Dermantsi → Veliko Tarnovo (114 km) – UC 32%",
     "// A2-03: Letnitsa → Polikraishte (94 km) – Planned, design phase 2027, forecast 2032"),
    ("// A2-04: Veliko Tarnovo → Shumen (125 km) – Planned",
     "// A2-04: Polikraishte → Buhovtsi (45 km) – Planned/UC; Loznitsa–Buhovtsi 12km active"),
    ("// A2-05: Shumen → Varna (73 km) – Open",
     "// A2-05: Buhovtsi → Varna (107 km) – Open, includes 1974 historic axis + 2013–2022 extensions"),
]

changes = 0
for old, new in fixes:
    if old in c:
        c = c.replace(old, new)
        print(f"OK: {new[:60]}...")
        changes += 1
    else:
        print(f"??: not found: {old[:60]}...")

with open(fp, 'w', encoding='utf-8') as f: f.write(c)
print(f"\nDone: {changes}/{len(fixes)} comments updated.")
