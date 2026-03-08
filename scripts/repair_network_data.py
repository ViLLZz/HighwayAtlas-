"""
Repair NetworkData.cs — re-insert all 13 segments that were lost.
Inserts:
  - A1-05 (Yambol-Karnobat)
  - A2-01..05 (Hemus + Shumen-Varna)
  after A1-04, and:
  - A3-03 (Sandanski-Kulata)
  - A4-01 Maritsa
  - A5-01 Cherno More
  - A6-01 Europe
  - E79-01 Vidin-Montana
  - E79-02 Montana-Botevgrad
  - RVT-01 Ruse-VT
  - MB-01 Mezdra-Botevgrad
  before 'return network;'
"""

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# ─── BLOCK A: A1-05 + A2-01..A2-05 ──────────────────────────────────────────
BLOCK_A1_05_AND_A2 = """            // A1-05: Yambol → Karnobat (43 km) – opened 15 July 2013
            // Historical: Lot 4 – the very last piece, completing Bulgaria's first full motorway
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-05",
                displayName: Text("Автомагистрала „Тракия"", "Trakia Motorway"),
                sectionName: Text("Ямбол – Карнобат", "Yambol – Karnobat"),
                description: Text("Лот 4 – последният останал участък, който завърши цялата магистрала. Открит на 15 юли 2013 г. с европейско кохезионно финансиране.", "Lot 4 – the final missing link that completed the entire motorway. Opened 15 July 2013 with EU Cohesion Fund co-financing."),
                importance: Text("Затвори последния пролуп в A1 и направи пълната Черноморска ос реалност.", "Closed the last gap in A1 and made the full Black Sea axis a reality."),
                status: SegmentStatus.Open,
                lengthKm: 43,
                maxSpeedKph: 140,
                startYear: 2010,
                budgetMillionEur: 98,
                fundingProgram: "EU Cohesion Fund",
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway, Lot 4 completion 2013",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.4840, 26.5030, "Yambol"),
                    (42.5440, 26.6870, "Bolyarovo junction"),
                    (42.6510, 26.9790, "Karnobat")
                ],
                milestones:
                [
                    Milestone(2010, "Договорът за Лот 4 (Ямбол – Карнобат) е подписан", "Contract for Lot 4 (Yambol–Karnobat) signed"),
                    Milestone(2013, "Лот 4 в ekspлоатация – A1 Тракия е завършена", "Lot 4 operational – A1 Trakia completed", "success")
                ]),

            // A2-01: Sofia → Boaza (43 km) – Highway Hemus, western section, Open
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-01",
                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),
                sectionName: Text("София – Бoаза", "Sofia – Boaza"),
                description: Text("Западният завършен участък на „Хемус" от Ябланица до прохода Боаза в Балкана. Финансиран с ИСПА 2000–2007.", "The western completed section of Hemus motorway through Yablanitsa to the Boaza pass in the Balkan range. ISPA 2000–2007 funded."),
                importance: Text("Основна артерия свързваща столицата с централна и северна България.", "Primary artery linking the capital to central and northern Bulgaria."),
                status: SegmentStatus.Open,
                lengthKm: 43,
                maxSpeedKph: 140,
                startYear: 2000,
                budgetMillionEur: 620,
                fundingProgram: "EU Cohesion Fund + ISPA + national",
                completionPercent: 100,
                sourceName: "Wikipedia – Hemus motorway",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (42.6977, 23.3219, "Sofia"),
                    (42.8170, 23.5170, "Botevgrad"),
                    (42.9400, 23.9100, "Boaza pass")
                ],
                milestones:
                [
                    Milestone(2000, "ЕС ИСПА финансирането за Хемус е одобрено", "EU ISPA financing for Hemus approved"),
                    Milestone(2007, "София – Боаза в пълна експлоатация", "Sofia–Boaza fully operational", "success")
                ]),

            // A2-02: Boaza → Dermantsi (63 km) – Open 2021
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-02",
                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),
                sectionName: Text("Бoаза – Дерманци", "Boaza – Dermantsi"),
                description: Text("Централният участък от Боаза до Дерманци (Плевенско), открит на 2 ноември 2021 г. Финансиран с КФ на ЕС.", "The central section from Boaza to Dermantsi (near Pleven), opened 2 November 2021. Co-financed by the EU Cohesion Fund."),
                importance: Text("Запълни критичния балкански дял и съкрати времето за пътуване София–Варна до под 4 часа.", "Filled the critical Balkan section and cut Sofia–Varna travel time to under 4 hours."),
                status: SegmentStatus.Open,
                lengthKm: 63,
                maxSpeedKph: 140,
                startYear: 2015,
                budgetMillionEur: 480,
                fundingProgram: "EU Cohesion Fund + national",
                completionPercent: 100,
                sourceName: "Wikipedia – Hemus motorway opening 2021",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (42.9400, 23.9100, "Boaza pass"),
                    (43.0800, 24.3200, "Lovech area"),
                    (43.1800, 24.5500, "Dermantsi")
                ],
                milestones:
                [
                    Milestone(2015, "Строителство на Боаза–Дерманци стартира", "Construction of Boaza–Dermantsi begins"),
                    Milestone(2021, "Боаза–Дерманци открити – критичен балкански участък завършен", "Boaza–Dermantsi opened – critical Balkan section completed", "success")
                ]),

            // A2-03: Dermantsi → Veliko Tarnovo (114 km) – UC 32%
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-03",
                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),
                sectionName: Text("Дерманци – Велико Търново", "Dermantsi – Veliko Tarnovo"),
                description: Text("Незавършеният централно-северен дял на Хемус. Строителството продължава на отделни лотове; прогноза за завършване 2027 г.", "The uncompleted central-northern section of Hemus. Construction continues in separate lots; forecast completion 2027."),
                importance: Text("Затваря пролуката между Плевен и Велико Търново по оста София–Варна.", "Closes the gap between Pleven and Veliko Tarnovo on the Sofia–Varna axis."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 114,
                maxSpeedKph: 140,
                startYear: 2019,
                forecastOpenYear: 2027,
                completionPercent: 32,
                budgetMillionEur: 680,
                fundingProgram: "EU Cohesion Fund",
                contractor: "Trace Group / Buildinvest / PORR",
                sourceName: "API Bulgaria – Hemus motorway progress",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.1800, 24.5500, "Dermantsi"),
                    (43.3300, 24.9500, "Pleven area"),
                    (43.2200, 25.4500, "Pavlikeni area"),
                    (43.0770, 25.6173, "Veliko Tarnovo")
                ],
                milestones:
                [
                    Milestone(2019, "Строителството на Дерманци–ВТ стартира на отделни лотове", "Construction of Dermantsi–VT begins in separate lots"),
                    Milestone(2023, "Лот 1 е на 65% завършеност", "Lot 1 at 65% completion", "info"),
                    Milestone(2027, "Прогнозно отваряне на целия участък", "Projected opening of full section", "warning")
                ]),

            // A2-04: Veliko Tarnovo → Shumen (125 km) – Planned
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-04",
                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),
                sectionName: Text("Велико Търново – Шумен", "Veliko Tarnovo – Shumen"),
                description: Text("Планираният изток на Хемус от Велико Търново до Шумен. Тръжните процедури се подготвят; финансирането с КФ на ЕС и CEF е в процес на договаряне.", "The planned eastern section of Hemus from Veliko Tarnovo to Shumen. Tender procedures being prepared; EU CF and CEF financing under negotiation."),
                importance: Text("Звено от TEN-T Core Network. Ще намали времето за пътуване до Черно море допълнително с ~40 мин.", "TEN-T Core Network link. Will further cut travel time to the Black Sea by ~40 min."),
                status: SegmentStatus.Planned,
                lengthKm: 125,
                maxSpeedKph: 140,
                startYear: 2025,
                forecastOpenYear: 2032,
                budgetMillionEur: 520,
                fundingProgram: "EU Cohesion Fund + CEF",
                contractor: "TBD",
                sourceName: "МРРБ – Hemus motorway master plan",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.0770, 25.6173, "Veliko Tarnovo"),
                    (43.2560, 26.2340, "Popovo"),
                    (43.2720, 26.9236, "Shumen")
                ],
                milestones:
                [
                    Milestone(2022, "ОВОС процедурата е стартирана", "EIA procedure started"),
                    Milestone(2025, "Очаквано начало на строителство", "Expected construction start", "warning")
                ]),

            // A2-05: Shumen → Varna (73 km) – Open
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-05",
                displayName: Text("Автомагистрала „Хемус"", "Hemus Motorway"),
                sectionName: Text("Шумен – Варна", "Shumen – Varna"),
                description: Text("Източният завършен дял на Хемус, свързващ Шумен с черноморската столица Варна. Открит на части 1974–2002.", "The eastern completed section of Hemus connecting Shumen with the Black Sea capital Varna. Opened in phases 1974–2002."),
                importance: Text("Директна магистрална връзка с пристанище Варна – вход за черноморската търговия.", "Direct motorway link to Varna Port – gateway for Black Sea trade."),
                status: SegmentStatus.Open,
                lengthKm: 73,
                maxSpeedKph: 140,
                startYear: 1970,
                budgetMillionEur: 110,
                fundingProgram: "National budget + EIB",
                completionPercent: 100,
                sourceName: "Wikipedia – Hemus motorway",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.2720, 26.9236, "Shumen"),
                    (43.3300, 27.4200, "Provadiya"),
                    (43.2141, 27.9147, "Varna")
                ],
                milestones:
                [
                    Milestone(1974, "Шумен–Варна открит за движение", "Shumen–Varna opened to traffic", "success"),
                    Milestone(2002, "Четирилентова реконструкция завършена", "Four-lane reconstruction completed", "success")
                ]),

"""

