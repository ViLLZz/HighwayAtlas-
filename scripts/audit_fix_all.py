#!/usr/bin/env python3
"""
Comprehensive data audit fix for NetworkData.cs
Based on Wikipedia/official sources for all Bulgarian motorway segments.
"""
import sys

PATH = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs"

with open(PATH, encoding="utf-8") as f:
    content = f.read()

original = content
changes = 0

def rep(old, new, label):
    global content, changes
    if old not in content:
        print(f"  MISSING: {label}")
        return
    content = content.replace(old, new, 1)
    changes += 1
    print(f"  OK: {label}")

# =========================================================
# A1-01: Sofia → Plovdiv  length 145 → 132 km
# (Wikipedia: km 0 to km 132.3 Plovdiv-east)
# =========================================================
rep(
    '                sectionName: Text("София – Пловдив", "Sofia – Plovdiv"),\n'
    '                description: Text("Гръбнакът на южната ос. Строителството е продължило 11 години – от първите 10 km до Нови Хан (1978) до пълното достигане на Пловдив (1984).", "Backbone of the southern axis. Construction took 11 years from the first 10 km to Novi Han (1978) to reaching Plovdiv (1984)."),\n'
    '                importance: Text("Свързва столицата с Тракия, индустриалните зони и Черноморието.", "Connects the capital with Thrace, industrial zones, and the Black Sea."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 145,',

    '                sectionName: Text("София – Пловдив", "Sofia – Plovdiv"),\n'
    '                description: Text("Гръбнакът на южната ос. Строителството е продължило 11 години – от първите 10 km до Нови Хан (1978) до пълното достигане на Пловдив (1984).", "Backbone of the southern axis. Construction took 11 years from the first 10 km to Novi Han (1978) to reaching Plovdiv (1984)."),\n'
    '                importance: Text("Свързва столицата с Тракия, индустриалните зони и Черноморието.", "Connects the capital with Thrace, industrial zones, and the Black Sea."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 132,',

    "A1-01 length 145→132"
)

# Fix A1-01 shapePoints – corrected Novi Han coordinate (km 5.9 near ring road)
rep(
    '                    (42.6977, 23.3219, "Sofia"),\n'
    '                    (42.6430, 23.6140, "Novi Han"),\n'
    '                    (42.5500, 23.7500, "Vakarel"),\n'
    '                    (42.4330, 23.8160, "Ihtiman"),\n'
    '                    (42.1900, 24.3300, "Pazardzhik"),\n'
    '                    (42.1354, 24.7453, "Plovdiv")',

    '                    (42.6977, 23.3219, "Sofia ring road"),\n'
    '                    (42.6650, 23.4130, "Novi Han junction"),\n'
    '                    (42.5480, 23.7200, "Vakarel"),\n'
    '                    (42.4380, 23.8180, "Ihtiman"),\n'
    '                    (42.2050, 24.3310, "Pazardzhik"),\n'
    '                    (42.1432, 24.7531, "Plovdiv-east junction")',

    "A1-01 shape points corrected"
)

# =========================================================
# A1-02: Plovdiv → Stara Zagora  length 73 → 76 km
# (km 132.3 Plovdiv-east to km 208.2 Stara Zagora = 75.9 km)
# =========================================================
rep(
    '                sectionName: Text("Пловдив – Стара Загора", "Plovdiv – Stara Zagora"),\n'
    '                description: Text("Изграден на два етапа: 32 km с ЕБВР финансиране (1995) и финалният Участък до Стара Загора с ЕИБ (2007).", "Built in two phases: 32 km EBRD-financed (1995) and the final stretch to Stara Zagora with EIB (2007)."),\n'
    '                importance: Text("Свързва Пловдивската равнина с тракийската ос и черноморския коридор.", "Links the Plovdiv plain to the Thracian axis and the Black Sea corridor."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 73,',

    '                sectionName: Text("Пловдив – Стара Загора", "Plovdiv – Stara Zagora"),\n'
    '                description: Text("Изграден на два етапа: 32 km с ЕБВР финансиране (1995) и финалният участък до Стара Загора с ЕИБ (2007).", "Built in two phases: 32 km EBRD-financed (1995) and the final stretch to Stara Zagora with EIB (2007)."),\n'
    '                importance: Text("Свързва Пловдивската равнина с тракийската ос и черноморския коридор.", "Links the Plovdiv plain to the Thracian axis and the Black Sea corridor."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 76,',

    "A1-02 length 73→76"
)

# Fix A1-02 shapePoints – "Orizovo interchange" is at km 168.5 on A1, lon ~25.29°E
rep(
    '                    (42.1354, 24.7453, "Plovdiv"),\n'
    '                    (42.1510, 24.9800, "Plodovitovo / Orizovo"),\n'
    '                    (42.2002, 25.3290, "Chirpan"),\n'
    '                    (42.4258, 25.6345, "Stara Zagora")',

    '                    (42.1432, 24.7531, "Plovdiv-east junction"),\n'
    '                    (42.1510, 24.9800, "Plodovitovo"),\n'
    '                    (42.1630, 25.2900, "Orizovo interchange (A4 branch)"),\n'
    '                    (42.2000, 25.3500, "Chirpan"),\n'
    '                    (42.4258, 25.6345, "Stara Zagora")',

    "A1-02 shape points – add Orizovo interchange"
)

# =========================================================
# A1-03: Karnobat → Burgas  length 36 → 35 km
# (Wikipedia: km 323.8 Karnobat to km 359 Burgas = 35.2 km; Lot 5 opened 2006)
# =========================================================
rep(
    '                sectionName: Text("Карнобат – Бургас", "Karnobat – Burgas"),\n'
    '                description: Text("Лот 5 – открит в края на 2006 г., цяла година преди централните лотове. Финансиран от ЕИБ.", "Lot 5 – opened late 2006, a full year before the central lots. EIB-financed."),\n'
    '                importance: Text("Осигури директен достъп до пристанище Бургас преди цялата магистрала да е завършена.", "Provided direct access to Burgas Port before the full motorway was complete."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 36,',

    '                sectionName: Text("Карнобат – Бургас", "Karnobat – Burgas"),\n'
    '                description: Text("Лот 5 – открит в края на 2006 г., цяла година преди централните лотове. Финансиран от ЕИБ.", "Lot 5 – opened late 2006, a full year before the central lots. EIB-financed."),\n'
    '                importance: Text("Осигури директен достъп до пристанище Бургас преди цялата магистрала да е завършена.", "Provided direct access to Burgas Port before the full motorway was complete."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 35,',

    "A1-03 length 36→35"
)

