using Motorway.Domain;

namespace Motorway.Engine;

public static class GeoMath
{
    private const double EarthRadiusKm = 6371.0088;

    public static double HaversineKm(RoutePoint start, RoutePoint end)
    {
        var dLat = DegreesToRadians(end.Latitude - start.Latitude);
        var dLon = DegreesToRadians(end.Longitude - start.Longitude);
        var lat1 = DegreesToRadians(start.Latitude);
        var lat2 = DegreesToRadians(end.Latitude);

        var a = Math.Pow(Math.Sin(dLat / 2), 2)
                + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusKm * c;
    }

    public static double BearingDegrees(RoutePoint start, RoutePoint end)
    {
        var lat1 = DegreesToRadians(start.Latitude);
        var lat2 = DegreesToRadians(end.Latitude);
        var dLon = DegreesToRadians(end.Longitude - start.Longitude);

        var y = Math.Sin(dLon) * Math.Cos(lat2);
        var x = Math.Cos(lat1) * Math.Sin(lat2)
                - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

        return (RadiansToDegrees(Math.Atan2(y, x)) + 360) % 360;
    }

    public static IReadOnlyList<RoutePoint> SmoothPolyline(IReadOnlyList<RoutePoint> points, int iterations = 2)
    {
        if (points.Count < 3) return points;
        var current = points;
        for (var pass = 0; pass < iterations; pass++)
        {
            var next = new List<RoutePoint>(current.Count * 2) { current[0] };
            for (var i = 0; i < current.Count - 1; i++)
            {
                var p0 = current[i];
                var p1 = current[i + 1];
                next.Add(new RoutePoint(
                    0.75 * p0.Latitude + 0.25 * p1.Latitude,
                    0.75 * p0.Longitude + 0.25 * p1.Longitude));
                next.Add(new RoutePoint(
                    0.25 * p0.Latitude + 0.75 * p1.Latitude,
                    0.25 * p0.Longitude + 0.75 * p1.Longitude));
            }
            next.Add(current[^1]);
            current = next;
        }
        return current;
    }

    public static (double MinLat, double MaxLat, double MinLon, double MaxLon) BoundingBox(IEnumerable<RoutePoint> points)
    {
        double minLat = double.MaxValue, maxLat = double.MinValue;
        double minLon = double.MaxValue, maxLon = double.MinValue;
        foreach (var p in points)
        {
            if (p.Latitude < minLat) minLat = p.Latitude;
            if (p.Latitude > maxLat) maxLat = p.Latitude;
            if (p.Longitude < minLon) minLon = p.Longitude;
            if (p.Longitude > maxLon) maxLon = p.Longitude;
        }
        return (minLat, maxLat, minLon, maxLon);
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

    private static double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;
}

public sealed record NetworkStats(
    int SegmentCount,
    int RouteCount,
    double TotalKm,
    double OpenKm,
    double ConstructionKm,
    double PlannedKm,
    double CompletionPercent);

public sealed record SegmentGeometryDiagnostics(
    string RouteCode,
    string SectionCode,
    int PointCount,
    double AverageSpanKm,
    double LongestSpanKm,
    double MaxBearingDelta,
    int SharpTurnCount,
    bool IsSparseForLength);

public sealed record NetworkGeometryDiagnostics(
    int SegmentCount,
    int SparseSegmentCount,
    int SharpTurnSegmentCount,
    double MaxObservedSpanKm,
    IReadOnlyList<SegmentGeometryDiagnostics> Segments);

public sealed record SegmentContinuityDiagnostics(
    string RouteCode,
    string FromSectionCode,
    string ToSectionCode,
    double EndpointGapKm,
    double BearingDelta,
    string Severity);

public sealed record NetworkContinuityDiagnostics(
    int RouteCount,
    int TransitionCount,
    int ReviewTransitionCount,
    int BrokenTransitionCount,
    double MaxEndpointGapKm,
    IReadOnlyList<SegmentContinuityDiagnostics> Transitions);

public sealed record SegmentProvenanceDiagnostics(
    string RouteCode,
    string SectionCode,
    string EvidenceGrade,
    string OfficialSourceKind,
    bool HasOfficialSource,
    bool HasSecondarySource,
    string? OfficialHost,
    string? SecondaryHost);

public sealed record NetworkProvenanceDiagnostics(
    int SegmentCount,
    int OfficialSourceCount,
    int RouteSpecificOfficialCount,
    int NetworkWideOfficialCount,
    int SecondaryNarrativeCount,
    int UnattributedSegmentCount,
    IReadOnlyList<SegmentProvenanceDiagnostics> Segments);

public sealed class RouteEngine
{
    public NetworkStats CalculateStats(HighwayRoute route)
    {
        var lengthsByStatus = route.GetLengthByStatus();

        return new NetworkStats(
            route.Segments.Count,
            route.RouteCodes.Count,
            route.TotalKm,
            route.OpenKm,
            lengthsByStatus.GetValueOrDefault(SegmentStatus.UnderConstruction),
            lengthsByStatus.GetValueOrDefault(SegmentStatus.Planned),
            route.CompletionPercent);
    }