# ─── BLOCK B: A3-03 + A4 + A5 + A6 + E79 + RVT + MB ─────────────────────────
BLOCK_REST = """            // A3-03: Sandanski → Kulata border crossing (29 km) – Planned
            CreateSegment(
                routeCode: "A3",
                name: "Struma Motorway",
                sectionCode: "A3-03",
                displayName: Text("Автомагистрала „Струма"", "Struma Motorway"),
                sectionName: Text("Сандански – Кулата (граница)", "Sandanski – Kulata (border)"),
                description: Text("Последният планиран участък на „Струма" от Сандански до ГКПП Кулата на границата с Гърция.", "The final planned section of Struma from Sandanski to the Kulata border crossing with Greece."),
                importance: Text("Ще завърши коридор IV до гръцката граница и Солун.", "Will complete Corridor IV to the Greek border and Thessaloniki."),
                status: SegmentStatus.Planned,
                lengthKm: 29,
                maxSpeedKph: 140,
                startYear: 2028,
                forecastOpenYear: 2030,
                budgetMillionEur: 95,
                fundingProgram: "National budget",
                contractor: "TBD",
                sourceName: "МРРБ – A3 Struma completion plan",
                sourceUrl: "https://en.wikipedia.org/wiki/Struma_motorway",
                shapePoints:
                [
                    (41.5639, 23.2761, "Sandanski"),
                    (41.4200, 23.3000, "Petrich area"),
                    (41.3380, 23.3670, "Kulata border crossing")
                ],
                milestones:
                [
                    Milestone(2028, "Очаквано начало на строителство след завършване на Кресна", "Expected construction start after Kresna section completion", "warning"),
                    Milestone(2030, "Прогнозно завършване – А3 напълно завършена", "Forecast completion – A3 fully complete", "warning")
                ]),

            // A4-01: Maritsa Motorway – Plovdiv to Turkish border (117 km) – Open
            CreateSegment(
                routeCode: "A4",
                name: "Maritsa Motorway",
                sectionCode: "A4-01",
                displayName: Text("Автомагистрала „Марица"", "Maritsa Motorway"),
                sectionName: Text("Пловдив – Капитан Андреево (граница)", "Plovdiv – Kapitan Andreevo (border)"),
                description: Text("„Марица" свързва Пловдив с турската граница при Капитан Андреево. Открита поетапно 2009–2012, финансирана с ИСПА и КФ.", "Maritsa links Plovdiv to the Turkish border at Kapitan Andreevo. Opened in phases 2009–2012, financed with EU ISPA and Cohesion Fund."),
                importance: Text("Трансевропейски коридор X. Основна ос за търговия между ЕС и Турция.", "Trans-European Corridor X. Primary trade axis between the EU and Turkey."),
                status: SegmentStatus.Open,
                lengthKm: 117,
                maxSpeedKph: 140,
                startYear: 2008,
                budgetMillionEur: 380,
                fundingProgram: "EU ISPA + Cohesion Fund",
                completionPercent: 100,
                sourceName: "Wikipedia – Maritsa motorway",
                sourceUrl: "https://en.wikipedia.org/wiki/Maritsa_motorway",
                shapePoints:
                [
                    (42.1354, 24.7453, "Plovdiv"),
                    (42.0500, 25.3000, "Haskovo area"),
                    (41.8710, 25.7990, "Svilengrad"),
                    (41.7420, 26.3410, "Kapitan Andreevo border")
                ],
                milestones:
                [
                    Milestone(2008, "Стартира строителство на лотовете Пловдив–Хасково", "Construction of Plovdiv–Haskovo lots begins"),
                    Milestone(2009, "Пловдив–Хасково открит", "Plovdiv–Haskovo opened", "success"),
                    Milestone(2012, "Хасково–Капитан Андреево открит – Марица завършена", "Haskovo–Kapitan Andreevo opened – Maritsa complete", "success")
                ]),

            // A5-01: Cherno More (Black Sea) – Planned (103 km)
            CreateSegment(
                routeCode: "A5",
                name: "Cherno More Motorway",
                sectionCode: "A5-01",
                displayName: Text("Автомагистрала „Черно море"", "Cherno More Motorway"),
                sectionName: Text("Варна – Бургас (крайбрежен коридор)", "Varna – Burgas (coastal corridor)"),
                description: Text("Планираната крайбрежна магистрала свързваща двата черноморски центъра. Трасето е одобрено, финансирането се договаря.", "Planned coastal motorway connecting both Black Sea centres. Alignment approved, financing in negotiation."),
                importance: Text("Ще обедини двата черноморски туристически центъра и ще разтовари Е87.", "Will unite both Black Sea tourist hubs and relieve E87."),
                status: SegmentStatus.Planned,
                lengthKm: 103,
                maxSpeedKph: 140,
                startYear: 2027,
                forecastOpenYear: 2035,
                budgetMillionEur: 420,
                fundingProgram: "National budget",
                contractor: "TBD",
                sourceName: "МРРБ – A5 Cherno More motorway plan",
                sourceUrl: "https://en.wikipedia.org/wiki/Cherno_More_motorway",
                shapePoints:
                [
                    (43.2141, 27.9147, "Varna"),
                    (42.9500, 27.7000, "Obzor area"),
                    (42.5048, 27.4626, "Burgas")
                ],
                milestones:
                [
                    Milestone(2022, "ОВОС процедурата е стартирана за крайбрежния коридор", "EIA procedure started for coastal corridor"),
                    Milestone(2027, "Очаквано начало на строителство", "Expected construction start", "warning")
                ]),

            // A6-01: Europe Motorway – Sofia ring to Serbian border (61 km) – Open
            CreateSegment(
                routeCode: "A6",
                name: "Europe Motorway",
                sectionCode: "A6-01",
                displayName: Text("Автомагистрала „Европа"", "Europe Motorway"),
                sectionName: Text("София – Калотина (граница Сърбия)", "Sofia – Kalotina (Serbian border)"),
                description: Text("„Европа" свързва София с ГКПП Калотина на сръбската граница. Открита 2013–2021 с финансиране от CEF и национален бюджет.", "Europe motorway links Sofia to the Kalotina Serbian border crossing. Opened 2013–2021 with CEF and national budget financing."),
                importance: Text("Трансевропейски коридор X. Ключова ос за търговия и туризъм между Балканите и Западна Европа.", "Trans-European Corridor X. Key axis for trade and tourism between the Balkans and Western Europe."),
                status: SegmentStatus.Open,
                lengthKm: 61,
                maxSpeedKph: 140,
                startYear: 2010,
                budgetMillionEur: 340,
                fundingProgram: "CEF + national",
                completionPercent: 100,
                sourceName: "Wikipedia – Europe motorway Bulgaria",
                sourceUrl: "https://en.wikipedia.org/wiki/Motorway_A6_(Bulgaria)",
                shapePoints:
                [
                    (42.6977, 23.3219, "Sofia West"),
                    (42.8290, 23.0740, "Slivnitsa"),
                    (42.9800, 22.8820, "Dragoman"),
                    (43.0130, 22.7850, "Kalotina border crossing")
                ],
                milestones:
                [
                    Milestone(2010, "Строителство на Европа стартира", "Europe motorway construction begins"),
                    Milestone(2013, "София–Сливница открита", "Sofia–Slivnitsa opened", "success"),
                    Milestone(2021, "Сливница–Калотина открита – Европа завършена", "Slivnitsa–Kalotina opened – Europe motorway complete", "success")
                ]),

            // E79-01: Vidin → Montana (99 km) – Open (part of Corridor IV)
            CreateSegment(
                routeCode: "E79",
                name: "Vidin – Montana Expressway",
                sectionCode: "E79-01",
                displayName: Text("Път E79 – Видин – Монтана", "E79 Vidin – Montana"),
                sectionName: Text("Видин – Монтана", "Vidin – Montana"),
                description: Text("Завършеният северен участък на Е79 от Видин до Монтана. Финансиран по CEF и национален бюджет, открит 2020.", "The completed northern section of E79 from Vidin to Montana. CEF and national budget financed, opened 2020."),
                importance: Text("Свързва мост „Дунав мост 2" при Видин с вътрешността. Коридор IV.", "Links Danube Bridge 2 at Vidin to the interior. Corridor IV."),
                status: SegmentStatus.Open,
                lengthKm: 99,
                maxSpeedKph: 120,
                startYear: 2014,
                budgetMillionEur: 195,
                fundingProgram: "CEF + national",
                completionPercent: 100,
                sourceName: "API Bulgaria – E79 Vidin-Montana",
                sourceUrl: "https://en.wikipedia.org/wiki/E79_road_(Bulgaria)",
                shapePoints:
                [
                    (43.9939, 22.8750, "Vidin"),
                    (43.8000, 23.0500, "Vratsa area"),
                    (43.4082, 23.2254, "Montana")
                ],
                milestones:
                [
                    Milestone(2014, "Строителство Видин–Монтана стартира", "Vidin–Montana construction begins"),
                    Milestone(2020, "Видин–Монтана в пълна експлоатация", "Vidin–Montana fully operational", "success")
                ]),

            // E79-02: Montana → Botevgrad (81 km) – UC 43%
            CreateSegment(
                routeCode: "E79",
                name: "Montana – Botevgrad Expressway",
                sectionCode: "E79-02",
                displayName: Text("Път E79 – Монтана – Ботевград", "E79 Montana – Botevgrad"),
                sectionName: Text("Монтана – Ботевград", "Montana – Botevgrad"),
                description: Text("В строеж южен дял на Е79, свързващ Монтана с Ботевград. Ще затвори коридорът Видин–А1. Договор с GP Group / Geomash.", "Under construction southern section of E79 connecting Montana to Botevgrad. Will complete the Vidin–A1 corridor. Contract with GP Group / Geomash."),
                importance: Text("Затваря Коридор IV Видин–А1 Тракия и осигурява алтернативна ос от Румъния.", "Completes Corridor IV Vidin–A1 Trakia and provides alternative axis from Romania."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 81,
                maxSpeedKph: 120,
                startYear: 2020,
                forecastOpenYear: 2027,
                completionPercent: 43,
                budgetMillionEur: 380,
                fundingProgram: "CEF + national",
                contractor: "GP Group / Geomash",
                sourceName: "API Bulgaria – E79 Montana-Botevgrad",
                sourceUrl: "https://en.wikipedia.org/wiki/E79_road_(Bulgaria)",
                shapePoints:
                [
                    (43.4082, 23.2254, "Montana"),
                    (43.2000, 23.5000, "Vratsa area"),
                    (42.9200, 23.7800, "Mezdra area"),
                    (42.9050, 23.7900, "Botevgrad")
                ],
                milestones:
                [
                    Milestone(2020, "Договорът с GP Group / Geomash е подписан", "Contract with GP Group / Geomash signed"),
                    Milestone(2023, "43% завършеност – активно строителство", "43% completion – active construction", "info"),
                    Milestone(2027, "Прогнозно отваряне – коридорът е завършен", "Forecast opening – corridor complete", "warning")
                ]),

            // RVT-01: Ruse → Veliko Tarnovo (67 km) – UC 18%
            CreateSegment(
                routeCode: "RVT",
                name: "Ruse – Veliko Tarnovo Expressway",
                sectionCode: "RVT-01",
                displayName: Text("Скоростен път Русе – Велико Търново", "Ruse – Veliko Tarnovo Expressway"),
                sectionName: Text("Русе – Велико Търново", "Ruse – Veliko Tarnovo"),
                description: Text("Скоростен път от Русе до Велико Търново с финансиране CEF. В строеж от 2021 г. от Trace Group / Geomash JV.", "Expressway from Ruse to Veliko Tarnovo with CEF financing. Under construction since 2021 by Trace Group / Geomash JV."),
                importance: Text("Ще намали времето за пътуване Русе–ВТ от 90 на 45 мин и ще подобри достъпа до Дунав мост.", "Will cut Ruse–VT travel time from 90 to 45 min and improve access to the Danube Bridge."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 67,
                maxSpeedKph: 120,
                startYear: 2021,
                forecastOpenYear: 2028,
                completionPercent: 18,
                budgetMillionEur: 695,
                fundingProgram: "CEF + EU Cohesion Fund",
                contractor: "Trace Group / Geomash JV",
                sourceName: "API Bulgaria – Ruse-VT expressway",
                sourceUrl: "https://en.wikipedia.org/wiki/Ruse%E2%80%93Veliko_Tarnovo_motorway",
                shapePoints:
                [
                    (43.8356, 25.9657, "Ruse"),
                    (43.6000, 25.8200, "Byala"),
                    (43.0770, 25.6173, "Veliko Tarnovo")
                ],
                milestones:
                [
                    Milestone(2019, "CEF финансирането е одобрено", "CEF financing approved"),
                    Milestone(2021, "Строителство стартира – Trace Group / Geomash JV", "Construction begins – Trace Group / Geomash JV"),
                    Milestone(2028, "Прогнозно завършване", "Forecast completion", "warning")
                ]),

            // MB-01: Mezdra → Botevgrad (33 km) – UC 41%
            CreateSegment(
                routeCode: "MB",
                name: "Mezdra – Botevgrad Expressway",
                sectionCode: "MB-01",
                displayName: Text("Скоростен път Мездра – Ботевград", "Mezdra – Botevgrad Expressway"),
                sectionName: Text("Мездра – Ботевград", "Mezdra – Botevgrad"),
                description: Text("Скоростен път по долината на Искър свързващ Мездра с A2 при Ботевград. В строеж с национален бюджет.", "Expressway along the Iskar valley connecting Mezdra to A2 at Botevgrad. Under construction with national budget."),
                importance: Text("Ще осигури бързата северна ос Видин–Ботевград–А1/А2.", "Will provide the fast northern axis Vidin–Botevgrad–A1/A2."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 33,
                maxSpeedKph: 120,
                startYear: 2020,
                forecastOpenYear: 2026,
                completionPercent: 41,
                budgetMillionEur: 185,
                fundingProgram: "National budget",
                sourceName: "API Bulgaria – Mezdra-Botevgrad expressway",
                sourceUrl: "https://bg.wikipedia.org/wiki/%D0%A1%D0%BA%D0%BE%D1%80%D0%BE%D1%81%D1%82%D0%B5%D0%BD_%D0%BF%D1%8A%D1%82_%D0%9C%D0%B5%D0%B7%D0%B4%D1%80%D0%B0_%E2%80%93_%D0%91%D0%BE%D1%82%D0%B5%D0%B2%D0%B3%D1%80%D0%B0%D0%B4",
                shapePoints:
                [
                    (43.1490, 23.7100, "Mezdra"),
                    (43.0200, 23.7500, "Iskar gorge"),
                    (42.9050, 23.7900, "Botevgrad")
                ],
                milestones:
                [
                    Milestone(2020, "Строителство Мездра–Ботевград стартира", "Mezdra–Botevgrad construction begins"),
                    Milestone(2023, "41% завършеност", "41% completion", "info"),
                    Milestone(2026, "Прогнозно завършване", "Forecast completion", "warning")
                ]),

"""