# =========================================================
# A1-04: Stara Zagora → Yambol  length 63 → 81 km
# (Wikipedia: km 208.2 Stara Zagora to km 289.4 Yambol-east = 81.2 km)
# =========================================================
rep(
    '                sectionName: Text("Стара Загора – Ямбол", "Stara Zagora – Yambol"),\n'
    '                description: Text("Лотове 2 и 3 – централният тракийски дял, открит в средата на 2012 г. с ЕС кохезионно финансиране.", "Lots 2 and 3 – the central Thracian reach, opened mid-2012 with EU cohesion co-financing."),\n'
    '                importance: Text("Запълни критичната средна пролука и направи София–Ямбол достъпни за под два часа.", "Closed the critical central gap and made Sofia–Yambol accessible in under two hours."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 63,',

    '                sectionName: Text("Стара Загора – Ямбол", "Stara Zagora – Yambol"),\n'
    '                description: Text("Лотове 2 и 3 – централният тракийски дял, открит в средата на 2012 г. с ЕС кохезионно финансиране.", "Lots 2 and 3 – the central Thracian reach, opened mid-2012 with EU cohesion co-financing."),\n'
    '                importance: Text("Запълни критичната средна пролука и направи София–Ямбол достъпни за под два часа.", "Closed the critical central gap and made Sofia–Yambol accessible in under two hours."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 81,',

    "A1-04 length 63→81"
)

# Fix A1-04 shapePoints – use Yambol-east junction coords (km 289.4)
rep(
    '                    (42.4258, 25.6345, "Stara Zagora"),\n'
    '                    (42.4830, 26.0160, "Nova Zagora"),\n'
    '                    (42.4840, 26.5030, "Yambol")',

    '                    (42.4258, 25.6345, "Stara Zagora"),\n'
    '                    (42.4830, 26.0160, "Nova Zagora"),\n'
    '                    (42.4750, 26.3220, "Yambol-west / Sliven junction"),\n'
    '                    (42.4900, 26.5600, "Yambol-east junction")',

    "A1-04 shape points – fix Yambol endpoint + add Sliven junction"
)

# =========================================================
# A1-05: Yambol → Karnobat  length 43 → 34 km; fix wrong waypoint
# (Wikipedia: km 289.4 Yambol-east to km 323.8 Karnobat = 34.4 km)
# "Bolyarovo junction" is WRONG – Bolyarovo is far south of Yambol
# =========================================================
rep(
    '                sectionName: Text("Ямбол – Карнобат", "Yambol – Karnobat"),\n'
    '                description: Text("Лот 4 – последният останал участък, който завърши цялата магистрала. Открит на 15 юли 2013 г. с европейско кохезионно финансиране.", "Lot 4 – the final missing link that completed the entire motorway. Opened 15 July 2013 with EU Cohesion Fund co-financing."),\n'
    '                importance: Text("Затвори последния пролуп в A1 и направи пълната Черноморска ос реалност.", "Closed the last gap in A1 and made the full Black Sea axis a reality."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 43,',

    '                sectionName: Text("Ямбол – Карнобат", "Yambol – Karnobat"),\n'
    '                description: Text("Лот 4 – последният останал участък, който завърши цялата магистрала. Открит на 15 юли 2013 г. с европейско кохезионно финансиране.", "Lot 4 – the final missing link that completed the entire motorway. Opened 15 July 2013 with EU Cohesion Fund co-financing."),\n'
    '                importance: Text("Затвори последния пролуп в A1 и направи пълната Черноморска ос реалност.", "Closed the last gap in A1 and made the full Black Sea axis a reality."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 34,',

    "A1-05 length 43→34"
)

# Fix A1-05 shapePoints – remove wrong "Bolyarovo junction"
rep(
    '                    (42.4840, 26.5030, "Yambol"),\n'
    '                    (42.5440, 26.6870, "Bolyarovo junction"),\n'
    '                    (42.6510, 26.9790, "Karnobat")',

    '                    (42.4900, 26.5600, "Yambol-east junction"),\n'
    '                    (42.5710, 26.7700, "Sakar foothills"),\n'
    '                    (42.6510, 26.9790, "Karnobat")',

    "A1-05 shape points – fix Bolyarovo to correct NE route"
)

