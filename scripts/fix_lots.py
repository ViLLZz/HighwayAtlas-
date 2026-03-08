#!/usr/bin/env python3
"""Fix A2 lot km ranges and lot data in HtmlAtlasExporter.cs"""

filepath = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/HtmlAtlasExporter.cs"
with open(filepath, encoding='utf-8') as f:
    content = f.read()

start_marker = '    private static IReadOnlyList<LotDescriptor> BuildLots('
end_marker = '        _ => BuildGeneratedLots(segment)\n    };'

idx_start = content.find(start_marker)
idx_end = content.find(end_marker)

if idx_start == -1 or idx_end == -1:
    print("ERROR: markers not found")
    exit(1)

idx_end_full = idx_end + len(end_marker)

new_block = '''    private static IReadOnlyList<LotDescriptor> BuildLots(RouteSegment segment) => (segment.SectionCode ?? string.Empty).ToUpperInvariant() switch
    {
        "A2-01" =>
        [
            Lot("Лот 1А", "София \u2013 Яна", "Sofia \u2013 Yana", SegmentStatus.Open, 0, 18, 100, 2020, 92, noteBg: "Западна входна отсечка; включва сервизния обход към р. Искър.", noteEn: "Western entry section along the Iskar river approach."),
            Lot("Лот 1Б", "Яна \u2013 Ботевград", "Yana \u2013 Botevgrad", SegmentStatus.Open, 18, 62, 100, 2021, 250, noteBg: "Включва тунел \u201eПравец\u201c и Ботевград; ИСПА финансиране 2001\u20132007 г.", noteEn: "Includes Pravets tunnel and Botevgrad interchange; ISPA co-financing 2001\u20132007."),
            Lot("Лот 0", "Ботевград \u2013 Угърчин", "Botevgrad \u2013 Ugarchin", SegmentStatus.Open, 62, 103, 100, 2025, 200, noteBg: "Последно въведен: Ябланица\u2013Боаза (2019), Боаза\u2013Дерманци (окт 2025), Дерманци\u2013Угърчин (дек 2025).", noteEn: "Last commissioned: Yablanitsa\u2013Boaza (2019), Boaza\u2013Dermantsi (Oct 2025), Dermantsi\u2013Ugarchin (Dec 2025).")
        ],
        "A2-02" =>
        [
            Lot("Лот 2А", "Угърчин \u2013 Каленик", "Ugarchin \u2013 Kalenik", SegmentStatus.UnderConstruction, 0, 34, 35, 2028, 380, contractor: "Trace Group / PORR", noteBg: "Активно строителство Лотове 2\u20133; КФ финансиране; прогноза 2028 г.", noteEn: "Active construction Lots 2\u20133; EU Cohesion Fund; forecast 2028."),
            Lot("Лот 2Б", "Каленик \u2013 Летница", "Kalenik \u2013 Letnitsa", SegmentStatus.UnderConstruction, 34, 69, 18, 2029, 310, contractor: "Infra Expert / Hidrostroy", noteBg: "По-нисък дял готовност; сервизен коридор преди Летница/Крушуна.", noteEn: "Lower completion rate; service corridor before Letnitsa/Krushuna.")
        ],
        "A2-03" =>
        [
            Lot("Лот 3А", "Летница \u2013 Горна Оряховица", "Letnitsa \u2013 Gorna Oryahovitsa", SegmentStatus.Planned, 0, 47, 0, 2032, 450, noteBg: "Планиран; технически проект в подготовка; финансиране от КФ на ЕС.", noteEn: "Planned; technical design being prepared; EU Cohesion Fund financing."),
            Lot("Лот 3Б", "Горна Оряховица \u2013 Поликраище", "Gorna Oryahovitsa \u2013 Polikraishte", SegmentStatus.Planned, 47, 94, 0, 2032, 480, noteBg: "Завършва при стратегическия VT-шосеен възел Поликраище; очаква тръжна процедура.", noteEn: "Terminates at the strategic VT road junction; awaits tender procedure.")
        ],
        "A2-04" =>
        [
            Lot("Лот 4А", "Поликраище \u2013 Лозница", "Polikraishte \u2013 Loznitsa", SegmentStatus.Planned, 0, 33, 0, 2033, 230, noteBg: "Планиран; тръжна процедура в подготовка по CEF и КФ схема.", noteEn: "Planned; tender being prepared under CEF and Cohesion Fund scheme."),
            Lot("Лот 4Б", "Лозница \u2013 Буховци", "Loznitsa \u2013 Buhovtsi", SegmentStatus.UnderConstruction, 33, 45, 30, 2026, 120, noteBg: "Лозница\u2013Буховци (12 km): активно строителство, финален стратегически клин преди Варненската ос.", noteEn: "Loznitsa\u2013Buhovtsi (12 km): active construction, final strategic wedge before the Varna axis.")
        ],
        "A2-05" =>
        [
            Lot("Лот 5А", "Буховци \u2013 Белокопитово", "Buhovtsi \u2013 Belokopitovo", SegmentStatus.Open, 0, 28, 100, 2022, 175, noteBg: "Открит окт 2022; затваря последния незавършен дял към Шумен.", noteEn: "Opened Oct 2022; closed the last unfinished gap toward Shumen."),
            Lot("Лот 5Б", "Шумен \u2013 Провадия", "Shumen \u2013 Provadiya", SegmentStatus.Open, 28, 67, 100, 2005, 190, noteBg: "Историческа ос; открит 1974 г., разширен 2005 г.; висок трафик.", noteEn: "Historic axis; opened 1974, widened 2005; high traffic volume."),
            Lot("Лот 5В", "Провадия \u2013 Варна", "Provadiya \u2013 Varna", SegmentStatus.Open, 67, 107, 100, 2005, 210, noteBg: "Крайният приморски клин с директна магистрална връзка към Варненското пристанище.", noteEn: "Terminal coastal segment with direct motorway access to Varna Port.")
        ],
        _ => BuildGeneratedLots(segment)
    };'''

new_content = content[:idx_start] + new_block + content[idx_end_full:]
with open(filepath, 'w', encoding='utf-8') as f:
    f.write(new_content)

print(f"SUCCESS: replaced block ({idx_end_full - idx_start} -> {len(new_block)} chars)")

# Verify
with open(filepath, encoding='utf-8') as f:
    check = f.read()

checks = [
    ('A2-01 lot 0 extended to 103', '62, 103' in check),
    ('A2-02 new UC lots present', 'Ugarchin \u2013 Kalenik' in check),
    ('A2-02 old lot removed', 'Boaza \u2013 Dermantsi interchange' not in check),
    ('A2-03 Planned lots', 'Letnitsa \u2013 Gorna Oryahovitsa' in check),
    ('A2-03 old UC lots removed', 'Dermantsi interchange \u2013 Ugarchin' not in check),
    ('A2-04 two new lots', 'Polikraishte \u2013 Loznitsa' in check and 'Loznitsa \u2013 Buhovtsi' in check),
    ('A2-04 old lots removed', 'Shumen west' not in check),
    ('A2-05 three lots covering 0-107', '67, 107' in check and 'Provadiya \u2013 Varna' in check),
    ('A2-05 old lots removed', '39, 73' not in check),
]

all_ok = True
for name, result in checks:
    status = 'OK' if result else 'FAIL'
    if not result:
        all_ok = False
    print(f"  {status}: {name}")

print("All checks passed!" if all_ok else "Some checks FAILED!")
