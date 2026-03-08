#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Fix network description strings and Bulgarian hero text in HtmlAtlasExporter.cs"""

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

# Network subtitle BG
fix("network subtitle bg",
    'subtitle = new { bg = "Обновеният интерфейс поставя картата в самостоятелен, контролиран панел с по-ясна йерархия между маршрути, лотове и данни.",',
    'subtitle = new { bg = "Актуален преглед на 9 маршрута, 20 участъка и над 50 лота – от първите открития до днешните строежи.",')

# Network subtitle EN
fix("network subtitle en",
    'en = "The refreshed interface moves the map into a contained dashboard panel with clearer hierarchy between routes, lots, and data." },',
    'en = "Live snapshot of 9 routes, 20 sections, 50+ lots – from first openings to today\'s active construction." },')

# Network message BG
fix("network message bg",
    'message = new { bg = "Картата е заключена около България и вече работи като инфографски модул, а не като пълноекранен фон.",',
    'message = new { bg = "Мрежата обхваща 1 506 km; 991 km са отворени, 250 km в строеж, 265 km планирани.",')

# Network message EN
fix("network message en",
    'en = "The map is locked around Bulgaria and now behaves like an infographic module rather than a full-page background." }',
    'en = "The network spans 1,506 km: 991 km open, 250 km under construction, 265 km planned." }')

# bulgariaScopeNote EN
fix("bulgariaScopeNote en",
    "bulgariaScopeNote: 'The map viewport is constrained around Bulgaria and acts as an infographic panel rather than a full-page canvas.',",
    "bulgariaScopeNote: 'The network covers 9 routes (A1–A6, E79, RVT, MB) – 1,506 km total, of which 991 km are open.',")

# bulgariaScopeNote BG
fix("bulgariaScopeNote bg",
    "bulgariaScopeNote: 'Картният прозорец е ограничен около територията на България и служи като инфографски панел, а не като фоново платно.',",
    "bulgariaScopeNote: 'Мрежата обхваща 9 маршрута (А1–А6, E79, RVT, MB) – общо 1 506 km, от които 991 km са отворени.',")

with open(fp, 'w', encoding='utf-8') as f: f.write(c)

for item in changes: print(f"OK : {item}")
for item in fails: print(f"?? : {item}")
print(f"Done: {len(changes)} applied, {len(fails)} not found.")