# =========================================================
# A2-01: COMPLETE REWRITE
# Was: Sofia→Boaza 43km Open
# Now: Sofia→Ugarchin 103km Open (fully built western section, last piece Dec 2025)
# (Wikipedia: km 0 Sofia ring → km 102.8 Ugarchin temp exit, all in service Dec 2025)
# =========================================================
rep(
    '            // A2-01: Sofia → Boaza (43 km) – Highway Hemus, western section, Open\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-01",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("София – Бoаза", "Sofia – Boaza"),\n'
    '                description: Text("Западният завършен участък на „Хемус" от Ябланица до прохода Боаза в Балкана. Финансиран с ИСПА 2000–2007.", "The western completed section of Hemus motorway through Yablanitsa to the Boaza pass in the Balkan range. ISPA 2000–2007 funded."),\n'
    '                importance: Text("Основна артерия свързваща столицата с централна и северна България.", "Primary artery linking the capital to central and northern Bulgaria."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 43,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2000,\n'
    '                budgetMillionEur: 620,\n'
    '                fundingProgram: "EU Cohesion Fund + ISPA + national",\n'
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (42.6977, 23.3219, "Sofia"),\n'
    '                    (42.8170, 23.5170, "Botevgrad"),\n'
    '                    (42.9400, 23.9100, "Boaza pass")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2000, "ЕС ИСПА финансирането за Хемус е одобрено", "EU ISPA financing for Hemus approved"),\n'
    '                    Milestone(2007, "София – Боаза в пълна експлоатация", "Sofia–Boaza fully operational", "success")\n'
    '                ]),',

    '            // A2-01: Sofia → Ugarchin / Dermantsi (103 km) – Hemus western section, fully Open\n'
    '            // Built in phases 1974–2025: Sofia–Yablanitsa (1999), Yablanitsa–Boaza (2019),\n'
    '            // Boaza–Dermantsi (Oct 2025), Dermantsi–Ugarchin temp exit (Dec 2025)\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-01",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("София – Угърчин (Дерманци)", "Sofia – Ugarchin (Dermantsi area)"),\n'
    '                description: Text("Западният завършен участък на „Хемус" от Софийския пръстен до временния изход при Угърчин. Изграден на етапи 1974–2025 г.: София–Ябланица (1999), Ябланица–Боаза (2019), Боаза–Дерманци (октомври 2025), Дерманци–Угърчин (декември 2025).", "The completed western section of Hemus from the Sofia ring road to the temporary exit at Ugarchin. Built in phases 1974–2025: Sofia–Yablanitsa (1999), Yablanitsa–Boaza (2019), Boaza–Dermantsi (October 2025), Dermantsi–Ugarchin (December 2025)."),\n'
    '                importance: Text("Основна артерия свързваща столицата с Балкана, преодолявайки Витиня и Ечемишка с тунели и виадукти. Бебрешкият виадукт е най-високият магистрален мост на Балканите.", "Primary artery crossing the Balkans via Vitinya and Echemishka tunnels and viaducts. The Bebresh viaduct is the highest motorway bridge in the Balkans."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 103,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 1974,\n'
    '                budgetMillionEur: 720,\n'
    '                fundingProgram: "EU Cohesion Fund + ISPA + national",\n'
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (42.6977, 23.3219, "Sofia ring road"),\n'
    '                    (42.9160, 23.7880, "Botevgrad"),\n'
    '                    (43.0210, 23.9040, "Pravets"),\n'
    '                    (43.0150, 24.0890, "Yablanitsa"),\n'
    '                    (43.1220, 24.4850, "Boaza pass (I-4 junction)"),\n'
    '                    (43.2010, 24.5370, "Dermantsi"),\n'
    '                    (43.1960, 24.4110, "Ugarchin (temporary exit)")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(1974, "Официален старт на строителство на Хемус", "Official groundbreaking of Hemus motorway"),\n'
    '                    Milestone(1999, "София–Ябланица открита (74.6 km)", "Sofia–Yablanitsa opened (74.6 km)", "success"),\n'
    '                    Milestone(2019, "Ябланица–Боаза (Лот 0, 9.3 km) открит", "Yablanitsa–Boaza (Lot 0, 9.3 km) opened", "success"),\n'
    '                    Milestone(2025, "Боаза–Дерманци (10 km) – октомври 2025; Дерманци–Угърчин (5 km) – декември 2025", "Boaza–Dermantsi (10 km) Oct 2025; Dermantsi–Ugarchin (5 km) Dec 2025", "success")\n'
    '                ]),',

    "A2-01 complete rewrite: Sofia→Ugarchin 103km Open"
)

# =========================================================
# A2-02: COMPLETE REWRITE
# Was: Boaza→Dermantsi 63km Open 2021
# Now: Ugarchin→Letnitsa/Krushuna 69km UC ~25%
# (Wikipedia: km 102.8 → km 171.5 = 68.7km under construction lots 2-5)
# =========================================================
rep(
    '            // A2-02: Boaza → Dermantsi (63 km) – Open 2021\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-02",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Бoаза – Дерманци", "Boaza – Dermantsi"),\n'
    '                description: Text("Централният участък от Боаза до Дерманци (Плевенско), открит на 2 ноември 2021 г. Финансиран с КФ на ЕС.", "The central section from Boaza to Dermantsi (near Pleven), opened 2 November 2021. Co-financed by the EU Cohesion Fund."),\n'
    '                importance: Text("Запълни критичния балкански дял и съкрати времето за пътуване София–Варна до под 4 часа.", "Filled the critical Balkan section and cut Sofia–Varna travel time to under 4 hours."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 63,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2015,\n'
    '                budgetMillionEur: 480,\n'
    '                fundingProgram: "EU Cohesion Fund + national",\n'
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway opening 2021",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (42.9400, 23.9100, "Boaza pass"),\n'
    '                    (43.0800, 24.3200, "Lovech area"),\n'
    '                    (43.1800, 24.5500, "Dermantsi")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2015, "Строителство на Боаза–Дерманци стартира", "Construction of Boaza–Dermantsi begins"),\n'
    '                    Milestone(2021, "Боаза–Дерманци открити – критичен балкански участък завършен", "Boaza–Dermantsi opened – critical Balkan section completed", "success")\n'
    '                ]),',

    '            // A2-02: Ugarchin → Letnitsa / Krushuna (69 km) – UC, Lots 2-5 active\n'
    '            // Wikipedia km 102.8 (Ugarchin) → km 171.5 (Krushuna) = 68.7 km under construction\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-02",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Угърчин – Летница / Кръшуна", "Ugarchin – Letnitsa / Krushuna"),\n'
    '                description: Text("В активно строителство участъкът от Угърчин до Летница/Кръшуна (Лотове 2–5). Строителните дейности продължават в района на Плевен, Ловеч и Тетевен. Финансирането е с КФ на ЕС.", "Under active construction from Ugarchin to Letnitsa/Krushuna (Lots 2–5). Works continue in the Pleven, Lovech and Teteven areas. EU Cohesion Fund financing."),\n'
    '                importance: Text("Следващият участък за отваряне на Хемус – ще удължи достигнатото до Угърчин още на север към Плевенска равнина.", "The next section to open on Hemus – will extend the built section further northeast toward the Pleven plain."),\n'
    '                status: SegmentStatus.UnderConstruction,\n'
    '                lengthKm: 69,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2021,\n'
    '                forecastOpenYear: 2029,\n'
    '                completionPercent: 25,\n'
    '                budgetMillionEur: 480,\n'
    '                fundingProgram: "EU Cohesion Fund",\n'
    '                contractor: "Trace Group / Infra Expert / PORR",\n'
    '                sourceName: "Wikipedia – Hemus motorway UC sections",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.1960, 24.4110, "Ugarchin"),\n'
    '                    (43.3240, 24.6200, "Pleven / Lovech junction area"),\n'
    '                    (43.2900, 24.8700, "Letnitsa / Krushuna area")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2021, "Договорите за Лотове 2–5 са подписани", "Contracts for Lots 2–5 signed"),\n'
    '                    Milestone(2023, "Активно строителство по всички лотове", "Active construction on all lots", "info"),\n'
    '                    Milestone(2029, "Прогнозно отваряне Угърчин–Летница", "Forecast opening Ugarchin–Letnitsa", "warning")\n'
    '                ]),',

    "A2-02 complete rewrite: Ugarchin→Letnitsa 69km UC"
)

