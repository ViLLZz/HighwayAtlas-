#!/usr/bin/env python3
"""
Targeted field-by-field replacement for A2-01..04 and A3-03.
Uses sectionCode anchor + regex to replace individual fields.
"""
import re
import sys

PATH = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs"

with open(PATH, encoding="utf-8") as f:
    content = f.read()

changes = 0

def find_section_block(code):
    marker = f'sectionCode: "{code}"'
    idx = content.find(marker)
    if idx < 0:
        return None, None
    start = content.rfind("CreateSegment(", 0, idx)
    depth = 0
    i = start
    while i < len(content):
        c = content[i]
        if c == '(':
            depth += 1
        elif c == ')':
            depth -= 1
            if depth == 0:
                return start, i + 1
        i += 1
    return start, None

def replace_in_block(code, old_fragment, new_fragment, label):
    """Replace old_fragment with new_fragment only within the block for this sectionCode."""
    global content, changes
    s, e = find_section_block(code)
    if s is None:
        print(f"  MISSING block: {code}")
        return
    block = content[s:e]
    if old_fragment not in block:
        print(f"  MISSING in {code}: {label}")
        print(f"    Seeking: {repr(old_fragment[:80])}")
        return
    new_block = block.replace(old_fragment, new_fragment, 1)
    content = content[:s] + new_block + content[e:]
    changes += 1
    print(f"  OK [{code}]: {label}")

# ==============================================================
# A2-01: Sofia → Boaza (43km, Open) → Sofia → Ugarchin (103km, Open)
# ==============================================================
# EN sectionName
replace_in_block("A2-01",
    '"Sofia – Boaza")',
    '"Sofia – Ugarchin (Dermantsi area)")',
    "sectionName EN"
)
# lengthKm
replace_in_block("A2-01",
    "lengthKm: 43,",
    "lengthKm: 103,",
    "lengthKm 43→103"
)
# startYear
replace_in_block("A2-01",
    "startYear: 2000,",
    "startYear: 1974,",
    "startYear 2000→1974"
)
# budgetMillionEur
replace_in_block("A2-01",
    "budgetMillionEur: 620,",
    "budgetMillionEur: 720,",
    "budget 620→720"
)
# shapePoints
replace_in_block("A2-01",
    '                    (42.6977, 23.3219, "Sofia"),\n'
    '                    (42.8170, 23.5170, "Botevgrad"),\n'
    '                    (42.9400, 23.9100, "Boaza pass")',
    '                    (42.6977, 23.3219, "Sofia ring road"),\n'
    '                    (42.9160, 23.7880, "Botevgrad"),\n'
    '                    (43.0210, 23.9040, "Pravets"),\n'
    '                    (43.0150, 24.0890, "Yablanitsa"),\n'
    '                    (43.1220, 24.4850, "Boaza pass (I-4 junction)"),\n'
    '                    (43.2010, 24.5370, "Dermantsi"),\n'
    '                    (43.1960, 24.4110, "Ugarchin (temporary exit)")',
    "shapePoints Sofia→Ugarchin"
)
# milestones  
replace_in_block("A2-01",
    '                    Milestone(2000, "ЕС ИСПА финансирането за Хемус е одобрено", "EU ISPA financing for Hemus approved"),\n'
    '                    Milestone(2007, "София – Боаза в пълна експлоатация", "Sofia–Boaza fully operational", "success")',
    '                    Milestone(1974, "Официален старт на строителство на Хемус", "Official groundbreaking of Hemus motorway"),\n'
    '                    Milestone(1999, "София – Ябланица открита (74.6 km)", "Sofia–Yablanitsa opened (74.6 km)", "success"),\n'
    '                    Milestone(2019, "Ябланица – Боаза Лот 0 (9.3 km) открит", "Yablanitsa–Boaza Lot 0 (9.3 km) opened", "success"),\n'
    '                    Milestone(2025, "Боаза–Дерманци (окт 2025) и Дерманци–Угърчин (дек 2025) отворени", "Boaza–Dermantsi (Oct 2025) and Dermantsi–Ugarchin (Dec 2025) opened", "success")',
    "milestones updated"
)

