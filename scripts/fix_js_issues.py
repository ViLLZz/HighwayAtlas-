#!/usr/bin/env python3
"""Fix JS issues in HtmlAtlasExporter.cs:
1. Add baseline/planning/forecast to stateClass/stateIcon/stateLabel in renderTimeline
2. Fix dark funding badge colors to be readable on dark theme
3. Improve hero title/subtitle strings
"""

filepath = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/HtmlAtlasExporter.cs"

with open(filepath, encoding='utf-8') as f:
    content = f.read()

changes = []

# ── Fix 1: renderTimeline stateClass/stateIcon/stateLabel ──────────────────
old_render_tl = (
    "            const stateClass = { success: 'success', warning: 'warning', danger: 'danger', info: 'info', delayed: 'warning', cancelled: 'danger' };\n"
    "            const stateIcon  = { success: '\u2713', warning: '\u23f3', danger: '\u2717', info: '\u2139', delayed: '\u23f3', cancelled: '\u2717' };\n"
    "            const stateLabel = { success: 'Opened', warning: 'Delayed', danger: 'Issue', info: 'Update', delayed: 'Delayed', cancelled: 'Cancelled' };"
)
new_render_tl = (
    "            const stateClass = { success: 'success', warning: 'warning', danger: 'danger', info: 'info', delayed: 'warning', cancelled: 'danger', baseline: 'info', planning: 'info', forecast: 'info' };\n"
    "            const stateIcon  = { success: '\u2713', warning: '\u23f3', danger: '\u2717', info: '\u2139', delayed: '\u23f3', cancelled: '\u2717', baseline: '\u2139', planning: '\U0001F4CB', forecast: '\u2192' };\n"
    "            const stateLabel = { success: 'Opened', warning: 'Delayed', danger: 'Issue', info: 'Update', delayed: 'Delayed', cancelled: 'Cancelled', baseline: 'Baseline', planning: 'Planning', forecast: 'Forecast' };"
)
if old_render_tl in content:
    content = content.replace(old_render_tl, new_render_tl)
    changes.append("renderTimeline stateClass/Icon/Label extended")
else:
    changes.append("FAIL: renderTimeline stateClass not found as expected")

# ── Fix 2: funding badge colors (3 occurrences of the palette, but likely 1) ──
old_funding = "const palette = [['CEF','#0057a8'],['ISPA','#006a9c'],['Cohesion','#005f8a'],['EIB','#004494'],['EBRD','#0072bc'],['national','#5a6b55']];"
new_funding = "const palette = [['CEF','#4aabff'],['ISPA','#5db8ff'],['Cohesion','#82c5ff'],['EIB','#a3b9ff'],['EBRD','#72d5f5'],['national','#7dc46a']];"

if old_funding in content:
    count_f = content.count(old_funding)
    content = content.replace(old_funding, new_funding)
    changes.append(f"funding badge colors fixed ({count_f} occurrences)")
else:
    # Try to find it with different spacing
    import re
    m = re.search(r"const palette = \[\[.CEF...#\w+", content)
    if m:
        changes.append(f"FAIL: funding palette found but different format at pos {m.start()}: {m.group()}")
    else:
        changes.append("FAIL: funding palette not found")

# ── Fix 3: Hero title/subtitle – Bulgarian ───────────────────────────────
old_hero_bg_title = "heroTitle: 'Bulgaria in a tighter, contained map frame',"
new_hero_bg_title = "heroTitle: '\u0410\u0442\u043b\u0430\u0441 \u043d\u0430 \u0430\u0432\u0442\u043e\u043c\u0430\u0433\u0438\u0441\u0442\u0440\u0430\u043b\u0438\u0442\u0435 \u2013 \u0430\u043a\u0442\u0443\u0430\u043b\u0435\u043d \u0438\u043d\u0444\u043e\u0433\u0440\u0430\u0444\u0441\u043a\u0438 \u043f\u0440\u0435\u0433\u043b\u0435\u0434',"

# Bg block: heroTitle and heroSubtitle appear in the bg text object
old_hero_bg = (
    "                heroTitle: 'България в ясен, ограничен картографски кадър',\n"
    "                heroSubtitle: 'Картата вече стои като самостоятелен панел, заключена около България, без да превзема цялата страница.',"
)
new_hero_bg = (
    "                heroTitle: '\u0410\u0442\u043b\u0430\u0441 \u043d\u0430 \u0431\u044a\u043b\u0433\u0430\u0440\u0441\u043a\u0438\u0442\u0435 \u0430\u0432\u0442\u043e\u043c\u0430\u0433\u0438\u0441\u0442\u0440\u0430\u043b\u0438 \u2013 \u0430\u043a\u0442\u0443\u0430\u043b\u0435\u043d \u0438\u043d\u0444\u043e\u0433\u0440\u0430\u0444\u0441\u043a\u0438 \u043f\u0440\u0435\u0433\u043b\u0435\u0434',\n"
    "                heroSubtitle: '9 \u043a\u043b\u044e\u0447\u043e\u0432\u0438 \u043a\u043e\u0440\u0438\u0434\u043e\u0440\u0430 \u00b7 \u043e\u0442 \u0421\u043e\u0444\u0438\u044f \u0434\u043e \u0427\u0435\u0440\u043d\u043e \u043c\u043e\u0440\u0435 \u00b7 1\u202f506 km \u0434\u043e\u043a\u0443\u043c\u0435\u043d\u0442\u0438\u0440\u0430\u043d\u0438',"
)