# =========================================================
# A2-03: COMPLETE REWRITE
# Was: Dermantsi→VT 114km UC 32%  (wrong: VT is at km 222.7 in tender/planned zone; 
#       the km 43.18°N,24.55°E-to-(43.077°N,25.617°E) line crossed A2-04 line)
# Now: Letnitsa→Pavlikeni/VT junction 94km Planned
# (Wikipedia: km 171.5 → km 265.6 = 94.1 km, tender/planned)
# =========================================================
rep(
    '            // A2-03: Dermantsi → Veliko Tarnovo (114 km) – UC 32%\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-03",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Дерманци – Велико Търново", "Dermantsi – Veliko Tarnovo"),\n'
    '                description: Text("Незавършеният централно-северен дял на Хемус. Строителството продължава на отделни лотове; прогноза за завършване 2027 г.", "The uncompleted central-northern section of Hemus. Construction continues in separate lots; forecast completion 2027."),\n'
    '                importance: Text("Затваря пролуката между Плевен и Велико Търново по оста София–Варна.", "Closes the gap between Pleven and Veliko Tarnovo on the Sofia–Varna axis."),\n'
    '                status: SegmentStatus.UnderConstruction,\n'
    '                lengthKm: 114,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2019,\n'
    '                forecastOpenYear: 2027,\n'
    '                completionPercent: 32,\n'
    '                budgetMillionEur: 680,\n'
    '                fundingProgram: "EU Cohesion Fund",\n'
    '                contractor: "Trace Group / Buildinvest / PORR",\n'
    '                sourceName: "API Bulgaria – Hemus motorway progress",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.1800, 24.5500, "Dermantsi"),\n'
    '                    (43.3300, 24.9500, "Pleven area"),\n'
    '                    (43.2200, 25.4500, "Pavlikeni area"),\n'
    '                    (43.0770, 25.6173, "Veliko Tarnovo")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2019, "Строителството на Дерманци–ВТ стартира на отделни лотове", "Construction of Dermantsi–VT begins in separate lots"),\n'
    '                    Milestone(2023, "Лот 1 е на 65% завършеност", "Lot 1 at 65% completion", "info"),\n'
    '                    Milestone(2027, "Прогнозно отваряне на целия участък", "Projected opening of full section", "warning")\n'
    '                ]),',

    '            // A2-03: Letnitsa / Krushuna → Polikraishte (VT junction) (94 km) – Planned / Tender\n'
    '            // Wikipedia km 171.5 (Krushuna) → km 265.6 (Kovachevsko kale) = 94.1 km\n'
    '            // Status: tender/design for km 171.5-222.7; planned beyond VT junction\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-03",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Летница – Поликраище (VT разклон)", "Letnitsa – Polikraishte (VT junction)"),\n'
    '                description: Text("Планираният централен дял на Хемус от Летница до разклона за Велико Търново при Поликраище. Участъкът преминава северно от ВТ по Дунавската равнина. Тръжните процедури за Лотове 6–9 се подготвят с финансиране от КФ на ЕС.", "The planned central section of Hemus from Letnitsa to the Veliko Tarnovo junction at Polikraishte. The route passes north of VT through the Danubian plain. Tender procedures for Lots 6–9 are being prepared with EU Cohesion Fund financing."),\n'
    '                importance: Text("Планираният участък от TEN-T ядрова мрежа, свързващ Пловенската равнина с оста към Шумен и Варна.", "Planned TEN-T Core Network section connecting the Pleven plain to the Shumen–Varna axis."),\n'
    '                status: SegmentStatus.Planned,\n'
    '                lengthKm: 94,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2027,\n'
    '                forecastOpenYear: 2032,\n'
    '                completionPercent: 0,\n'
    '                budgetMillionEur: 680,\n'
    '                fundingProgram: "EU Cohesion Fund + CEF",\n'
    '                contractor: "TBD",\n'
    '                sourceName: "Wikipedia – Hemus motorway exits table",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.2900, 24.8700, "Letnitsa / Krushuna"),\n'
    '                    (43.2450, 25.3200, "Pavlikeni"),\n'
    '                    (43.3100, 25.6200, "Daskot / VT area"),\n'
    '                    (43.3600, 25.7200, "Polikraishte (VT junction)")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2024, "Технически проекти за Лотове 6–9 в разработка", "Technical designs for Lots 6–9 in development", "info"),\n'
    '                    Milestone(2027, "Очаквано начало на строителство", "Expected construction start", "warning"),\n'
    '                    Milestone(2032, "Прогнозно завършване – Летница–Поликраище", "Forecast completion – Letnitsa–Polikraishte", "warning")\n'
    '                ]),',

    "A2-03 complete rewrite: Letnitsa→VT junction 94km Planned"
)

