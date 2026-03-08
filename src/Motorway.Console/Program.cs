using System.Text.Json;
using Motorway.Engine;
using Motorway.Infrastructure;

var options = PreviewOptions.Parse(args);

if (options.ShowHelp)
{
    PreviewOptions.PrintHelp();
    return;
}

var network = NationalNetworkSeed.BuildDefault();
var engine = new RouteEngine();
var stats = engine.CalculateStats(network);
var geometryDiagnostics = engine.AnalyzeGeometry(network);
var continuityDiagnostics = engine.AnalyzeContinuity(network);
var provenanceDiagnostics = engine.AnalyzeProvenance(network);
var warnings = OsmDataValidator.ValidateBulgariaBounds(network)
    .Concat(NetworkConsistencyValidator.Validate(network))
    .Distinct(StringComparer.Ordinal)
    .ToArray();
var workspaceRoot = ResolveWorkspaceRoot();
var exportDirectory = Path.Combine(workspaceRoot, "exports", "atlas");

Console.WriteLine("Bulgarian Motorways & Highways Map");
Console.WriteLine(new string('=', 40));
Console.WriteLine($"Routes in atlas:       {stats.RouteCount}");
Console.WriteLine($"Sections in atlas:     {stats.SegmentCount}");
Console.WriteLine($"Total length:          {stats.TotalKm:N0} km");
Console.WriteLine($"Open now:              {stats.OpenKm:N0} km");
Console.WriteLine($"Under construction:    {stats.ConstructionKm:N0} km");
Console.WriteLine($"Still planned:         {stats.PlannedKm:N0} km");
Console.WriteLine($"Network complete:      {stats.CompletionPercent:N1}%");
Console.WriteLine($"Sparse shape data:     {geometryDiagnostics.SparseSegmentCount} sections");
Console.WriteLine($"Sharp bends to review: {geometryDiagnostics.SharpTurnSegmentCount}");
Console.WriteLine($"Largest point gap:     {geometryDiagnostics.MaxObservedSpanKm:N1} km");
Console.WriteLine($"Hand-offs to review:   {continuityDiagnostics.ReviewTransitionCount}");
Console.WriteLine($"Broken hand-offs:      {continuityDiagnostics.BrokenTransitionCount}");
Console.WriteLine($"Largest join gap:      {continuityDiagnostics.MaxEndpointGapKm:N1} km");
Console.WriteLine($"Official-source count: {provenanceDiagnostics.OfficialSourceCount}/{provenanceDiagnostics.SegmentCount}");
Console.WriteLine($"Route-specific refs:   {provenanceDiagnostics.RouteSpecificOfficialCount}");
Console.WriteLine($"Network-level refs:    {provenanceDiagnostics.NetworkWideOfficialCount}");
Console.WriteLine($"Supporting refs:       {provenanceDiagnostics.SecondaryNarrativeCount}");
Console.WriteLine();

if (warnings.Length == 0)
{
    Console.WriteLine("Validation: all mapped points stay inside the Bulgaria bounds check.");
}
else
{
    Console.WriteLine("Validation warnings:");
    foreach (var warning in warnings)
    {
        Console.WriteLine($" - {warning}");
    }
}

Directory.CreateDirectory(exportDirectory);

var geoJsonPath = Path.Combine(exportDirectory, "network.geojson");
var diagnosticsPath = Path.Combine(exportDirectory, "network-diagnostics.json");
var htmlPath = Path.Combine(exportDirectory, "index.html");
var htmlV5Path = Path.Combine(exportDirectory, "bulgarian-motorway-atlas-v5.html");
var html = HtmlAtlasExporter.Export(network);

File.WriteAllText(geoJsonPath, GeoJsonExporter.Export(network));
File.WriteAllText(
    diagnosticsPath,
    JsonSerializer.Serialize(
        new
        {
            generatedAtUtc = DateTime.UtcNow,
            geometry = geometryDiagnostics,
            continuity = continuityDiagnostics,
            provenance = provenanceDiagnostics
        },
        new JsonSerializerOptions { WriteIndented = true }));
File.WriteAllText(htmlPath, html);
File.WriteAllText(htmlV5Path, html);

Console.WriteLine();
Console.WriteLine($"GeoJSON export:        {geoJsonPath}");
Console.WriteLine($"Diagnostics export:    {diagnosticsPath}");
Console.WriteLine($"HTML atlas export:     {htmlPath}");
Console.WriteLine($"HTML atlas (v5 alias): {htmlV5Path}");
Console.WriteLine($"File preview URI:      {new Uri(htmlPath).AbsoluteUri}");

if (!options.Serve)
{
    Console.WriteLine();
    Console.WriteLine("Run with --serve to start a local preview link.");
    return;
}

using var cancellationSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellationSource.Cancel();
};

await LocalPreviewServer.RunAsync(exportDirectory, options.Port, options.OpenBrowser, cancellationSource.Token);

static string ResolveWorkspaceRoot()
{
    var candidates = new[]
    {
        Directory.GetCurrentDirectory(),
        AppContext.BaseDirectory
    }
    .Select(Path.GetFullPath)
    .Distinct(StringComparer.Ordinal);

    foreach (var candidate in candidates)
    {
        var current = new DirectoryInfo(candidate);

        while (current is not null)
        {
            var solutionPath = Path.Combine(current.FullName, "Motorway.sln");
            if (File.Exists(solutionPath))
            {
                return current.FullName;
            }

            current = current.Parent;
        }
    }

    return Directory.GetCurrentDirectory();
}

internal sealed record PreviewOptions(bool Serve, bool OpenBrowser, int Port, bool ShowHelp)
{
    public static PreviewOptions Parse(string[] args)
    {
        var serve = false;
        var openBrowser = false;
        var port = 5057;
        var showHelp = false;

        foreach (var arg in args)
        {
            if (string.Equals(arg, "--serve", StringComparison.OrdinalIgnoreCase))
            {
                serve = true;
                openBrowser = true;
                continue;
            }

            if (string.Equals(arg, "--open", StringComparison.OrdinalIgnoreCase))
            {
                openBrowser = true;
                continue;
            }

            if (string.Equals(arg, "--no-open", StringComparison.OrdinalIgnoreCase))
            {
                openBrowser = false;
                continue;
            }

            if (arg.StartsWith("--port=", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(arg[7..], out var parsedPort)
                && parsedPort is > 0 and < 65536)
            {
                port = parsedPort;
                continue;
            }

            if (string.Equals(arg, "--help", StringComparison.OrdinalIgnoreCase)
                || string.Equals(arg, "-h", StringComparison.OrdinalIgnoreCase))
            {
                showHelp = true;
            }
        }

        return new PreviewOptions(serve, openBrowser, port, showHelp);
    }

    public static void PrintHelp()
    {
        Console.WriteLine("Motorway local preview options");
        Console.WriteLine("  --serve        Start a local HTTP preview server.");
        Console.WriteLine("  --open         Open the preview in the default browser.");
        Console.WriteLine("  --no-open      Keep the browser closed when serving.");
        Console.WriteLine("  --port=5057    Use a custom localhost port.");
        Console.WriteLine("  --help         Show this help text.");
    }
}
