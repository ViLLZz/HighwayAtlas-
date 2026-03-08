using System.Text.Json;
using Motorway.Domain;
using Motorway.Engine;

namespace Motorway.Infrastructure;

public static class NationalNetworkSeed
{
    private const string SeedSourceLabel = "Motorway Atlas refined seed";
    private const string DefaultSourceName = "API Bulgaria public road status + OSM geometry cross-check";
    private const string DefaultSourceUrl = "https://www.api.bg/";

    public static HighwayRoute BuildDefault()
    {
        var network = new HighwayRoute("Bulgarian Motorways & Expressways");

        network.AddSegments(
        [
            // A1-01: Sofia → Plovdiv (145 km) – built 1973-1984
            // Historical: first 10 km (Sofia–Novi Han) opened 1978; Plovdiv reached 1984
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-01",
                displayName: Text("Автомагистрала „Тракия”", "Trakia Motorway"),
                sectionName: Text("София – Пловдив", "Sofia – Plovdiv"),
                description: Text("Гръбнакът на южната ос. Строителството е продължило 11 години – от първите 10 km до Нови Хан (1978) до пълното достигане на Пловдив (1984).", "Backbone of the southern axis. Construction took 11 years from the first 10 km to Novi Han (1978) to reaching Plovdiv (1984)."),
                importance: Text("Свързва столицата с Тракия, индустриалните зони и Черноморието.", "Connects the capital with Thrace, industrial zones, and the Black Sea."),
                status: SegmentStatus.Open,
                lengthKm: 132,
                maxSpeedKph: 140,
                startYear: 1973,
                budgetMillionEur: 185,
                fundingProgram: "EBRD + national",
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway construction history",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.6977, 23.3219, "Sofia ring road"),
                    (42.6650, 23.4130, "Novi Han junction"),
                    (42.5480, 23.7200, "Vakarel"),
                    (42.4380, 23.8180, "Ihtiman"),
                    (42.2050, 24.3310, "Pazardzhik"),
                    (42.1432, 24.7531, "Plovdiv-east junction")
                ],
                milestones:
                [
                    Milestone(1973, "Началото на строителството на „Тракия”", "Construction of Trakia begins"),
                    Milestone(1978, "Първите 10 km (София – Нови Хан) в експлоатация", "First 10 km Sofia–Novi Han in service", "success"),
                    Milestone(1984, "Пловдив е достигнат – западният дял е завършен", "Plovdiv reached – western section fully open", "success")
                ]),

            // A1-02: Plovdiv → Stara Zagora (73 km) – partial 1995, full 2007
            // Historical: 32 km EBRD (Plovdiv–Plodovitovo) 1995; EIB Lot 1 full 2007
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-02",
                displayName: Text("Автомагистрала „Тракия”", "Trakia Motorway"),
                sectionName: Text("Пловдив – Стара Загора", "Plovdiv – Stara Zagora"),
                description: Text("Изграден на два етапа: 32 km с ЕБВР финансиране (1995) и финалният участък до Стара Загора с ЕИБ (2007).", "Built in two phases: 32 km EBRD-financed (1995) and the final stretch to Stara Zagora with EIB (2007)."),
                importance: Text("Свързва Пловдивската равнина с тракийската ос и черноморския коридор.", "Links the Plovdiv plain to the Thracian axis and the Black Sea corridor."),
                status: SegmentStatus.Open,
                lengthKm: 76,
                maxSpeedKph: 140,
                startYear: 1993,
                budgetMillionEur: 210,
                fundingProgram: "EU ISPA + EIB",
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway, EBRD 1995 and EIB 2007",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.1432, 24.7531, "Plovdiv-east junction"),
                    (42.1510, 24.9800, "Plodovitovo"),
                    (42.1630, 25.2900, "Orizovo interchange (A4 branch)"),
                    (42.2000, 25.3500, "Chirpan"),
                    (42.4258, 25.6345, "Stara Zagora")
                ],
                milestones:
                [
                    Milestone(1993, "Строителството на ЕБВР участъка стартира", "EBRD-financed section construction begins"),
                    Milestone(1995, "32 km от Пловдив до Плодовитово открити (ЕБВР)", "32 km Plovdiv–Plodovitovo opened (EBRD)", "success"),
                    Milestone(2000, "ЕИБ заем за Лот 1 (Оризово – Стара Загора)", "EIB loan secured for Lot 1 (Orizovo–Stara Zagora)"),
                    Milestone(2007, "Лот 1 в пълна експлоатация – Стара Загора е достигната", "Lot 1 fully operational – Stara Zagora reached", "success")
                ]),

            // A1-03: Karnobat → Burgas (36 km) – opened 2006
            // Historical: EIB Lot 5 opened a year before the central lots were built
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-03",
                displayName: Text("Автомагистрала „Тракия”", "Trakia Motorway"),
                sectionName: Text("Карнобат – Бургас", "Karnobat – Burgas"),
                description: Text("Лот 5 – открит в края на 2006 г., цяла година преди централните лотове. Финансиран от ЕИБ.", "Lot 5 – opened late 2006, a full year before the central lots. EIB-financed."),
                importance: Text("Осигури директен достъп до пристанище Бургас преди цялата магистрала да е завършена.", "Provided direct access to Burgas Port before the full motorway was complete."),
                status: SegmentStatus.Open,
                lengthKm: 35,
                maxSpeedKph: 140,
                startYear: 2003,
                budgetMillionEur: 85,
                fundingProgram: "EIB loan",
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway, Lot 5 opening 2006",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.6510, 26.9790, "Karnobat"),
                    (42.5800, 27.1830, "Aytos"),
                    (42.5048, 27.4626, "Burgas")
                ],
                milestones:
                [
                    Milestone(2003, "Строителството на Лот 5 (Карнобат – Бургас) стартира", "Lot 5 Karnobat–Burgas construction begins"),
                    Milestone(2006, "Лот 5 в експлоатация – директна връзка с пристанище Бургас", "Lot 5 operational – direct access to Burgas Port", "success")
                ]),

            // A1-04: Stara Zagora → Yambol (63 km) – opened 2012
            // Historical: Lots 2 & 3 opened mid-2012 with EU cohesion funding
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-04",
                displayName: Text("Автомагистрала „Тракия”", "Trakia Motorway"),
                sectionName: Text("Стара Загора – Ямбол", "Stara Zagora – Yambol"),
                description: Text("Лотове 2 и 3 – централният тракийски дял, открит в средата на 2012 г. с ЕС кохезионно финансиране.", "Lots 2 and 3 – the central Thracian reach, opened mid-2012 with EU cohesion co-financing."),
                importance: Text("Запълни критичната средна пролука и направи София–Ямбол достъпни за под два часа.", "Closed the critical central gap and made Sofia–Yambol accessible in under two hours."),
                status: SegmentStatus.Open,
                lengthKm: 81,
                maxSpeedKph: 140,
                startYear: 2009,
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway, Lots 2 & 3 opening 2012",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.4258, 25.6345, "Stara Zagora"),
                    (42.4830, 26.0160, "Nova Zagora"),
                    (42.4750, 26.3220, "Yambol-west / Sliven junction"),
                    (42.4900, 26.5600, "Yambol-east junction")
                ],
                milestones:
                [
                    Milestone(2009, "Тръжната процедура за лотове 2, 3 и 4 е открита", "Tender for Lots 2, 3 and 4 launched"),
                    Milestone(2010, "Договорите за строителство са подписани", "Construction contracts signed"),
                    Milestone(2012, "Стара Загора – Ямбол-изток в експлоатация (Лотове 2 и 3)", "Stara Zagora–Yambol-east operational (Lots 2 & 3)", "success")
                ]),

            // A1-05: Yambol → Karnobat (43 km) – opened 15 July 2013
            // Historical: Lot 4 – the very last piece, completing Bulgaria's first full motorway
            CreateSegment(
                routeCode: "A1",
                name: "Trakia Motorway",
                sectionCode: "A1-05",
                displayName: Text("Автомагистрала „Тракия”", "Trakia Motorway"),
                sectionName: Text("Ямбол – Карнобат", "Yambol – Karnobat"),
                description: Text("Лот 4 – последният останал участък, който завърши цялата магистрала. Открит на 15 юли 2013 г. с европейско кохезионно финансиране.", "Lot 4 – the final missing link that completed the entire motorway. Opened 15 July 2013 with EU Cohesion Fund co-financing."),
                importance: Text("Затвори последния пролуп в A1 и направи пълната Черноморска ос реалност.", "Closed the last gap in A1 and made the full Black Sea axis a reality."),
                status: SegmentStatus.Open,
                lengthKm: 34,
                maxSpeedKph: 140,
                startYear: 2010,
                budgetMillionEur: 98,
                fundingProgram: "EU Cohesion Fund",
                completionPercent: 100,
                sourceName: "Wikipedia – Trakia motorway, Lot 4 completion 2013",
                sourceUrl: "https://en.wikipedia.org/wiki/Trakia_motorway",
                shapePoints:
                [
                    (42.4900, 26.5600, "Yambol-east junction"),
                    (42.5710, 26.7700, "Sakar foothills"),
                    (42.6510, 26.9790, "Karnobat")
                ],
                milestones:
                [
                    Milestone(2010, "Договорът за Лот 4 (Ямбол – Карнобат) е подписан", "Contract for Lot 4 (Yambol–Karnobat) signed"),
                    Milestone(2013, "Лот 4 в ekspлоатация – A1 Тракия е завършена", "Lot 4 operational – A1 Trakia completed", "success")
                ]),

            // A2-01: Sofia → Ugarchin (103 km) – Hemus opened in phases 1974–2025, fully open Dec 2025
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-01",
                displayName: Text("Автомагистрала „Хемус”", "Hemus Motorway"),
                sectionName: Text("София – Угърчин", "Sofia – Ugarchin"),
                description: Text("Завършеният западен участък на „Хемус“ от Софийския околовръстен път до временния край при Угърчин. Изграден е поетапно в периода 1974–2025 г. с участие на ИСПА, Кохезионния фонд и национално финансиране.", "The completed western section of Hemus from the Sofia ring road to the temporary exit at Ugarchin. Built in phases 1974–2025: Sofia–Yablanitsa (1999), Yablanitsa–Boaza (2019), Boaza–Dermantsi (October 2025), Dermantsi–Ugarchin (December 2025)."),
                importance: Text("Основна артерия свързваща столицата с централна и северна България.", "Primary artery linking the capital to central and northern Bulgaria."),
                status: SegmentStatus.Open,
                lengthKm: 103,
                maxSpeedKph: 140,
                startYear: 1974,
                budgetMillionEur: 720,
                fundingProgram: "EU Cohesion Fund + ISPA + national",
                completionPercent: 100,
                sourceName: "Wikipedia – Hemus motorway",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (42.6977, 23.3219, "Sofia ring road"),
                    (42.9160, 23.7880, "Botevgrad"),
                    (43.0210, 23.9040, "Pravets"),
                    (43.0150, 24.0890, "Yablanitsa"),
                    (43.1220, 24.4850, "Boaza pass (I-4 junction)"),
                    (43.2010, 24.5370, "Dermantsi"),
                    (43.1960, 24.4110, "Ugarchin (temporary exit)")
                ],
                milestones:
                [
                    Milestone(1974, "Официален старт на строителство на Хемус", "Official groundbreaking of Hemus motorway"),
                    Milestone(1999, "София – Ябланица открита (74.6 km)", "Sofia–Yablanitsa opened (74.6 km)", "success"),
                    Milestone(2019, "Ябланица – Боаза Лот 0 (9.3 km) открит", "Yablanitsa–Boaza Lot 0 (9.3 km) opened", "success"),
                    Milestone(2025, "Боаза–Дерманци (окт 2025) и Дерманци–Угърчин (дек 2025) отворени", "Boaza–Dermantsi (Oct 2025) and Dermantsi–Ugarchin (Dec 2025) opened", "success")
                ]),

            // A2-02: Ugarchin → Letnitsa (69 km) – Under Construction 25%, forecast 2029
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-02",
                displayName: Text("Автомагистрала „Хемус”", "Hemus Motorway"),
                sectionName: Text("Угърчин – Летница / Крушуна", "Ugarchin – Letnitsa / Krushuna"),
                description: Text("Участъкът Угърчин – Летница / Крушуна е в активно строителство по лотове 2–5. Работи се в районите на Ловеч, Плевен и Тетевен с финансиране от Кохезионния фонд и националния бюджет.", "Under active construction from Ugarchin to Letnitsa/Krushuna (Lots 2–5). Works continue in the Pleven, Lovech and Teteven areas. EU Cohesion Fund financing."),
                importance: Text("Ще затвори критичния централен дял и ще намали времето за пътуване София–Варна след цялостното завършване на оста.", "Will close the critical central gap and reduce Sofia–Varna travel time once the full axis is completed."),
                status: SegmentStatus.UnderConstruction,
                lengthKm: 69,
                maxSpeedKph: 140,
                startYear: 2021,
                budgetMillionEur: 480,
                fundingProgram: "EU Cohesion Fund + national",
                forecastOpenYear: 2029,
                completionPercent: 25,
                contractor: "Trace Group / Infra Expert / PORR",
                sourceName: "Wikipedia – Hemus motorway UC sections",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.1960, 24.4110, "Ugarchin / Dermantsi area"),
                    (43.2360, 24.4620, "Lovech plateau west"),
                    (43.2850, 24.5480, "Bahovitsa corridor"),
                    (43.3320, 24.6360, "Lovech east / Bahovitsa junction"),
                    (43.3460, 24.7320, "Pleven–Troyan junction area"),
                    (43.3250, 24.7900, "Krushuna west"),
                    (43.3040, 24.8420, "Krushuna approach"),
                    (43.2900, 24.8700, "Letnitsa / Krushuna area")
                ],
                milestones:
                [
                    Milestone(2021, "Договорите за Лотове 2–5 Угърчин–Летница са подписани", "Contracts for Lots 2–5 Ugarchin–Letnitsa signed"),
                    Milestone(2023, "Активно строителство по всички лотове на участъка", "Active construction on all lots of the section", "info"),
                    Milestone(2029, "Прогнозно отваряне Угърчин–Летница", "Forecast opening Ugarchin–Letnitsa", "warning")
                ]),

            // A2-03: Letnitsa → Polikraishte (94 km) – Planned, design phase 2027, forecast 2032
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-03",
                displayName: Text("Автомагистрала „Хемус”", "Hemus Motorway"),
                sectionName: Text("Летница – Поликраище", "Letnitsa – Polikraishte (VT junction)"),
                description: Text("Планираният централно-северен дял на „Хемус“ между Летница и Поликраище. Подготвят се технически проекти и тръжни процедури за бъдещите лотове с очаквано строителство след 2027 г.", "The planned central section of Hemus from Letnitsa to the Veliko Tarnovo junction at Polikraishte. The route passes north of VT through the Danubian plain. Tender procedures for the upcoming lots are being prepared with EU Cohesion Fund financing."),
                importance: Text("Затваря пролуката между Плевен и Велико Търново по оста София–Варна.", "Closes the gap between Pleven and Veliko Tarnovo on the Sofia–Varna axis."),
                status: SegmentStatus.Planned,
                lengthKm: 94,
                maxSpeedKph: 140,
                startYear: 2027,
                forecastOpenYear: 2032,
                completionPercent: 0,
                budgetMillionEur: 680,
                fundingProgram: "EU Cohesion Fund",
                contractor: "TBD",
                sourceName: "Wikipedia – Hemus motorway planning overview",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.2900, 24.8700, "Letnitsa / Krushuna"),
                    (43.2760, 25.0420, "Suhindol north corridor"),
                    (43.2520, 25.2140, "Pavlikeni west"),
                    (43.2460, 25.3200, "Pavlikeni"),
                    (43.2720, 25.4720, "Daskot west"),
                    (43.3140, 25.6140, "Daskot / VT area"),
                    (43.3600, 25.7200, "Polikraishte (VT junction)")
                ],
                milestones:
                [
                    Milestone(2024, "Технически проекти за Летница–Поликраище в разработка", "Technical designs for Letnitsa–Polikraishte in development", "info"),
                    Milestone(2027, "Очаквано начало на строителство", "Expected construction start", "warning"),
                    Milestone(2032, "Прогнозно завършване Летница–Поликраище", "Forecast completion Letnitsa–Polikraishte", "warning")
                ]),

            // A2-04: Polikraishte → Buhovtsi (45 km) – Planned/UC; Loznitsa–Buhovtsi 12km active
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-04",
                displayName: Text("Автомагистрала „Хемус”", "Hemus Motorway"),
                sectionName: Text("Велико Търново – Шумен", "Polikraishte – Buhovtsi (Targovishte)"),
                description: Text("Планираният изток на Хемус от Велико Търново до Шумен. Тръжните процедури се подготвят; финансирането с КФ на ЕС и CEF е в процес на договаряне.", "The last unbuilt gap on Hemus before the completed Buhovtsi–Varna section. The Loznitsa–Buhovtsi lot (12 km) is under active construction; the remaining Polikraishte–Loznitsa (33 km) is planned."),
                importance: Text("Звено от TEN-T Core Network. Ще намали времето за пътуване до Черно море допълнително с ~40 мин.", "TEN-T Core Network link. Will further cut travel time to the Black Sea by ~40 min."),
                status: SegmentStatus.Planned,
                lengthKm: 45,
                maxSpeedKph: 140,
                startYear: 2025,
                forecastOpenYear: 2033,
                completionPercent: 10,
                budgetMillionEur: 350,
                fundingProgram: "EU Cohesion Fund + CEF",
                contractor: "TBD",
                sourceName: "Wikipedia – Hemus motorway eastern planning",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.3600, 25.7200, "Polikraishte (VT junction)"),
                    (43.3950, 25.9800, "Strazhitsa north corridor"),
                    (43.3820, 26.2200, "Popovo north"),
                    (43.3380, 26.4300, "Loznitsa west"),
                    (43.2920, 26.5250, "Loznitsa east / Targovishte west"),
                    (43.2460, 26.6120, "Buhovtsi / Targovishte")
                ],
                milestones:
                [
                    Milestone(2023, "Лозница–Буховци: строителство започнато", "Loznitsa–Buhovtsi: construction started", "info"),
                    Milestone(2025, "Тръжни процедури за Поликраище–Лозница", "Tender procedures for Polikraishte–Loznitsa", "warning"),
                    Milestone(2033, "Прогнозно завършване – Поликраище–Буховци", "Forecast completion – Polikraishte–Buhovtsi", "warning")
                ]),

            // A2-05: Buhovtsi → Varna (107 km) – Open, includes 1974 historic axis + 2013–2022 extensions
            CreateSegment(
                routeCode: "A2",
                name: "Hemus Motorway",
                sectionCode: "A2-05",
                displayName: Text("Автомагистрала „Хемус”", "Hemus Motorway"),
                sectionName: Text("Буховци – Варна (Шумен – Варна)", "Buhovtsi – Varna (via Shumen)"),
                description: Text("Завършеният източен дял на Хемус от Буховци (Търговище) до Варна. Включва историческата Шумен–Варна ос (1974 г.) и разширенията 2013–2022 г. до Буховци. Открит е поетапно в периода 1974–2022 г.", "The completed eastern section of Hemus from Buhovtsi (Targovishte) to Varna. Includes the historic Shumen–Varna axis (1974) and 2013–2022 extensions to Buhovtsi. Opened in phases 1974–2022."),
                importance: Text("Директна магистрална връзка с пристанище Варна и Черноморието. Обхваща изцяло завършените 107 km от ТЕН-Т ядровата мрежа.", "Direct motorway link to Varna Port and the Black Sea. Covers the fully completed 107 km of TEN-T Core Network."),
                status: SegmentStatus.Open,
                lengthKm: 107,
                maxSpeedKph: 140,
                startYear: 1974,
                budgetMillionEur: 240,
                fundingProgram: "National budget + EIB + EU SF",
                completionPercent: 100,
                sourceName: "Wikipedia – Hemus motorway",
                sourceUrl: "https://en.wikipedia.org/wiki/Hemus_motorway",
                shapePoints:
                [
                    (43.2460, 26.6120, "Buhovtsi / Targovishte"),
                    (43.2870, 26.7820, "Kaspichan corridor"),
                    (43.3130, 26.9380, "Belokopitovo interchange"),
                    (43.2860, 27.3300, "Provadiya corridor"),
                    (43.2141, 27.9147, "Varna")
                ],
                milestones:
                [
                    Milestone(1974, "Шумен–Варна открит за движение", "Shumen–Varna opened to traffic", "success"),
                    Milestone(2005, "Участък при Шумен разширен", "Shumen section extended", "success"),
                    Milestone(2015, "Белокопитово (16.3 km) открит – 2015", "Belokopitovo section (16.3 km) opened 2015", "success"),
                    Milestone(2022, "Буховци–Белокопитово открит октомври 2022 – Буховци–Варна напълно в сервиз", "Buhovtsi–Belokopitovo opened Oct 2022 – Buhovtsi–Varna fully in service", "success")
                ]),

            // A3-01: Sofia → Simitli (107 km) – completed Struma corridor
            // Built in phases 2002–2016; all main lots open.
            CreateSegment(
                routeCode: "A3",
                name: "Struma Motorway",
                sectionCode: "A3-01",
                displayName: Text("Автомагистрала „Струма”", "Struma Motorway"),
                sectionName: Text("София – Симитли", "Sofia – Simitli"),
                description: Text("Завършеният участък по долината на Струма от Перник до Симитли. Основните лотове се пускат поетапно 2009–2016 г. с ИСПА и КФ на ЕС.", "The completed section along the Struma valley from Pernik to Simitli. Major lots opened 2009–2016 with EU ISPA and Cohesion Fund co-financing."),
                importance: Text("Ключова артерия към Благоевград, Югозападен университет и граничния преход. Международен транзитен коридор IV.", "Key artery to Blagoevgrad, South-West University, and the border crossing. International Transit Corridor IV."),
                status: SegmentStatus.Open,
                lengthKm: 105,
                maxSpeedKph: 140,
                startYear: 2002,
                completionPercent: 100,
                budgetMillionEur: 940,
                fundingProgram: "EU ISPA + Cohesion Fund",
                sourceName: "Wikipedia – Struma motorway overview",
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
                displayName: Text("Автомагистрала „Струма”", "Struma Motorway"),
                sectionName: Text("Симитли – Кресненско дефиле (Лот 3.2)", "Simitli – Kresna Gorge (Lot 3.2)"),
                description: Text(
                    "Кресненското дефиле е най-спорният строителен обект в историята на българската пътна инфраструктура. Проектът е спрян от Министерството на околната среда (август 2022 г.) до ново ОВОС и оценка по Натура 2000. Дефилето е Натура 2000 обект с уникално биоразнообразие.",
                    "The Kresna gorge (Lot 3.2) is the most contested infrastructure project in Bulgarian history. Construction was halted by the Environment Ministry in August 2022 pending a new EIA and Natura 2000 assessment. The gorge is a Natura 2000 site with unique biodiversity."),
                importance: Text(
                    "Единственият незавършен участък в Коридор IV до гръцката граница. Спирането на проекта означава, че трафикът продължава по претоварения път I-1 през дефилето.",
                    "The only incomplete section in Corridor IV to the Greek border. The project halt means traffic continues on congested I-1 through the gorge."),
                status: SegmentStatus.Planned,
                lengthKm: 23,
                maxSpeedKph: 140,
                startYear: 2027,
                forecastOpenYear: 2031,
                completionPercent: 0,
                budgetMillionEur: 380,
                fundingProgram: "EU Cohesion Fund + national",
                contractor: "TBD – project suspended pending new EIA",
                sourceName: "Wikipedia – Struma motorway Lot 3.2 Kresna Gorge",
                sourceUrl: "https://en.wikipedia.org/wiki/Struma_motorway",
                shapePoints:
                [
                    (41.8950, 23.0900, "Simitli"),
                    (41.7700, 23.1200, "Kresna gorge north approach"),
                    (41.7220, 23.1350, "Kresna gorge (Lot 3.2 – blocked)"),
                    (41.6980, 23.1520, "Dolna Gradeshnitsa (south end)")
                ],
                milestones:
                [
                    Milestone(1999, "Европейски протести срещу трасето по дефилето", "European objections to the route through the gorge", "warning"),
                    Milestone(2017, "ОВОС одобрен след ревизия; ново трасе в дефилето", "EIA approved after revision; new alignment through the gorge", "info"),
                    Milestone(2022, "Стоп: Министерство на ОС спира Лот 3.2 – необходима нова ОВОС и оценка Натура 2000", "Stop: Environment Ministry halts Lot 3.2 – new EIA and Natura 2000 assessment required", "warning"),
                    Milestone(2031, "Прогнозна дата за цялостно завършване (при одобрен нов проект)", "Forecast completion date (subject to new project approval)", "warning")
                ]),


            // A3-03: Kresna → Kulata border crossing (41 km) – Open
            CreateSegment(
                routeCode: "A3",
                name: "Struma Motorway",
                sectionCode: "A3-03",
                displayName: Text("Автомагистрала „Струма”", "Struma Motorway"),
                sectionName: Text("Сандански – Кулата (граница)", "Kresna – Kulata (Lots 3.3 + 4)"),
                description: Text("Завършеният южен участък на „Струма“ от Кресна до ГКПП Кулата. Лот 4 (Сандански–Кулата) е открит през 2015 г., а Лот 3.3 (Кресна–Сандански) – през декември 2018 г.", "The completed southern section of Struma from Kresna to the Kulata border crossing. Lot 4 (Sandanski–Kulata) opened 20 August 2015; Lot 3.3 (Kresna–Sandanski) completed 17 December 2018."),
                importance: Text("Завършва коридор IV до гръцката граница и Солун.", "Completes Corridor IV to the Greek border and Thessaloniki."),
                status: SegmentStatus.Open,
                lengthKm: 41,
                maxSpeedKph: 140,
                startYear: 2013,
                completionPercent: 100,
                budgetMillionEur: 290,
                fundingProgram: "EU Cohesion Fund + national",
                sourceName: "Wikipedia – Struma motorway southern section",
                sourceUrl: "https://en.wikipedia.org/wiki/Struma_motorway",
                shapePoints:
                [
                    (41.6980, 23.1520, "Dolna Gradeshnitsa (Lot 3.3 start)"),
                    (41.5639, 23.2761, "Sandanski"),
                    (41.4342, 23.2050, "Petrich"),
                    (41.3380, 23.3670, "Kulata border crossing")
                ],
                milestones:
                [
                    Milestone(2013, "Строителство на Лот 3.3 (Кресна–Сандански) и Лот 4 (Сандански–Кулата) стартира", "Construction of Lot 3.3 (Kresna–Sandanski) and Lot 4 (Sandanski–Kulata) begins"),
                    Milestone(2015, "Лот 4 Сандански–Кулата открит на 20 август 2015 г.", "Lot 4 Sandanski–Kulata opened 20 August 2015", "success"),
                    Milestone(2018, "Лот 3.3 Кресна–Сандански завършен (17 декември 2018 г.) – южен А3 напълно отворен", "Lot 3.3 Kresna–Sandanski completed (17 December 2018) – southern A3 fully open", "success")
                ]),

            // A4-01: Maritsa Motorway – Plovdiv to Turkish border (117 km) – Open
            CreateSegment(
                routeCode: "A4",
                name: "Maritsa Motorway",
                sectionCode: "A4-01",
                displayName: Text("Автомагистрала „Марица”", "Maritsa Motorway"),
                sectionName: Text("А1/Оризово – Капитан Андреево (граница)", "A1/Orizovo junction – Kapitan Andreevo (border)"),
                description: Text("„Марица” свързва Пловдив с турската граница при Капитан Андреево. Открита поетапно 2009–2012, финансирана с ИСПА и КФ.", "Maritsa links Plovdiv to the Turkish border at Kapitan Andreevo. Opened in phases 2009–2012, financed with EU ISPA and Cohesion Fund."),
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
                    (42.1630, 25.2900, "Orizovo interchange (A1 junction)"),
                    (42.1430, 25.5700, "Dimitrovgrad area"),
                    (41.9330, 25.7990, "Haskovo"),
                    (41.8710, 25.9700, "Svilengrad"),
                    (41.7420, 26.3410, "Kapitan Andreevo border")
                ],
                milestones:
                [
                    Milestone(2008, "Стартира строителство на лотовете Пловдив–Хасково", "Construction of Plovdiv–Haskovo lots begins"),
                    Milestone(2009, "Пловдив–Хасково открит", "Plovdiv–Haskovo opened", "success"),
                    Milestone(2015, "Хасково–Капитан Андреево открит – Марица завършена (29 октомври 2015)", "Haskovo–Kapitan Andreevo opened – Maritsa complete (29 October 2015)", "success")
                ]),

            // A5-01: Cherno More (Black Sea) – Planned (103 km)
            CreateSegment(
                routeCode: "A5",
                name: "Cherno More Motorway",
                sectionCode: "A5-01",
                displayName: Text("Автомагистрала „Черно море”", "Cherno More Motorway"),
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
                sourceName: "Wikipedia – Cherno More motorway plan",
                sourceUrl: "https://en.wikipedia.org/wiki/Cherno_More_motorway",
                shapePoints:
                [
                    (43.2141, 27.9147, "Varna"),
                    (43.0500, 27.7800, "Byala coast"),
                    (42.8200, 27.7600, "Obzor area"),
                    (42.6600, 27.7100, "Sunny Beach / Nessebar corridor"),
                    (42.5700, 27.5600, "Pomorie"),
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
                displayName: Text("Автомагистрала „Европа”", "Europe Motorway"),
                sectionName: Text("София – Калотина (граница Сърбия)", "Sofia – Kalotina (Serbian border)"),
                description: Text("„Европа” свързва София с ГКПП Калотина на сръбската граница. Открита 2013–2021 с финансиране от CEF и национален бюджет.", "Europe motorway links Sofia to the Kalotina Serbian border crossing. Opened 2013–2021 with CEF and national budget financing."),
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
                importance: Text("Свързва мост „Дунав мост 2” при Видин с вътрешността. Коридор IV.", "Links Danube Bridge 2 at Vidin to the interior. Corridor IV."),
                status: SegmentStatus.Open,
                lengthKm: 99,
                maxSpeedKph: 120,
                startYear: 2014,
                budgetMillionEur: 195,
                fundingProgram: "CEF + national",
                completionPercent: 100,
                sourceName: "Wikipedia – E79 Vidin–Montana overview",
                sourceUrl: "https://en.wikipedia.org/wiki/E79_road_(Bulgaria)",
                shapePoints:
                [
                    (43.9939, 22.8750, "Vidin"),
                    (43.8600, 22.9500, "Vidin south / Gramada area"),
                    (43.7100, 23.0600, "Berkovitsa north approach"),
                    (43.5600, 23.1300, "Berkovitsa / Vratsa junction area"),
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
                sourceName: "Wikipedia – E79 Montana–Botevgrad overview",
                sourceUrl: "https://en.wikipedia.org/wiki/E79_road_(Bulgaria)",
                shapePoints:
                [
                    (43.4082, 23.2254, "Montana"),
                    (43.2100, 23.5530, "Vratsa"),
                    (43.1490, 23.7100, "Mezdra"),
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
                sourceName: "Wikipedia – Ruse–Veliko Tarnovo expressway overview",
                sourceUrl: "https://en.wikipedia.org/wiki/Ruse%E2%80%93Veliko_Tarnovo_motorway",
                shapePoints:
                [
                    (43.8356, 25.9657, "Ruse"),
                    (43.6883, 25.7396, "Byala"),
                    (43.4700, 25.6900, "Pavlikeni area"),
                    (43.2200, 25.6500, "Gorna Oryahovitsa approach"),
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
                sourceName: "Wikipedia – Mezdra–Botevgrad expressway overview",
                sourceUrl: "https://bg.wikipedia.org/wiki/%D0%A1%D0%BA%D0%BE%D1%80%D0%BE%D1%81%D1%82%D0%B5%D0%BD_%D0%BF%D1%8A%D1%82_%D0%9C%D0%B5%D0%B7%D0%B4%D1%80%D0%B0_%E2%80%93_%D0%91%D0%BE%D1%82%D0%B5%D0%B2%D0%B3%D1%80%D0%B0%D0%B4",
                shapePoints:
                [
                    (43.1490, 23.7100, "Mezdra"),
                    (43.0900, 23.7300, "Lakatnik / Iskar gorge north"),
                    (43.0450, 23.7600, "Iskar gorge south – Ledenika area"),
                    (42.9050, 23.7900, "Botevgrad")
                ],
                milestones:
                [
                    Milestone(2020, "Строителство Мездра–Ботевград стартира", "Mezdra–Botevgrad construction begins"),
                    Milestone(2023, "41% завършеност", "41% completion", "info"),
                    Milestone(2026, "Прогнозно завършване", "Forecast completion", "warning")
                ]),

        ]);

        return network;
    }

    private static RouteSegment CreateSegment(
        string routeCode,
        string name,
        string sectionCode,
        LocalizedText displayName,
        LocalizedText sectionName,
        LocalizedText description,
        LocalizedText importance,
        SegmentStatus status,
        double lengthKm,
        int maxSpeedKph,
        (double Latitude, double Longitude, string Name)[] shapePoints,
        int? startYear = null,
        int? forecastOpenYear = null,
        double? completionPercent = null,
        double? budgetMillionEur = null,
        string? fundingProgram = null,
        string? contractor = null,
        string? sourceName = null,
        string? sourceUrl = null,
        ProjectMilestone[]? milestones = null)
    {
        var shape = Shape(shapePoints, lengthKm);
        var start = shape.First();
        var end = shape.Last();

        return new RouteSegment
        {
            RouteCode = routeCode,
            Name = name,
            SectionCode = sectionCode,
            DisplayName = displayName,
            SectionName = sectionName,
            Description = description,
            StrategicImportance = importance,
            Start = start,
            End = end,
            Status = status,
            LengthKm = lengthKm,
            MaxSpeedKph = maxSpeedKph,
            StartYear = startYear,
            ForecastOpenYear = forecastOpenYear,
            CompletionPercentOverride = completionPercent,
            BudgetMillionEur = budgetMillionEur,
            FundingProgram = fundingProgram,
            Contractor = contractor,
            SourceName = string.IsNullOrWhiteSpace(sourceName) || string.Equals(sourceName, SeedSourceLabel, StringComparison.OrdinalIgnoreCase)
                ? DefaultSourceName
                : sourceName,
            SourceUrl = string.IsNullOrWhiteSpace(sourceUrl) ? DefaultSourceUrl : sourceUrl,
            Milestones = milestones ?? [],
            Shape = shape
        };
    }

    private static RoutePoint[] Shape((double Latitude, double Longitude, string Name)[] points, double totalLengthKm)
    {
        if (points.Length == 0) return [];
        if (points.Length == 1) return [new RoutePoint(points[0].Latitude, points[0].Longitude, points[0].Name, totalLengthKm)];

        // Compute Haversine cumulative distances between consecutive waypoints
        var rawKm = new double[points.Length];
        rawKm[0] = 0;
        for (var i = 1; i < points.Length; i++)
        {
            var prev = new RoutePoint(points[i - 1].Latitude, points[i - 1].Longitude, points[i - 1].Name);
            var curr = new RoutePoint(points[i].Latitude, points[i].Longitude, points[i].Name);
            rawKm[i] = rawKm[i - 1] + GeoMath.HaversineKm(prev, curr);
        }

        // Scale raw distances to match the authoritative totalLengthKm
        var scale = rawKm[points.Length - 1] > 0 ? totalLengthKm / rawKm[points.Length - 1] : 1.0;

        return points
            .Select((point, index) => new RoutePoint(
                point.Latitude,
                point.Longitude,
                point.Name,
                Math.Round(rawKm[index] * scale, 1)))
            .ToArray();
    }

    private static LocalizedText Text(string bg, string en) => new(bg, en);

    private static ProjectMilestone Milestone(int year, string bg, string en, string state = "info")
        => new(year, Text(bg, en), state);
}

