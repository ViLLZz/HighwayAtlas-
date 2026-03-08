using System.Text.Json;
using Motorway.Infrastructure;
using Xunit;

namespace Motorway.Tests;

public sealed class ExporterSmokeTests
{
    [Fact]
    public void HtmlAtlasExport_ContainsAlphaUiControlsAndHemusData()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var html = HtmlAtlasExporter.Export(network);

        Assert.Contains("selection-clear", html, StringComparison.Ordinal);
        Assert.Contains("language-label", html, StringComparison.Ordinal);
        Assert.Contains("basemap-label", html, StringComparison.Ordinal);
        Assert.Contains("clearSelection", html, StringComparison.Ordinal);
        Assert.Contains("resetView", html, StringComparison.Ordinal);
        Assert.Contains("source-presets-label", html, StringComparison.Ordinal);
        Assert.Contains("sourceQuality", html, StringComparison.Ordinal);
        Assert.Contains("officialRecord", html, StringComparison.Ordinal);
        Assert.Contains("supportingReference", html, StringComparison.Ordinal);
        Assert.Contains("ownershipNotice", html, StringComparison.Ordinal);
        Assert.Contains("Belokopitovo interchange", html, StringComparison.Ordinal);
        Assert.Contains("Strazhitsa north corridor", html, StringComparison.Ordinal);
        Assert.Contains("const ALL_STATUSES", html, StringComparison.Ordinal);
        Assert.Contains("modeledSpan", html, StringComparison.Ordinal);
        Assert.Contains("Span 1", html, StringComparison.Ordinal);
        Assert.DoesNotContain("A1-1", html, StringComparison.Ordinal);
    }

    [Fact]
    public void GeoJsonExport_ProducesAFeaturePerSegment()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var geoJson = GeoJsonExporter.Export(network);
        using var document = JsonDocument.Parse(geoJson);

        var features = document.RootElement.GetProperty("features");

        Assert.Equal(network.Segments.Count, features.GetArrayLength());
        Assert.All(features.EnumerateArray(), feature =>
        {
            Assert.Equal("Feature", feature.GetProperty("type").GetString());
            Assert.Equal("LineString", feature.GetProperty("geometry").GetProperty("type").GetString());
        });
    }

    [Fact]
    public void GeometryDiagnostics_CanBeSerializedForAtlasArtifacts()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var diagnostics = new Motorway.Engine.RouteEngine().AnalyzeGeometry(network);
        var json = JsonSerializer.Serialize(diagnostics);

        Assert.Contains("SparseSegmentCount", json, StringComparison.Ordinal);
        Assert.Contains("A2-02", json, StringComparison.Ordinal);
        Assert.Contains("A2-03", json, StringComparison.Ordinal);
    }

    [Fact]
    public void ContinuityDiagnostics_CanBeSerializedForAtlasArtifacts()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var diagnostics = new Motorway.Engine.RouteEngine().AnalyzeContinuity(network);
        var json = JsonSerializer.Serialize(diagnostics);

        Assert.Contains("TransitionCount", json, StringComparison.Ordinal);
        Assert.Contains("BrokenTransitionCount", json, StringComparison.Ordinal);
        Assert.Contains("A2-04", json, StringComparison.Ordinal);
    }

    [Fact]
    public void ProvenanceDiagnostics_CanBeSerializedForAtlasArtifacts()
    {
        var network = NationalNetworkSeed.BuildDefault();
        var diagnostics = new Motorway.Engine.RouteEngine().AnalyzeProvenance(network);
        var json = JsonSerializer.Serialize(diagnostics);

        Assert.Contains("OfficialSourceCount", json, StringComparison.Ordinal);
        Assert.Contains("NetworkWideOfficialCount", json, StringComparison.Ordinal);
        Assert.Contains("SecondaryNarrativeCount", json, StringComparison.Ordinal);
    }
}