# =========================================================
# A2-04: COMPLETE REWRITE
# Was: VT→Shumen 125km Planned  (wrong: VT not on route; Shumen is in eastern open section)
# Now: Polikraishte→Buhovtsi 45km Planned (remaining gap before eastern open section)
# (Wikipedia: km 265.6 → km 310.9 = 45.3 km; km 299-310.9 is last UC lot)
# =========================================================
rep(
    '            // A2-04: Veliko Tarnovo → Shumen (125 km) – Planned\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-04",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Велико Търново – Шумен", "Veliko Tarnovo – Shumen"),\n'
    '                description: Text("Планираният изток на Хемус от Велико Търново до Шумен. Тръжните процедури се подготвят; финансирането с КФ на ЕС и CEF е в процес на договаряне.", "The planned eastern section of Hemus from Veliko Tarnovo to Shumen. Tender procedures being prepared; EU CF and CEF financing under negotiation."),\n'
    '                importance: Text("Звено от TEN-T Core Network. Ще намали времето за пътуване до Черно море допълнително с ~40 мин.", "TEN-T Core Network link. Will further cut travel time to the Black Sea by ~40 min."),\n'
    '                status: SegmentStatus.Planned,\n'
    '                lengthKm: 125,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2025,\n'
    '                forecastOpenYear: 2032,\n'
    '                budgetMillionEur: 520,\n'
    '                fundingProgram: "EU Cohesion Fund + CEF",\n'
    '                contractor: "TBD",\n'
    '                sourceName: "МРРБ – Hemus motorway master plan",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.0770, 25.6173, "Veliko Tarnovo"),\n'
    '                    (43.2560, 26.2340, "Popovo"),\n'
    '                    (43.2720, 26.9236, "Shumen")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2022, "ОВОС процедурата е стартирана", "EIA procedure started"),\n'
    '                    Milestone(2025, "Очаквано начало на строителство", "Expected construction start", "warning")\n'
    '                ]),',

    '            // A2-04: Polikraishte → Buhovtsi / Targovishte (45 km) – Planned + small UC lot\n'
    '            // Wikipedia km 265.6 (Kovachevsko kale) → km 310.9 (Buhovtsi) = 45.3 km\n'
    '            // km 299-310.9 (Loznitsa→Buhovtsi) is currently "Under construction"\n'
    '            CreateSegment(\n'
    '                routeCode: "A2",\n'
    '                name: "Hemus Motorway",\n'
    '                sectionCode: "A2-04",\n'
    '                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),\n'
    '                sectionName: Text("Поликраище – Буховци (Търговище)", "Polikraishte – Buhovtsi (Targovishte)"),\n'
    '                description: Text("Последната незастроена пролука на Хемус преди достигане до завършения Буховци–Варна участък. Участъкът Лозница–Буховци (12 km) е в активно строителство; останалата Поликраище–Лозница (33 km) е планирана.", "The last unbuilt gap on Hemus before reaching the completed Buhovtsi–Varna section. The Loznitsa–Buhovtsi lot (12 km) is under active construction; the remaining Polikraishte–Loznitsa (33 km) is planned."),\n'
    '                importance: Text("Ще затвори последната пролука по оста София–Варна и ще свърже в едно двата завършени края на Хемус.", "Will close the last gap on the Sofia–Varna axis and unite the two completed ends of Hemus."),\n'
    '                status: SegmentStatus.Planned,\n'
    '                lengthKm: 45,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2025,\n'
    '                forecastOpenYear: 2033,\n'
    '                completionPercent: 10,\n'
    '                budgetMillionEur: 350,\n'
    '                fundingProgram: "EU Cohesion Fund",\n'
    '                contractor: "TBD",\n'
    '                sourceName: "Wikipedia – Hemus motorway exits (km 265–310)",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.3600, 25.7200, "Polikraishte (VT junction)"),\n'
    '                    (43.3700, 26.2000, "Popovo area"),\n'
    '                    (43.3700, 26.5400, "Loznitsa (UC lot)"),\n'
    '                    (43.1930, 26.5680, "Buhovtsi / Targovishte")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(2023, "Лозница–Буховци: строителство започнато", "Loznitsa–Buhovtsi: construction started", "info"),\n'
    '                    Milestone(2025, "Тръжни процедури за Поликраище–Лозница", "Tender procedures for Polikraishte–Loznitsa", "warning"),\n'
    '                    Milestone(2033, "Прогнозно завършване – Поликраище–Буховци", "Forecast completion – Polikraishte–Buhovtsi", "warning")\n'
    '                ]),',

    "A2-04 complete rewrite: Polikraishte→Buhovtsi 45km Planned"
)