    public NetworkGeometryDiagnostics AnalyzeGeometry(HighwayRoute route)
    {
        var diagnostics = route.Segments
            .Select(AnalyzeSegmentGeometry)
            .ToArray();

        return new NetworkGeometryDiagnostics(
            diagnostics.Length,
            diagnostics.Count(item => item.IsSparseForLength),
            diagnostics.Count(item => item.SharpTurnCount > 0),
            diagnostics.Length == 0 ? 0 : diagnostics.Max(item => item.LongestSpanKm),
            diagnostics);
    }

    public NetworkContinuityDiagnostics AnalyzeContinuity(HighwayRoute route)
    {
        var transitions = route.Segments
            .GroupBy(segment => segment.RouteCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .SelectMany(group => AnalyzeRouteContinuity(group.Key, group.OrderBy(item => item.SectionCode, StringComparer.OrdinalIgnoreCase).ToArray()))
            .ToArray();

        return new NetworkContinuityDiagnostics(
            route.RouteCodes.Count,
            transitions.Length,
            transitions.Count(item => string.Equals(item.Severity, "review", StringComparison.OrdinalIgnoreCase)),
            transitions.Count(item => string.Equals(item.Severity, "broken", StringComparison.OrdinalIgnoreCase)),
            transitions.Length == 0 ? 0 : transitions.Max(item => item.EndpointGapKm),
            transitions);
    }

    public NetworkProvenanceDiagnostics AnalyzeProvenance(HighwayRoute route)
    {
        var diagnostics = route.Segments
            .Select(segment =>
            {
                var officialHost = TryGetHost(segment.OfficialSourceUrl);
                var secondaryHost = TryGetHost(segment.SecondarySourceUrl);
                var officialKind = string.IsNullOrWhiteSpace(segment.OfficialSourceKind) ? "none" : segment.OfficialSourceKind;

                return new SegmentProvenanceDiagnostics(
                    segment.RouteCode,
                    segment.SectionCode ?? segment.RouteCode,
                    ClassifyEvidenceGrade(segment),
                    officialKind,
                    segment.HasOfficialSource,
                    segment.HasSecondarySource,
                    officialHost,
                    secondaryHost);
            })
            .ToArray();

        return new NetworkProvenanceDiagnostics(
            diagnostics.Length,
            diagnostics.Count(item => item.HasOfficialSource),
            diagnostics.Count(item => item.HasOfficialSource && string.Equals(item.OfficialSourceKind, "route-specific", StringComparison.OrdinalIgnoreCase)),
            diagnostics.Count(item => item.HasOfficialSource && string.Equals(item.OfficialSourceKind, "network-wide", StringComparison.OrdinalIgnoreCase)),
            diagnostics.Count(item => item.HasSecondarySource),
            diagnostics.Count(item => string.Equals(item.EvidenceGrade, "unattributed", StringComparison.OrdinalIgnoreCase)),
            diagnostics);
    }

    public TimeSpan EstimateTravelTime(IEnumerable<RouteSegment> segments, int fallbackSpeedKph = 90)
    {
        var hours = segments.Sum(segment =>
        {
            var speed = segment.MaxSpeedKph.GetValueOrDefault(fallbackSpeedKph);
            return speed <= 0 ? 0 : segment.LengthKm / speed;
        });

        return TimeSpan.FromHours(hours);
    }

    public (double MinLat, double MaxLat, double MinLon, double MaxLon) CalculateRouteBounds(HighwayRoute route)
    {
        var allPoints = route.Segments
            .SelectMany(s => s.Shape.Concat(new[] { s.Start, s.End }));
        return GeoMath.BoundingBox(allPoints);
    }

    private static SegmentGeometryDiagnostics AnalyzeSegmentGeometry(RouteSegment segment)
    {
        if (segment.Shape.Count < 2)
        {
            return new SegmentGeometryDiagnostics(
                segment.RouteCode,
                segment.SectionCode ?? segment.RouteCode,
                segment.Shape.Count,
                0,
                0,
                0,
                0,
                segment.LengthKm >= 25);
        }

        var spans = new List<double>(segment.Shape.Count - 1);
        var bearings = new List<double>(segment.Shape.Count - 1);

        for (var index = 1; index < segment.Shape.Count; index++)
        {
            var start = segment.Shape[index - 1];
            var end = segment.Shape[index];
            spans.Add(GeoMath.HaversineKm(start, end));
            bearings.Add(GeoMath.BearingDegrees(start, end));
        }

        var maxBearingDelta = 0d;
        var sharpTurnCount = 0;

        for (var index = 1; index < bearings.Count; index++)
        {
            var delta = BearingDelta(bearings[index - 1], bearings[index]);
            maxBearingDelta = Math.Max(maxBearingDelta, delta);
            if (delta >= 40)
            {
                sharpTurnCount += 1;
            }
        }

        var averageSpanKm = spans.Average();
        var longestSpanKm = spans.Max();
        var isSparseForLength = segment.LengthKm >= 30 && (segment.Shape.Count < 6 || longestSpanKm > 15);

        return new SegmentGeometryDiagnostics(
            segment.RouteCode,
            segment.SectionCode ?? segment.RouteCode,
            segment.Shape.Count,
            Math.Round(averageSpanKm, 2),
            Math.Round(longestSpanKm, 2),
            Math.Round(maxBearingDelta, 1),
            sharpTurnCount,
            isSparseForLength);
    }

    private static double BearingDelta(double left, double right)
    {
        var delta = Math.Abs(left - right) % 360;
        return delta > 180 ? 360 - delta : delta;
    }

    private static IEnumerable<SegmentContinuityDiagnostics> AnalyzeRouteContinuity(string routeCode, IReadOnlyList<RouteSegment> segments)
    {
        for (var index = 1; index < segments.Count; index++)
        {
            var previous = segments[index - 1];
            var current = segments[index];
            var gapKm = GeoMath.HaversineKm(previous.End, current.Start);
            var previousBearing = InferTerminalBearing(previous, fromEnd: true);
            var currentBearing = InferTerminalBearing(current, fromEnd: false);
            var bearingDelta = previousBearing.HasValue && currentBearing.HasValue
                ? BearingDelta(previousBearing.Value, currentBearing.Value)
                : 0;

            yield return new SegmentContinuityDiagnostics(
                routeCode,
                previous.SectionCode ?? previous.RouteCode,
                current.SectionCode ?? current.RouteCode,
                Math.Round(gapKm, 2),
                Math.Round(bearingDelta, 1),
                ClassifyContinuitySeverity(gapKm, bearingDelta));
        }
    }

    private static double? InferTerminalBearing(RouteSegment segment, bool fromEnd)
    {
        if (segment.Shape.Count >= 2)
        {
            return fromEnd
                ? GeoMath.BearingDegrees(segment.Shape[^2], segment.Shape[^1])
                : GeoMath.BearingDegrees(segment.Shape[0], segment.Shape[1]);
        }

        if (!string.IsNullOrWhiteSpace(segment.Start.Name) || !string.IsNullOrWhiteSpace(segment.End.Name))
        {
            return GeoMath.BearingDegrees(segment.Start, segment.End);
        }

        return null;
    }

    private static string ClassifyContinuitySeverity(double gapKm, double bearingDelta)
    {
        if (gapKm >= 18 || bearingDelta >= 85)
        {
            return "broken";
        }

        if (gapKm >= 6 || bearingDelta >= 45)
        {
            return "review";
        }

        return "healthy";
    }

    private static string ClassifyEvidenceGrade(RouteSegment segment)
    {
        if (!segment.HasOfficialSource && !segment.HasSecondarySource)
        {
            return "unattributed";
        }

        if (!segment.HasOfficialSource && segment.HasSecondarySource)
        {
            return "secondary-only";
        }

        if (string.Equals(segment.OfficialSourceKind, "route-specific", StringComparison.OrdinalIgnoreCase))
        {
            return segment.HasSecondarySource ? "official-route-plus-secondary" : "official-route";
        }

        return segment.HasSecondarySource ? "official-network-plus-secondary" : "official-network";
    }

    private static string? TryGetHost(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri.Host : null;
}

public static class OsmDataValidator
{
    private const double BulgariaMinLat = 41.2;
    private const double BulgariaMaxLat = 44.3;
    private const double BulgariaMinLon = 22.3;
    private const double BulgariaMaxLon = 28.8;

    public static IReadOnlyList<string> ValidateBulgariaBounds(HighwayRoute route)
    {
        var warnings = new List<string>();

        foreach (var segment in route.Segments)
        {
            ValidatePoint(route.Name, segment.RouteCode, segment.Start, warnings);
            ValidatePoint(route.Name, segment.RouteCode, segment.End, warnings);
        }

        return warnings;
    }

    private static void ValidatePoint(string routeName, string routeCode, RoutePoint point, ICollection<string> warnings)
    {
        var isInside = point.Latitude >= BulgariaMinLat
                       && point.Latitude <= BulgariaMaxLat
                       && point.Longitude >= BulgariaMinLon
                       && point.Longitude <= BulgariaMaxLon;

        if (!isInside)
        {
            warnings.Add($"{routeName} / {routeCode}: point '{point.Name}' is outside Bulgaria bounds.");
        }
    }
}

public static class NetworkConsistencyValidator
{
    public static IReadOnlyList<string> Validate(HighwayRoute route)
    {
        var warnings = new List<string>();

        foreach (var segment in route.Segments)
        {
            if (segment.EffectiveCompletionPercent is < 0 or > 100)
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: completion percent is outside 0-100.");
            }

            if (segment.StartYear is int startYear && segment.ForecastOpenYear is int forecastYear && forecastYear < startYear)
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: forecast year is earlier than start year.");
            }

            if (segment.Status == SegmentStatus.Open && !segment.Milestones.Any(item => item.State == "success"))
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: open segment has no success milestone.");
            }

            if (!string.IsNullOrWhiteSpace(segment.OfficialSourceName)
                && Uri.TryCreate(segment.OfficialSourceUrl, UriKind.Absolute, out var sourceUri)
                && MentionsOfficialAgency(segment.OfficialSourceName)
                && !IsOfficialSourceHost(sourceUri.Host))
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: official source name sounds official but URL host is '{sourceUri.Host}'.");
            }

