"""
Full NetworkData.cs editor — applies all planned changes:
1. Split A3-01 (Sofia-Sandanski 143km) into A3-01 (Sofia-Simitli 107km Open) + A3-02 (Kresna 35km UC)
2. Rename old A3-02 (Sandanski-Kulata) -> A3-03, fix Sandanski coords
3. Add budgetMillionEur + fundingProgram to all remaining segments
4. Add forecastOpenYear + contractor on UC segments
"""

import re

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

print(f"File loaded: {len(content)} chars")

# ─────────────────────────────────────────────────────────────────────────────
# HELPER: find a block starting with a known anchor and ending before the next
# block in the stream
# ─────────────────────────────────────────────────────────────────────────────
def find_segment_block(content, section_code):
    """Return (start, end) of the CreateSegment(…) call for a given sectionCode."""
    anchor = f'sectionCode: "{section_code}"'
    idx = content.find(anchor)
    if idx < 0:
        raise ValueError(f"anchor not found: {anchor}")
    # walk backwards to find the CreateSegment( or the // comment line
    seg_start = content.rfind('\n            // ', 0, idx)
    if seg_start < 0:
        seg_start = content.rfind('\n            CreateSegment(', 0, idx)
    # walk forwards past the milestones ]); to find the closing
    # The block ends at ']);' that closes the CreateSegment call
    depth = 0
    i = idx
    while i < len(content):
        ch = content[i]
        if ch == '(':
            depth += 1
        elif ch == ')':
            depth -= 1
            if depth <= 0:
                # find the closing ';' after ')'
                end = content.index(';', i)
                return (seg_start + 1, end + 1)
        i += 1
    raise ValueError(f"Could not find end of block for {section_code}")


# ─────────────────────────────────────────────────────────────────────────────
# 1. A3-01 split
# ─────────────────────────────────────────────────────────────────────────────
start, end = find_segment_block(content, 'A3-01')
old_block = content[start:end]
print(f"\nA3-01 block found: chars {start}–{end} ({end-start} chars)")
print("First 120:", repr(old_block[:120]))