# =========================================================
# A2-05: UPDATE sectionName, length 73→107, shapePoints
# Was: Shumen→Varna 73km
# Now: Buhovtsi→Varna 107km Open (full eastern open section)
# (Wikipedia: km 310.9 Buhovtsi → km 418 Varna = 107.1 km, all in service)
# =========================================================
rep(
    '                sectionName: Text("Шумен – Варна", "Shumen – Varna"),\n'
    '                description: Text("Източният завършен дял на Хемус, свързващ Шумен с черноморската столица Варна. Открит на части 1974–2002.", "The eastern completed section of Hemus connecting Shumen with the Black Sea capital Varna. Opened in phases 1974–2002."),\n'
    '                importance: Text("Директна магистрална връзка с пристанище Варна – вход за черноморската търговия.", "Direct motorway link to Varna Port – gateway for Black Sea trade."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 73,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 1970,\n'
    '                budgetMillionEur: 110,\n'
    '                fundingProgram: "National budget + EIB",\n'
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.2720, 26.9236, "Shumen"),\n'
    '                    (43.3300, 27.4200, "Provadiya"),\n'
    '                    (43.2141, 27.9147, "Varna")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(1974, "Шумен–Варна открит за движение", "Shumen–Varna opened to traffic", "success"),\n'
    '                    Milestone(2002, "Четирилентова реконструкция завършена", "Four-lane reconstruction completed", "success")\n'
    '                ]),',

    '                sectionName: Text("Буховци – Варна (Шумен – Варна)", "Buhovtsi – Varna (via Shumen)"),\n'
    '                description: Text("Завършениятизточен дял на Хемус от Буховци (Търговище) до Варна. Включва историческата Шумен–Варна ос (1974 г.) и разширенията 2013–2022 г. до Буховци. Открит на участъци в рамките на 1974–2022 г.", "The completed eastern section of Hemus from Buhovtsi (Targovishte) to Varna. Includes the historic Shumen–Varna axis (1974) and 2013–2022 extensions to Buhovtsi. Opened in phases 1974–2022."),\n'
    '                importance: Text("Директна магистрална връзка с пристанище Варна и Черноморието. Обхваща изцяло завършените 107 km от ТЕН-Т ядровата мрежа.", "Direct motorway link to Varna Port and the Black Sea. Covers the fully completed 107 km of TEN-T Core Network."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 107,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 1974,\n'
    '                budgetMillionEur: 240,\n'
    '                fundingProgram: "National budget + EIB + EU SF",\n'
    '                completionPercent: 100,\n'
    '                sourceName: "Wikipedia – Hemus motorway",\n'
    '                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",\n'
    '                shapePoints:\n'
    '                [\n'
    '                    (43.1930, 26.5680, "Buhovtsi / Targovishte"),\n'
    '                    (43.3700, 27.0400, "Belokopitovo / Shumen approach"),\n'
    '                    (43.2720, 26.9390, "Shumen"),\n'
    '                    (43.3300, 27.4200, "Provadiya"),\n'
    '                    (43.2141, 27.9147, "Varna")\n'
    '                ],\n'
    '                milestones:\n'
    '                [\n'
    '                    Milestone(1974, "Шумен–Варна открит за движение", "Shumen–Varna opened to traffic", "success"),\n'
    '                    Milestone(2005, "Участък при Шумен разширен", "Shumen section extended", "success"),\n'
    '                    Milestone(2015, "Белокопитово (16.3 km) открит – 2015", "Belokopitovo section (16.3 km) opened 2015", "success"),\n'
    '                    Milestone(2022, "Буховци–Белокопитово открит октомври 2022 – Буховци–Варна напълно в сервиз", "Buhovtsi–Belokopitovo opened Oct 2022 – Buhovtsi–Varna fully in service", "success")\n'
    '                ]),',

    "A2-05 update: Buhovtsi→Varna 107km Open"
)

# =========================================================
# A3-02: Kresna Gorge section
# Was: Simitli→Sandanski 35km UC 34%  (WRONG: lot 3.2 is PLANNED, lot 3.3 is OPEN)
# Now: Simitli→Kresna gorge south end 23km PLANNED (Lot 3.2 blocked since Aug 2022)
# (Wikipedia: km 105.4 Simitli → km 128 Dolna Gradeshnitsa = 22.6 km, listed as PLANNED)
# =========================================================
rep(
    '                sectionName: Text("Симитли – Сандански (Кресненско дефиле)", "Simitli – Sandanski (Kresna Gorge)"),\n'
    '                description: Text(\n'
    '                    "Кресненското дефиле е най-спорният строителен участък в историята на българската пътна инфраструктура. Едновременно е природозащитен обект и единственият технически коридор за магистралата.",\n'
    '                    "The Kresna gorge is the most contested construction site in Bulgarian infrastructure history. It is simultaneously a protected natural area and the only viable motorway corridor."),\n'
    '                importance: Text(\n'
    '                    "Трансевропейски коридор IV (TEN-T). Без него целият коридор Сърбия–България–Гърция застава претоварен алтернативен път.",\n'
    '                    "Trans-European Corridor IV (TEN-T). Without it, the entire Serbia–Bulgaria–Greece corridor remains on congested alternative routes."),\n'
    '                status: SegmentStatus.UnderConstruction,\n'
    '                lengthKm: 35,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2019,\n'
    '                forecastOpenYear: 2028,\n'
    '                completionPercent: 34,\n'
    '                budgetMillionEur: 460,\n'
    '                fundingProgram: "EU Cohesion Fund + national",\n'
    '                contractor: "Trace Group / Strabag JV",\n'
    '                sourceName: "МРРБ / API Bulgaria – Struma Lot 3.2 / 3.3",',

    '                sectionName: Text("Симитли – Кресненско дефиле (Лот 3.2)", "Simitli – Kresna Gorge (Lot 3.2)"),\n'
    '                description: Text(\n'
    '                    "Кресненското дефиле е най-спорният строителен обект в историята на българската пътна инфраструктура. Проектът е спрян от Министерството на околната среда (август 2022 г.) до ново ОВОС и оценка по Натура 2000. Дефилето е Натура 2000 обект с уникален биоразнообразие.",\n'
    '                    "The Kresna gorge (Lot 3.2) is the most contested infrastructure project in Bulgarian history. Construction was halted by the Environment Ministry in August 2022 pending a new EIA and Natura 2000 assessment. The gorge is a Natura 2000 site with unique biodiversity."),\n'
    '                importance: Text(\n'
    '                    "Единственият незавършен участък в Коридор IV до гръцката граница. Спирането на проекта означава, че трафикът продължава по претоварения път I-1 през дефилето.",\n'
    '                    "The only incomplete section in Corridor IV to the Greek border. The project halt means traffic continues on congested I-1 through the gorge."),\n'
    '                status: SegmentStatus.Planned,\n'
    '                lengthKm: 23,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2027,\n'
    '                forecastOpenYear: 2031,\n'
    '                completionPercent: 0,\n'
    '                budgetMillionEur: 380,\n'
    '                fundingProgram: "EU Cohesion Fund + national",\n'
    '                contractor: "TBD – project suspended pending new EIA",\n'
    '                sourceName: "Wikipedia – Struma motorway Lot 3.2 Kresna Gorge",',

    "A3-02 fix: Kresna Gorge 23km PLANNED (blocked 2022)"
)

