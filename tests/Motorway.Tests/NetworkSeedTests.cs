using Motorway.Domain;
using Motorway.Engine;
using Motorway.Infrastructure;
using Xunit;

namespace Motorway.Tests;

public sealed class NetworkSeedTests
{
    [Fact]
    public void BuildDefault_ProducesExpectedTopLevelNetworkStats()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var engine = new RouteEngine();
        var stats = engine.CalculateStats(network);

        Assert.Equal(20, stats.SegmentCount);
        Assert.Equal(9, stats.RouteCount);
        Assert.Equal(1506, Math.Round(stats.TotalKm, 0));
        Assert.Equal(991, Math.Round(stats.OpenKm, 0));
        Assert.Equal(250, Math.Round(stats.ConstructionKm, 0));
        Assert.Equal(265, Math.Round(stats.PlannedKm, 0));
        Assert.InRange(stats.CompletionPercent, 65.7, 65.9);
    }

    [Fact]
    public void BuildDefault_HasNoValidatorWarnings()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var warnings = OsmDataValidator.ValidateBulgariaBounds(network)
            .Concat(NetworkConsistencyValidator.Validate(network))
            .ToArray();

        Assert.Empty(warnings);
    }

    [Fact]
    public void HemusEasternSections_FollowExpectedCorridorOrder()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var plannedEast = network.Segments.Single(segment => segment.SectionCode == "A2-04");
        var openEast = network.Segments.Single(segment => segment.SectionCode == "A2-05");

        Assert.Equal("Strazhitsa north corridor", plannedEast.Shape[1].Name);
        Assert.Equal("Buhovtsi / Targovishte", plannedEast.Shape[^1].Name);
        Assert.True(plannedEast.Shape[^1].Latitude > 43.2);
        Assert.True(plannedEast.Shape[^1].Longitude > 26.5);

        Assert.Equal("Kaspichan corridor", openEast.Shape[1].Name);
        Assert.Equal("Belokopitovo interchange", openEast.Shape[2].Name);
        Assert.True(openEast.Shape[2].Longitude > openEast.Shape[1].Longitude);
        Assert.True(openEast.Shape[^1].Longitude > openEast.Shape[2].Longitude);
    }

    [Fact]
    public void EverySegmentShape_HasMonotonicCumulativeDistance()
    {
        var network = NationalNetworkSeed.BuildDefault();

        foreach (var segment in network.Segments)
        {
            for (var index = 1; index < segment.Shape.Count; index++)
            {
                Assert.True(
                    segment.Shape[index].CumulativeKm >= segment.Shape[index - 1].CumulativeKm,
                    $"{segment.SectionCode}: cumulative km decreases at {segment.Shape[index].Name}");
            }
        }
    }

    [Fact]
    public void AnalyzeGeometry_ReportsExpectedDiagnosticsForDefaultSeed()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var engine = new RouteEngine();
        var diagnostics = engine.AnalyzeGeometry(network);

        Assert.Equal(network.Segments.Count, diagnostics.SegmentCount);
        Assert.True(diagnostics.MaxObservedSpanKm > 0);
        Assert.Contains(diagnostics.Segments, item => item.SectionCode == "A2-02" && item.PointCount >= 8);
        Assert.Contains(diagnostics.Segments, item => item.SectionCode == "A2-03" && item.PointCount >= 7);
    }

    [Fact]
    public void AnalyzeContinuity_ReportsRouteTransitionsForDefaultSeed()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var engine = new RouteEngine();
        var diagnostics = engine.AnalyzeContinuity(network);

        Assert.True(diagnostics.RouteCount >= 9);
        Assert.True(diagnostics.TransitionCount > 0);
        Assert.True(diagnostics.MaxEndpointGapKm >= 0);
        Assert.Contains(diagnostics.Transitions, item => item.RouteCode == "A2" && item.FromSectionCode == "A2-04" && item.ToSectionCode == "A2-05");
        Assert.Contains(diagnostics.Transitions, item => item.RouteCode == "A1" && item.FromSectionCode == "A1-04" && item.ToSectionCode == "A1-05");
    }
}