# ─── APPLY INSERTIONS ─────────────────────────────────────────────────────────

# Insertion point 1: Replace the A1-05 comment stub + A3-01 opening comment
# with: full A1-05 + A2 blocks + A3-01 comment
MARKER_A1 = '            // A1-05: Yambol → Karnobat (43 km) – opened 15 July 2013\n// A3-01:'
if MARKER_A1 in content:
    idx = content.find(MARKER_A1)
    # Keep the // A3-01: comment as-is (it's part of our new A3-01 block)
    replacement_prefix = BLOCK_A1_05_AND_A2 + '            // A3-01:'
    content = content[:idx] + replacement_prefix + content[idx + len(MARKER_A1):]
    print(f"Inserted A1-05 + A2 blocks at char {idx}")
else:
    print("ERROR: A1-05 marker not found!")
    print("Content around A1-04 end:")
    idx2 = content.find('A1-05')
    print(repr(content[max(0,idx2-50):idx2+200]))

# Insertion point 2: Add remaining segments before 'return network;'
MARKER_RETURN = '\n\n        return network;'
if MARKER_RETURN in content:
    idx2 = content.find(MARKER_RETURN)
    content = content[:idx2] + '\n\n' + BLOCK_REST.rstrip() + MARKER_RETURN + content[idx2 + len(MARKER_RETURN):]
    print(f"Inserted A3-03 + A4–MB blocks at char {idx2}")
else:
    print("ERROR: return network marker not found!")

# ─── VERIFY ──────────────────────────────────────────────────────────────────
import re
codes = re.findall(r'sectionCode: "([^"]+)"', content)
print(f"\nFinal section codes ({len(codes)}): {codes}")

# ─── SAVE ─────────────────────────────────────────────────────────────────────
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print(f"Saved. {len(content)} chars.")