if old_hero_bg in content:
    content = content.replace(old_hero_bg, new_hero_bg)
    changes.append("hero bg title/subtitle updated")
else:
    # Try individual
    old_bg_title_line = "                heroTitle: '\u0411\u044a\u043b\u0433\u0430\u0440\u0438\u044f \u0432 \u044f\u0441\u0435\u043d, \u043e\u0433\u0440\u0430\u043d\u0438\u0447\u0435\u043d \u043a\u0430\u0440\u0442\u043e\u0433\u0440\u0430\u0444\u0441\u043a\u0438 \u043a\u0430\u0434\u044a\u0440',"
    if old_bg_title_line in content:
        new_bg_title_line = "                heroTitle: '\u0410\u0442\u043b\u0430\u0441 \u043d\u0430 \u0431\u044a\u043b\u0433\u0430\u0440\u0441\u043a\u0438\u0442\u0435 \u0430\u0432\u0442\u043e\u043c\u0430\u0433\u0438\u0441\u0442\u0440\u0430\u043b\u0438 \u2013 \u0430\u043a\u0442\u0443\u0430\u043b\u0435\u043d \u0438\u043d\u0444\u043e\u0433\u0440\u0430\u0444\u0441\u043a\u0438 \u043f\u0440\u0435\u0433\u043b\u0435\u0434',"
        content = content.replace(old_bg_title_line, new_bg_title_line)
        changes.append("hero bg title updated (individual)")
    else:
        changes.append("FAIL: hero bg title not found")
    
    old_bg_sub_line = "                heroSubtitle: '\u041a\u0430\u0440\u0442\u0430\u0442\u0430 \u0432\u0435\u0447\u0435 \u0441\u0442\u043e\u0438 \u043a\u0430\u0442\u043e \u0441\u0430\u043c\u043e\u0441\u0442\u043e\u044f\u0442\u0435\u043b\u0435\u043d \u043f\u0430\u043d\u0435\u043b, \u0437\u0430\u043a\u043b\u044e\u0447\u0435\u043d\u0430 \u043e\u043a\u043e\u043b\u043e \u0411\u044a\u043b\u0433\u0430\u0440\u0438\u044f, \u0431\u0435\u0437 \u0434\u0430 \u043f\u0440\u0435\u0432\u0437\u0435\u043c\u0430 \u0446\u044f\u043b\u0430\u0442\u0430 \u0441\u0442\u0440\u0430\u043d\u0438\u0446\u0430.',"
    if old_bg_sub_line in content:
        new_bg_sub_line = "                heroSubtitle: '9 \u043a\u043b\u044e\u0447\u043e\u0432\u0438 \u043a\u043e\u0440\u0438\u0434\u043e\u0440\u0430 \u00b7 \u043e\u0442 \u0421\u043e\u0444\u0438\u044f \u0434\u043e \u0427\u0435\u0440\u043d\u043e \u043c\u043e\u0440\u0435 \u00b7 1\u202f506 km \u0434\u043e\u043a\u0443\u043c\u0435\u043d\u0442\u0438\u0440\u0430\u043d\u0438',"
        content = content.replace(old_bg_sub_line, new_bg_sub_line)
        changes.append("hero bg subtitle updated (individual)")
    else:
        changes.append("FAIL: hero bg subtitle not found")

# ── Fix 4: Hero title/subtitle – English ─────────────────────────────────
old_hero_en = (
    "                heroTitle: 'Bulgaria in a tighter, contained map frame',\n"
    "                heroSubtitle: 'The map now behaves like a focused dashboard panel, locked around Bulgaria instead of taking over the full page.',"
)
new_hero_en = (
    "                heroTitle: 'Bulgarian Motorway Atlas \u2013 live infrastructure snapshot',\n"
    "                heroSubtitle: '9 key corridors \u00b7 Sofia to Black Sea \u00b7 1,506 km documented',"
)
if old_hero_en in content:
    content = content.replace(old_hero_en, new_hero_en)
    changes.append("hero en title/subtitle updated")
else:
    old_en_title = "                heroTitle: 'Bulgaria in a tighter, contained map frame',"
    old_en_sub = "                heroSubtitle: 'The map now behaves like a focused dashboard panel, locked around Bulgaria instead of taking over the full page.',"
    found_any = False
    if old_en_title in content:
        content = content.replace(old_en_title, "                heroTitle: 'Bulgarian Motorway Atlas \u2013 live infrastructure snapshot',")
        changes.append("hero en title updated (individual)")
        found_any = True
    if old_en_sub in content:
        content = content.replace(old_en_sub, "                heroSubtitle: '9 key corridors \u00b7 Sofia to Black Sea \u00b7 1,506 km documented',")
        changes.append("hero en subtitle updated (individual)")
        found_any = True
    if not found_any:
        changes.append("FAIL: hero en title/subtitle not found")

# ── Write back ─────────────────────────────────────────────────────────────
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

# ── Report ─────────────────────────────────────────────────────────────────
print("Changes applied:")
for c in changes:
    print(f"  {'OK' if not c.startswith('FAIL') else '!!'}: {c}")