# ==============================================================
# A2-02: Boaza → Dermantsi (63km, Open) → Ugarchin → Letnitsa (69km, UC 25%)
# ==============================================================
replace_in_block("A2-02",
    '"Boaza – Dermantsi")',
    '"Ugarchin – Letnitsa / Krushuna")',
    "sectionName EN"
)
replace_in_block("A2-02",
    'status: SegmentStatus.Open,\n'
    '                lengthKm: 63,',
    'status: SegmentStatus.UnderConstruction,\n'
    '                lengthKm: 69,',
    "status Open→UC + length 63→69"
)
replace_in_block("A2-02",
    "startYear: 2015,",
    "startYear: 2021,",
    "startYear 2015→2021"
)
# Replace completionPercent 100 and add forecastOpenYear + contractor
replace_in_block("A2-02",
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway opening 2021",',
    '                forecastOpenYear: 2029,\n'
    '                completionPercent: 25,\n'
    '                contractor: "Trace Group / Infra Expert / PORR",\n'
    '                sourceName: "Wikipedia – Hemus motorway UC sections",',
    "completionPercent 100→25 + add forecastOpenYear/contractor"
)
replace_in_block("A2-02",
    '                    (42.9400, 23.9100, "Boaza pass"),\n'
    '                    (43.0800, 24.3200, "Lovech area"),\n'
    '                    (43.1800, 24.5500, "Dermantsi")',
    '                    (43.1960, 24.4110, "Ugarchin"),\n'
    '                    (43.3240, 24.6200, "Pleven / Lovech junction area"),\n'
    '                    (43.2900, 24.8700, "Letnitsa / Krushuna area")',
    "shapePoints Ugarchin→Letnitsa"
)
replace_in_block("A2-02",
    '                    Milestone(2015, "Строителство на Боаза–Дерманци стартира", "Construction of Boaza–Dermantsi begins"),\n'
    '                    Milestone(2021, "Боаза–Дерманци открити – критичен балкански участък завършен", "Boaza–Dermantsi opened – critical Balkan section completed", "success")',
    '                    Milestone(2021, "Договорите за Лотове 2–5 Угърчин–Летница са подписани", "Contracts for Lots 2–5 Ugarchin–Letnitsa signed"),\n'
    '                    Milestone(2023, "Активно строителство по всички лотове на участъка", "Active construction on all lots of the section", "info"),\n'
    '                    Milestone(2029, "Прогнозно отваряне Угърчин–Летница", "Forecast opening Ugarchin–Letnitsa", "warning")',
    "milestones updated"
)

# ==============================================================
# A2-03: Dermantsi → VT (114km, UC 32%) → Letnitsa → VT/Polikraishte (94km, Planned)
# ==============================================================
replace_in_block("A2-03",
    '"Dermantsi – Veliko Tarnovo")',
    '"Letnitsa – Polikraishte (VT junction)")',
    "sectionName EN"
)
replace_in_block("A2-03",
    'status: SegmentStatus.UnderConstruction,\n'
    '                lengthKm: 114,',
    'status: SegmentStatus.Planned,\n'
    '                lengthKm: 94,',
    "status UC→Planned + length 114→94"
)
replace_in_block("A2-03",
    "startYear: 2019,\n"
    "                forecastOpenYear: 2027,\n"
    "                completionPercent: 32,",
    "startYear: 2027,\n"
    "                forecastOpenYear: 2032,\n"
    "                completionPercent: 0,",
    "startYear/forecastOpenYear/completionPercent"
)
replace_in_block("A2-03",
    'contractor: "Trace Group / Buildinvest / PORR",',
    'contractor: "TBD",',
    "contractor update"
)
replace_in_block("A2-03",
    '                    (43.1800, 24.5500, "Dermantsi"),\n'
    '                    (43.3300, 24.9500, "Pleven area"),\n'
    '                    (43.2200, 25.4500, "Pavlikeni area"),\n'
    '                    (43.0770, 25.6173, "Veliko Tarnovo")',
    '                    (43.2900, 24.8700, "Letnitsa / Krushuna"),\n'
    '                    (43.2450, 25.3200, "Pavlikeni"),\n'
    '                    (43.3100, 25.6200, "Daskot / VT area"),\n'
    '                    (43.3600, 25.7200, "Polikraishte (VT junction)")',
    "shapePoints Letnitsa→Polikraishte"
)
replace_in_block("A2-03",
    '                    Milestone(2019, "Строителството на Дерманци–ВТ стартира на отделни лотове", "Construction of Dermantsi–VT begins in separate lots"),\n'
    '                    Milestone(2023, "Лот 1 е на 65% завършеност", "Lot 1 at 65% completion", "info"),\n'
    '                    Milestone(2027, "Прогнозно отваряне на целия участък", "Projected opening of full section", "warning")',
    '                    Milestone(2024, "Технически проекти за Летница–Поликраище в разработка", "Technical designs for Letnitsa–Polikraishte in development", "info"),\n'
    '                    Milestone(2027, "Очаквано начало на строителство", "Expected construction start", "warning"),\n'
    '                    Milestone(2032, "Прогнозно завършване Летница–Поликраище", "Forecast completion Letnitsa–Polikraishte", "warning")',
    "milestones updated"
)

