namespace Motorway.Domain;

public sealed record LocalizedText(string Bg, string En)
{
    public string Get(string language) => string.Equals(language, "bg", StringComparison.OrdinalIgnoreCase) ? Bg : En;
}

public sealed record ProjectMilestone(int Year, LocalizedText Label, string State = "info");

public enum SegmentStatus
{
    Open,
    UnderConstruction,
    Planned,
    Closed
}

public sealed record RoutePoint(
    double Latitude,
    double Longitude,
    string Name = "",
    double CumulativeKm = 0,
    double? ElevationMeters = null,
    long? OsmNodeId = null,
    string? Surface = null);

public sealed record RouteSegment
{
    public required string RouteCode { get; init; }
    public required string Name { get; init; }
    public required RoutePoint Start { get; init; }
    public required RoutePoint End { get; init; }
    public required SegmentStatus Status { get; init; }
    public required double LengthKm { get; init; }
    public int? MaxSpeedKph { get; init; }
    public string? SectionCode { get; init; }
    public LocalizedText? DisplayName { get; init; }
    public LocalizedText? SectionName { get; init; }
    public LocalizedText? Description { get; init; }
    public LocalizedText? StrategicImportance { get; init; }
    public int? StartYear { get; init; }
    public int? ForecastOpenYear { get; init; }
    public double? CompletionPercentOverride { get; init; }
    public double? BudgetMillionEur { get; init; }
    public string? FundingProgram { get; init; }
    public string? Contractor { get; init; }
    public string? SourceName { get; init; }
    public string? SourceUrl { get; init; }
    public IReadOnlyList<ProjectMilestone> Milestones { get; init; } = [];
    public IReadOnlyList<RoutePoint> Shape { get; init; } = [];

    public LocalizedText EffectiveDisplayName => DisplayName ?? new LocalizedText(Name, Name);

    public LocalizedText EffectiveSectionName => SectionName ?? EffectiveDisplayName;

    public double EffectiveCompletionPercent => CompletionPercentOverride ?? Status switch
    {
        SegmentStatus.Open => 100,
        SegmentStatus.Closed => 0,
        _ => 0
    };
}

public sealed class HighwayRoute
{
    private readonly List<RouteSegment> _segments = [];

    public HighwayRoute(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public IReadOnlyList<RouteSegment> Segments => _segments;

    public double TotalKm => _segments.Sum(segment => segment.LengthKm);

    public double OpenKm => _segments
        .Where(segment => segment.Status == SegmentStatus.Open)
        .Sum(segment => segment.LengthKm);

    public double BudgetMillionEur => _segments.Sum(segment => segment.BudgetMillionEur ?? 0);

    public double CompletionPercent => TotalKm == 0 ? 0 : OpenKm / TotalKm * 100.0;

    public IReadOnlyCollection<string> RouteCodes => _segments
        .Select(segment => segment.RouteCode)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(code => code, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public void AddSegment(RouteSegment segment) => _segments.Add(segment);

    public void AddSegments(IEnumerable<RouteSegment> segments) => _segments.AddRange(segments);

    public IReadOnlyDictionary<SegmentStatus, double> GetLengthByStatus() => Enum
        .GetValues<SegmentStatus>()
        .ToDictionary(
            status => status,
            status => _segments.Where(segment => segment.Status == status).Sum(segment => segment.LengthKm));
}