            if (!segment.HasOfficialSource)
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: missing official source reference.");
            }

            if (segment.HasOfficialSource && string.IsNullOrWhiteSpace(segment.OfficialSourceVerifiedOn))
            {
                warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: official source has no verification date.");
            }

            for (var index = 1; index < segment.Shape.Count; index++)
            {
                if (segment.Shape[index].CumulativeKm < segment.Shape[index - 1].CumulativeKm)
                {
                    warnings.Add($"{segment.SectionCode ?? segment.RouteCode}: shape cumulative km decreases at point '{segment.Shape[index].Name}'.");
                    break;
                }
            }
        }

        return warnings;
    }

    private static bool MentionsOfficialAgency(string sourceName)
        => sourceName.Contains("API Bulgaria", StringComparison.OrdinalIgnoreCase)
           || sourceName.Contains("МРРБ", StringComparison.OrdinalIgnoreCase)
           || sourceName.Contains("government", StringComparison.OrdinalIgnoreCase)
           || sourceName.Contains("official", StringComparison.OrdinalIgnoreCase);

    private static bool IsOfficialSourceHost(string host)
        => host.EndsWith("api.bg", StringComparison.OrdinalIgnoreCase)
           || host.EndsWith("government.bg", StringComparison.OrdinalIgnoreCase)
           || host.EndsWith("gov.bg", StringComparison.OrdinalIgnoreCase)
           || host.EndsWith("egov.bg", StringComparison.OrdinalIgnoreCase)
           || host.EndsWith("europa.eu", StringComparison.OrdinalIgnoreCase);
}