# Fix A3-02 shapePoints (shorter, km 105.4 → km 128)
rep(
    '                    (41.8950, 23.0900, "Simitli"),\n'
    '                    (41.7900, 23.1100, "Kresna gorge north"),\n'
    '                    (41.7050, 23.1500, "Kresna"),\n'
    '                    (41.5639, 23.2761, "Sandanski")',

    '                    (41.8950, 23.0900, "Simitli"),\n'
    '                    (41.7700, 23.1200, "Kresna gorge north approach"),\n'
    '                    (41.7220, 23.1350, "Kresna gorge (Lot 3.2 – blocked)"),\n'
    '                    (41.6980, 23.1520, "Dolna Gradeshnitsa (south end)")',

    "A3-02 shape points corrected"
)

# Fix A3-02 milestones (remove wrong 2028; update)
rep(
    '                    Milestone(1999, "Европейски протести срещу трасето по дефилето", "European objections to the route through the gorge", "warning"),\n'
    '                    Milestone(2017, "ОВОС одобрен след ревизия; ново трасе в дефилето", "EIA approved after revision; new alignment through the gorge", "info"),\n'
    '                    Milestone(2019, "Строителството на Лот 3.2/3.3 (Кресненско дефиле) стартира", "Construction of Lot 3.2/3.3 (Kresna gorge) begins"),\n'
    '                    Milestone(2028, "Прогнозна дата за цялостно завършване", "Forecast completion date", "warning")',

    '                    Milestone(1999, "Европейски протести срещу трасето по дефилето", "European objections to the route through the gorge", "warning"),\n'
    '                    Milestone(2017, "ОВОС одобрен след ревизия; ново трасе в дефилето", "EIA approved after revision; new alignment through the gorge", "info"),\n'
    '                    Milestone(2022, "Стоп: Министерство на ОС спира Лот 3.2 – необходима нова ОВОС и оценка Натура 2000", "Stop: Environment Ministry halts Lot 3.2 – new EIA and Natura 2000 assessment required", "warning"),\n'
    '                    Milestone(2031, "Прогнозна дата за цялостно завършване (при одобрен нов проект)", "Forecast completion date (subject to new project approval)", "warning")',

    "A3-02 milestones updated"
)

# =========================================================
# A3-03: Sandanski → Kulata
# Was: 29km PLANNED  (COMPLETELY WRONG)
# Now: 41km OPEN since 2015/2018
# (Wikipedia: km 128 Dolna Gradeshnitsa → km 168.5 Kulata = 40.5 km; Lot 4 opened Aug 2015,
#  Lot 3.3 Kresna-Sandanski opened Dec 2018)
# =========================================================
rep(
    '                sectionName: Text("Сандански – Кулата (граница)", "Sandanski – Kulata (border)"),\n'
    '                description: Text("Последният планиран участък на „Струма" от Сандански до ГКПП Кулата на границата с Гърция.", "The final planned section of Struma from Sandanski to the Kulata border crossing with Greece."),\n'
    '                importance: Text("Ще завърши коридор IV до гръцката граница и Солун.", "Will complete Corridor IV to the Greek border and Thessaloniki."),\n'
    '                status: SegmentStatus.Planned,\n'
    '                lengthKm: 29,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2028,\n'
    '                forecastOpenYear: 2030,\n'
    '                budgetMillionEur: 95,\n'
    '                fundingProgram: "National budget",\n'
    '                contractor: "TBD",\n'
    '                sourceName: "МРРБ – A3 Struma completion plan",',

    '                sectionName: Text("Кресна – Кулата (Лот 3.3 + Лот 4)", "Kresna – Kulata (Lots 3.3 + 4)"),\n'
    '                description: Text("Завършеният южен участък на „Струма" от Кресна до ГКПП Кулата на границата с Гърция. Лот 4 (Сандански–Кулата) е открит на 20 август 2015 г., а Лот 3.3 (Кресна–Сандански) е завършен на 17 декември 2018 г.", "The completed southern section of Struma from Kresna to the Kulata border crossing. Lot 4 (Sandanski–Kulata) opened 20 August 2015; Lot 3.3 (Kresna–Sandanski) completed 17 December 2018."),\n'
    '                importance: Text("Завършен участък на Коридор IV до гръцката граница и Солун. Единственото прекъсване остава Кресненското дефиле (Лот 3.2).", "Completed Corridor IV section to the Greek border and Thessaloniki. The only break remaining is the Kresna Gorge (Lot 3.2)."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 41,\n'
    '                maxSpeedKph: 140,\n'
    '                startYear: 2013,\n'
    '                completionPercent: 100,\n'
    '                budgetMillionEur: 290,\n'
    '                fundingProgram: "EU Cohesion Fund + national",\n'
    '                sourceName: "Wikipedia – Struma motorway exits table",',

    "A3-03 fix: Kresna→Kulata 41km OPEN (was 29km Planned)"
)

# Fix A3-03 shapePoints (from Dolna Gradeshnitsa km 128 to Kulata km 168.5)
rep(
    '                    (41.5639, 23.2761, "Sandanski"),\n'
    '                    (41.4200, 23.3000, "Petrich area"),\n'
    '                    (41.3380, 23.3670, "Kulata border crossing")',

    '                    (41.6980, 23.1520, "Dolna Gradeshnitsa (Lot 3.3 start)"),\n'
    '                    (41.5639, 23.2761, "Sandanski"),\n'
    '                    (41.4342, 23.2050, "Petrich"),\n'
    '                    (41.3380, 23.3670, "Kulata border crossing")',

    "A3-03 shape points corrected"
)