# ==============================================================
# A2-04: VT → Shumen (125km, Planned) → Polikraishte → Buhovtsi (45km, Planned)
# ==============================================================
replace_in_block("A2-04",
    '"Veliko Tarnovo – Shumen")',
    '"Polikraishte – Buhovtsi (Targovishte)")',
    "sectionName EN"
)
replace_in_block("A2-04",
    "lengthKm: 125,",
    "lengthKm: 45,",
    "length 125→45"
)
replace_in_block("A2-04",
    "forecastOpenYear: 2032,\n"
    "                budgetMillionEur: 520,",
    "forecastOpenYear: 2033,\n"
    "                completionPercent: 10,\n"
    "                budgetMillionEur: 350,",
    "forecastOpenYear 2032→2033 + add completionPercent + budget 520→350"
)
replace_in_block("A2-04",
    '                    (43.0770, 25.6173, "Veliko Tarnovo"),\n'
    '                    (43.2560, 26.2340, "Popovo"),\n'
    '                    (43.2720, 26.9236, "Shumen")',
    '                    (43.3600, 25.7200, "Polikraishte (VT junction)"),\n'
    '                    (43.3700, 26.2000, "Popovo area"),\n'
    '                    (43.3700, 26.5400, "Loznitsa (UC lot)"),\n'
    '                    (43.1930, 26.5680, "Buhovtsi / Targovishte")',
    "shapePoints Polikraishte→Buhovtsi"
)
replace_in_block("A2-04",
    '                    Milestone(2022, "ОВОС процедурата е стартирана", "EIA procedure started"),\n'
    '                    Milestone(2025, "Очаквано начало на строителство", "Expected construction start", "warning")',
    '                    Milestone(2023, "Лозница–Буховци: строителство започнато", "Loznitsa–Buhovtsi: construction started", "info"),\n'
    '                    Milestone(2025, "Тръжни процедури за Поликраище–Лозница", "Tender procedures for Polikraishte–Loznitsa", "warning"),\n'
    '                    Milestone(2033, "Прогнозно завършване – Поликраище–Буховци", "Forecast completion – Polikraishte–Buhovtsi", "warning")',
    "milestones updated"
)

# ==============================================================
# A3-03: Sandanski → Kulata (29km, Planned) → Kresna → Kulata (41km, Open)
# Note: shapePoints and milestones already fixed by earlier script
# Need: status, length, startYear, completionPercent, budget, remove forecastOpenYear
# ==============================================================
replace_in_block("A3-03",
    '"Sandanski – Kulata (border)")',
    '"Kresna – Kulata (Lots 3.3 + 4)")',
    "sectionName EN"
)
replace_in_block("A3-03",
    'status: SegmentStatus.Planned,\n'
    '                lengthKm: 29,',
    'status: SegmentStatus.Open,\n'
    '                lengthKm: 41,',
    "status Planned→Open + length 29→41"
)
replace_in_block("A3-03",
    "startYear: 2028,\n"
    "                forecastOpenYear: 2030,",
    "startYear: 2013,\n"
    "                completionPercent: 100,",
    "startYear 2028→2013; remove forecastOpenYear; add completionPercent 100"
)
replace_in_block("A3-03",
    "budgetMillionEur: 95,\n"
    "                fundingProgram: \"National budget\",\n"
    "                contractor: \"TBD\",",
    "budgetMillionEur: 290,\n"
    "                fundingProgram: \"EU Cohesion Fund + national\",",
    "budget 95→290, fundingProgram, remove contractor"
)