new_a3_blocks = r"""// A3-01: Sofia → Simitli (107 km) – completed Struma corridor
            // Built in phases 2002–2016; all main lots open.
            CreateSegment(
                routeCode: "A3",
                name: "Struma Motorway",
                sectionCode: "A3-01",
                displayName: Text("Автомагистрала „Струма"", "Struma Motorway"),
                sectionName: Text("София – Симитли", "Sofia – Simitli"),
                description: Text("Завършеният участък по долината на Струма от Перник до Симитли. Основните лотове се пускат поетапно 2009–2016 г. с ИСПА и КФ на ЕС.", "The completed section along the Struma valley from Pernik to Simitli. Major lots opened 2009–2016 with EU ISPA and Cohesion Fund co-financing."),
                importance: Text("Ключова артерия към Благоевград, Югозападен университет и граничния преход. Международен транзитен коридор IV.", "Key artery to Blagoevgrad, South-West University, and the border crossing. International Transit Corridor IV."),
                status: SegmentStatus.Open,
                lengthKm: 107,
                maxSpeedKph: 140,
                startYear: 2002,
                completionPercent: 100,
                budgetMillionEur: 940,
                fundingProgram: "EU ISPA + Cohesion Fund",
                sourceName: "API Bulgaria / EC INEA project register – A3 Struma",
                sourceUrl: "https://en.wikipedia.org/wiki/Struma_motorway",
                shapePoints:
                [
                    (42.6977, 23.3219, "Sofia"),
                    (42.6050, 23.0380, "Pernik"),
                    (42.3700, 23.0100, "Bobov Dol"),
                    (42.2660, 23.1160, "Dupnitsa"),
                    (42.0200, 23.1000, "Blagoevgrad"),
                    (41.8950, 23.0900, "Simitli")
                ],
                milestones:
                [
                    Milestone(2002, "Тръжна процедура за Лот 1 (Даскалово – Дупница) е открита", "Tender for Lot 1 (Daskalovo–Dupnitsa) launched"),
                    Milestone(2009, "Лот 3 (Благоевград байпас) е открит", "Lot 3 Blagoevgrad bypass opened", "success"),
                    Milestone(2013, "Дупница – Благоевград в пълна експлоатация", "Dupnitsa–Blagoevgrad fully operational", "success"),
                    Milestone(2015, "Лот 1 в посока Перник – Даскалово открит", "Lot 1 (Pernik–Daskalovo) opened", "success"),
                    Milestone(2016, "А-3-01 в пълна експлоатация – София – Симитли", "A3-01 fully open – Sofia to Simitli", "success")
                ]),

            // A3-02: Симитли → Сандански през Кресненско дефиле (35 km) – В СТРОЕЖ
            // Най-спорният строителен обект в историята на българската пътна инфраструктура.
            CreateSegment(
                routeCode: "A3",
                name: "Struma Motorway",
                sectionCode: "A3-02",
                displayName: Text("Автомагистрала „Струма"", "Struma Motorway"),
                sectionName: Text("Симитли – Сандански (Кресненско дефиле)", "Simitli – Sandanski (Kresna Gorge)"),
                description: Text(
                    "Кресненското дефиле е най-спорният строителен участък в историята на българската пътна инфраструктура. Едновременно е природозащитен обект и единственият технически коридор за магистралата.",
                    "The Kresna gorge is the most contested construction site in Bulgarian infrastructure history. It is simultaneously a protected natural area and the only viable motorway corridor."),
                importance: Text(
                    "Трансевропейски коридор IV (TEN-T). Без него целият коридор Сърбия–България–Гърция застава претоварен алтернативен път.",
                    "Trans-European Corridor IV (TEN-T). Without it, the entire Serbia–Bulgaria–Greece corridor remains on congested alternative routes."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 35,
                maxSpeedKph: 140,
                startYear: 2019,
                forecastOpenYear: 2028,
                completionPercent: 34,
                budgetMillionEur: 460,
                fundingProgram: "EU Cohesion Fund + national",
                contractor: "Trace Group / Strabag JV",
                sourceName: "МРРБ / API Bulgaria – Struma Lot 3.2 / 3.3",
                sourceUrl: "https://en.wikipedia.org/wiki/Struma_motorway",
                shapePoints:
                [
                    (41.8950, 23.0900, "Simitli"),
                    (41.7900, 23.1100, "Kresna gorge north"),
                    (41.7050, 23.1500, "Kresna"),
                    (41.5639, 23.2761, "Sandanski")
                ],
                milestones:
                [
                    Milestone(1999, "Европейски протести срещу трасето по дефилето", "European objections to the route through the gorge", "warning"),
                    Milestone(2017, "ОВОС одобрен след ревизия; ново трасе в дефилето", "EIA approved after revision; new alignment through the gorge", "info"),
                    Milestone(2019, "Строителството на Лот 3.2/3.3 (Кресненско дефиле) стартира", "Construction of Lot 3.2/3.3 (Kresna gorge) begins"),
                    Milestone(2028, "Прогнозна дата за цялостно завършване", "Forecast completion date", "warning")
                ]),"""

content = content[:start] + new_a3_blocks + '\n' + content[end:]
print("A3 split applied.")

# ─────────────────────────────────────────────────────────────────────────────
# 2. Rename A3-02 (Sandanski-Kulata) -> A3-03 + fix Sandanski coords
# ─────────────────────────────────────────────────────────────────────────────
# After the replacement there will be TWO A3-02 entries temporarily.
# We already inserted the new A3-02 above. Now find the OLD A3-02 (Kulata section).
# The OLD A3-02 has Sandanski as start point. Let's find it by looking for A3-02 with Kulata.
# After split, content will have new A3-02 code (Kresna) and then the old one.
# The old one should have Sandanski coords (41.7450, 23.2830) which we want to fix to (41.5639, 23.2761).

# Find the SECOND occurrence of sectionCode: "A3-02" (the old Sandanski-Kulata block)
first_a302 = content.find('sectionCode: "A3-02"')
second_a302 = content.find('sectionCode: "A3-02"', first_a302 + 1)

if second_a302 >= 0:
    # Replace the sectionCode in that vicinity
    content = content[:second_a302] + 'sectionCode: "A3-03"' + content[second_a302 + len('sectionCode: "A3-02"'):]
    print("A3-02 → A3-03 rename applied.")
    # Fix old Sandanski coordinates (41.7450, 23.2830) -> (41.5639, 23.2761)
    old_coord = '(41.7450, 23.2830, "Sandanski")'
    new_coord = '(41.5639, 23.2761, "Sandanski")'
    if old_coord in content:
        content = content.replace(old_coord, new_coord)
        print("Sandanski coords fixed in A3-03.")
    else:
        print("WARNING: Sandanski old coords not found in A3-03 block")