# Fix A3-03 milestones
rep(
    '                    Milestone(2028, "Очаквано начало на строителство след завършване на Кресна", "Expected construction start after Kresna section completion", "warning"),\n'
    '                    Milestone(2030, "Прогнозно завършване – А3 напълно завършена", "Forecast completion – A3 fully complete", "warning")',

    '                    Milestone(2013, "Строителство на Лот 3.3 (Кресна–Сандански) и Лот 4 (Сандански–Кулата) стартира", "Construction of Lot 3.3 (Kresna–Sandanski) and Lot 4 (Sandanski–Kulata) begins"),\n'
    '                    Milestone(2015, "Лот 4 Сандански–Кулата открит на 20 август 2015 г.", "Lot 4 Sandanski–Kulata opened 20 August 2015", "success"),\n'
    '                    Milestone(2018, "Лот 3.3 Кресна–Сандански завършен (17 декември 2018 г.) – южен А3 напълно отворен", "Lot 3.3 Kresna–Sandanski completed (17 December 2018) – southern A3 fully open", "success")',

    "A3-03 milestones updated"
)

# =========================================================
# A4-01: Fix start coordinate – Orizovo interchange, not Plovdiv city centre
# (Wikipedia: A4 starts at Orizovo interchange on A1 between Plovdiv and Chirpan,
#  at A1 km 168.5; approximate coord (42.163°N, 25.290°E))
# =========================================================
rep(
    '                    (42.1354, 24.7453, "Plovdiv"),\n'
    '                    (42.0500, 25.3000, "Haskovo area"),\n'
    '                    (41.8710, 25.7990, "Svilengrad"),\n'
    '                    (41.7420, 26.3410, "Kapitan Andreevo border")',

    '                    (42.1630, 25.2900, "Orizovo interchange (A1 junction)"),\n'
    '                    (42.1430, 25.5700, "Dimitrovgrad area"),\n'
    '                    (41.9330, 25.7990, "Haskovo"),\n'
    '                    (41.8710, 25.9700, "Svilengrad"),\n'
    '                    (41.7420, 26.3410, "Kapitan Andreevo border")',

    "A4-01 start moved from Plovdiv to Orizovo interchange"
)

# Also fix A4-01 sectionName (was Plovdiv–Kapitan Andreevo, should be Трakia junction – Kapitan Andreevo)
rep(
    '                sectionName: Text("Пловдив – Капитан Андреево (граница)", "Plovdiv – Kapitan Andreevo (border)"),',
    '                sectionName: Text("А1/Оризово – Капитан Андреево (граница)", "A1/Orizovo junction – Kapitan Andreevo (border)"),',
    "A4-01 sectionName corrected to Orizovo"
)

# =========================================================
# E79-02: Fix mislabeled "Mezdra area" point at (42.92, 23.78)
# That coord is near Botevgrad, not Mezdra (which is at 43.15, 23.71)
# =========================================================
rep(
    '                    (43.4082, 23.2254, "Montana"),\n'
    '                    (43.2000, 23.5000, "Vratsa area"),\n'
    '                    (42.9200, 23.7800, "Mezdra area"),\n'
    '                    (42.9050, 23.7900, "Botevgrad")',

    '                    (43.4082, 23.2254, "Montana"),\n'
    '                    (43.2100, 23.5530, "Vratsa"),\n'
    '                    (43.1490, 23.7100, "Mezdra"),\n'
    '                    (42.9050, 23.7900, "Botevgrad")',

    "E79-02 fix Mezdra coord (was at Botevgrad); add Mezdra at correct 43.15°N"
)

# =========================================================
# A3-01: length 107→105 (Wikipedia km 0 → km 105.4 Simitli = 105.4 km)
# =========================================================
rep(
    '                sectionName: Text("София – Симитли", "Sofia – Simitli"),\n'
    '                description: Text("Завършеният участък по долината на Струма от Перник до Симитли. Основните лотове се пускат поетапно 2009–2016 г. с ИСПА и КФ на ЕС.", "The completed section along the Struma valley from Pernik to Simitli. Major lots opened 2009–2016 with EU ISPA and Cohesion Fund co-financing."),\n'
    '                importance: Text("Ключова артерия към Благоевград, Югозападен университет и граничния преход. Международен транзитен коридор IV.", "Key artery to Blagoevgrad, South-West University, and the border crossing. International Transit Corridor IV."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 107,',

    '                sectionName: Text("София – Симитли", "Sofia – Simitli"),\n'
    '                description: Text("Завършеният участък по долината на Струма от Перник до Симитли. Основните лотове се пускат поетапно 2009–2016 г. с ИСПА и КФ на ЕС.", "The completed section along the Struma valley from Pernik to Simitli. Major lots opened 2009–2016 with EU ISPA and Cohesion Fund co-financing."),\n'
    '                importance: Text("Ключова артерия към Благоевград, Югозападен университет и граничния преход. Международен транзитен коридор IV.", "Key artery to Blagoevgrad, South-West University, and the border crossing. International Transit Corridor IV."),\n'
    '                status: SegmentStatus.Open,\n'
    '                lengthKm: 105,',

    "A3-01 length 107→105"
)

# =========================================================
# A4-01: fix last milestone year (was 2012 but completed 2015)
# =========================================================
rep(
    '                    Milestone(2012, "Хасково–Капитан Андреево открит – Марица завършена", "Haskovo–Kapitan Andreevo opened – Maritsa complete", "success")',
    '                    Milestone(2015, "Хасково–Капитан Андреево открит – Марица завършена (29 октомври 2015)", "Haskovo–Kapitan Andreevo opened – Maritsa complete (29 October 2015)", "success")',
    "A4-01 completion year 2012→2015"
)

print(f"\nTotal changes applied: {changes}")

if changes == 0:
    print("ERROR: No changes applied!")
    sys.exit(1)

with open(PATH, "w", encoding="utf-8") as f:
    f.write(content)

print("File written successfully.")

# Verify no accidental corruptions by checking opening lines
with open(PATH, encoding="utf-8") as f:
    lines = f.readlines()
print(f"File has {len(lines)} lines.")