# ==============================================================
# A2-01: Update description (EN part only for reliability)
# ==============================================================
replace_in_block("A2-01",
    '"The western completed section of Hemus motorway through Yablanitsa to the Boaza pass in the Balkan range. ISPA 2000–2007 funded.")',
    '"The completed western section of Hemus from the Sofia ring road to the temporary exit at Ugarchin. Built in phases 1974–2025: Sofia–Yablanitsa (1999), Yablanitsa–Boaza (2019), Boaza–Dermantsi (October 2025), Dermantsi–Ugarchin (December 2025).")',
    "description EN"
)

# A2-02: Update description EN
replace_in_block("A2-02",
    '"The central section from Boaza to Dermantsi (near Pleven), opened 2 November 2021. Co-financed by the EU Cohesion Fund.")',
    '"Under active construction from Ugarchin to Letnitsa/Krushuna (Lots 2–5). Works continue in the Pleven, Lovech and Teteven areas. EU Cohesion Fund financing.")',
    "description EN"
)

# A2-03: Update description EN
replace_in_block("A2-03",
    '"The uncompleted central-northern section of Hemus. Construction continues in separate lots; forecast completion 2027.")',
    '"The planned central section of Hemus from Letnitsa to the Veliko Tarnovo junction at Polikraishte. The route passes north of VT through the Danubian plain. Tender procedures for the upcoming lots are being prepared with EU Cohesion Fund financing.")',
    "description EN"
)

# A2-04: Update description EN
replace_in_block("A2-04",
    '"The planned eastern section of Hemus from Veliko Tarnovo to Shumen. Tender procedures being prepared; EU CF and CEF financing under negotiation.")',
    '"The last unbuilt gap on Hemus before the completed Buhovtsi–Varna section. The Loznitsa–Buhovtsi lot (12 km) is under active construction; the remaining Polikraishte–Loznitsa (33 km) is planned.")',
    "description EN"
)

# A3-03: Update description EN  
replace_in_block("A3-03",
    '"The final planned section of Struma from Sandanski to the Kulata border crossing with Greece.")',
    '"The completed southern section of Struma from Kresna to the Kulata border crossing. Lot 4 (Sandanski–Kulata) opened 20 August 2015; Lot 3.3 (Kresna–Sandanski) completed 17 December 2018.")',
    "description EN"
)

# ==============================================================
print(f"\nTotal changes: {changes}")

with open(PATH, "w", encoding="utf-8") as f:
    f.write(content)
print("File written.")

# Verify
for code in ["A2-01", "A2-02", "A2-03", "A2-04", "A3-03"]:
    import re as _re
    marker = f'sectionCode: "{code}"'
    idx = content.find(marker)
    if idx < 0:
        print(f"{code}: NOT FOUND")
        continue
    start = content.rfind("CreateSegment(", 0, idx)
    depth = 0
    i = start
    while i < len(content):
        c = content[i]
        if c == '(':
            depth += 1
        elif c == ')':
            depth -= 1
            if depth == 0:
                block = content[start:i+1]
                m_len = _re.search(r'lengthKm: (\d+)', block)
                m_stat = _re.search(r'status: SegmentStatus\.(\w+)', block)
                m_name = _re.search(r'sectionName: Text\("([^"]+)", "([^"]+)"\)', block)
                print(f"{code}: len={m_len.group(1) if m_len else '?'}  status={m_stat.group(1) if m_stat else '?'}  EN={m_name.group(2) if m_name else '?'}")
                break
        i += 1