else:
    print("No second A3-02 found - old Kulata block may have different code")
    # Show current codes
    codes = re.findall(r'sectionCode: "([^"]+)"', content)
    print("Current codes:", codes)

# ─────────────────────────────────────────────────────────────────────────────
# 3. Add budgetMillionEur + fundingProgram to segments that lack them
#    Strategy: insert after "startYear: XXXX," line (or after completionPercent if present)
# ─────────────────────────────────────────────────────────────────────────────

BUDGET_DATA = {
    'A1-01': (185,  'EBRD + national',                    None,           None),
    'A1-02': (210,  'EU ISPA + EIB',                      None,           None),
    'A1-03': (85,   'EIB loan',                            None,           None),
    'A1-04': (145,  'EU Cohesion Fund',                    None,           None),
    'A1-05': (98,   'EU Cohesion Fund',                    None,           None),
    'A2-01': (620,  'EU Cohesion Fund + ISPA + national',  None,           None),
    'A2-02': (480,  'EU Cohesion Fund + national',          None,           None),
    'A2-03': (680,  'EU Cohesion Fund',                    'Trace Group / Buildinvest / PORR', 2027),
    'A2-04': (520,  'EU Cohesion Fund + CEF',              'TBD',          2032),
    'A2-05': (110,  'National budget + EIB',                None,           None),
    'A3-03': (95,   'National budget',                     'TBD',          2030),
    'A4-01': (380,  'EU ISPA + Cohesion Fund',             None,           None),
    'A5-01': (420,  'National budget',                     'TBD',          2035),
    'A6-01': (340,  'CEF + national',                      None,           None),
    'E79-01': (195, 'CEF + national',                      None,           None),
    'E79-02': (380, 'CEF + national',                     'GP Group / Geomash',  2027),
    'RVT-01': (695, 'CEF + EU Cohesion Fund',              'Trace Group / Geomash JV', 2028),
    'MB-01':  (185, 'National budget',                     None,           2026),
}

def add_fields_after_anchor(content, section_code, anchor_field, extra_lines):
    """Insert extra_lines after the first line matching anchor_field within the given section block."""
    code_idx = content.find(f'sectionCode: "{section_code}"')
    if code_idx < 0:
        return content, False
    # Find anchor within this block (search forward ~2000 chars)
    block = content[code_idx:code_idx + 3000]
    anchor_pos = block.find(anchor_field)
    if anchor_pos < 0:
        return content, False
    # Find end of that anchor line
    line_end = block.find('\n', anchor_pos)
    if line_end < 0:
        return content, False
    abs_line_end = code_idx + line_end
    content = content[:abs_line_end + 1] + extra_lines + content[abs_line_end + 1:]
    return content, True

def already_has(content, section_code, field):
    code_idx = content.find(f'sectionCode: "{section_code}"')
    if code_idx < 0:
        return False
    block = content[code_idx:code_idx + 3000]
    return field in block

for code, (budget, funding, contractor, forecast) in BUDGET_DATA.items():
    # Skip A3-01 and A3-02 – they were created with full data already
    if code in ('A3-01', 'A3-02'):
        continue

    extra = ''
    if not already_has(content, code, 'budgetMillionEur'):
        extra += f'                budgetMillionEur: {budget},\n'
    if not already_has(content, code, 'fundingProgram'):
        extra += f'                fundingProgram: "{funding}",\n'
    if contractor and not already_has(content, code, 'contractor:'):
        extra += f'                contractor: "{contractor}",\n'
    if forecast and not already_has(content, code, 'forecastOpenYear'):
        extra += f'                forecastOpenYear: {forecast},\n'

    if extra:
        # Insert after startYear line
        content, ok = add_fields_after_anchor(content, code, 'startYear:', extra)
        if ok:
            print(f"  Added to {code}: budget={budget}, funding={funding}" +
                  (f", contractor={contractor}" if contractor else '') +
                  (f", forecast={forecast}" if forecast else ''))
        else:
            print(f"  WARN: could not find startYear for {code}")
    else:
        print(f"  {code}: all fields already present, skipped")

# ─────────────────────────────────────────────────────────────────────────────
# Save
# ─────────────────────────────────────────────────────────────────────────────
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

print(f"\nDone. Saved {len(content)} chars.")
codes_after = re.findall(r'sectionCode: "([^"]+)"', content)
print("Final section codes:", codes_after)
print("Total segments:", len(codes_after))