public static class GeoJsonExporter
{
    public static string Export(HighwayRoute route)
    {
        var features = route.Segments.Select(segment => new
        {
            type = "Feature",
            properties = new
            {
                routeCode = segment.RouteCode,
                name = segment.Name,
                sectionCode = segment.SectionCode,
                nameBg = segment.EffectiveDisplayName.Bg,
                nameEn = segment.EffectiveDisplayName.En,
                sectionBg = segment.EffectiveSectionName.Bg,
                sectionEn = segment.EffectiveSectionName.En,
                status = segment.Status.ToString(),
                lengthKm = segment.LengthKm,
                maxSpeedKph = segment.MaxSpeedKph,
                completionPercent = segment.EffectiveCompletionPercent
            },
            geometry = new
            {
                type = "LineString",
                coordinates = segment.Shape.Select(point => new[] { point.Longitude, point.Latitude }).ToArray()
            }
        });

        var featureCollection = new
        {
            type = "FeatureCollection",
            name = route.Name,
            features = features.ToArray()
        };

        return JsonSerializer.Serialize(featureCollection, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}

public static class GeoJsonImport
{
    public static IReadOnlyList<IReadOnlyList<RoutePoint>> ReadLineStrings(string geoJsonContent)
    {
        using var document = JsonDocument.Parse(geoJsonContent);
        var root = document.RootElement;
        var results = new List<IReadOnlyList<RoutePoint>>();

        if (!root.TryGetProperty("features", out var featuresElement))
        {
            return results;
        }

        foreach (var feature in featuresElement.EnumerateArray())
        {
            if (!feature.TryGetProperty("geometry", out var geometry))
            {
                continue;
            }

            var type = geometry.GetProperty("type").GetString();

            if (string.Equals(type, "LineString", StringComparison.OrdinalIgnoreCase))
            {
                results.Add(ReadLineStringCoordinates(geometry.GetProperty("coordinates")));
            }
            else if (string.Equals(type, "MultiLineString", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var lineString in geometry.GetProperty("coordinates").EnumerateArray())
                {
                    results.Add(ReadLineStringCoordinates(lineString));
                }
            }
        }

        return results;
    }

    private static IReadOnlyList<RoutePoint> ReadLineStringCoordinates(JsonElement coordinates)
    {
        var points = new List<RoutePoint>();

        foreach (var coordinate in coordinates.EnumerateArray())
        {
            if (coordinate.GetArrayLength() < 2)
            {
                continue;
            }

            var longitude = coordinate[0].GetDouble();
            var latitude = coordinate[1].GetDouble();
            points.Add(new RoutePoint(latitude, longitude));
        }

        return points;
    }
}
