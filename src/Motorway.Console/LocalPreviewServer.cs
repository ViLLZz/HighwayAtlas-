using System.Diagnostics;
using System.Net;

public static class LocalPreviewServer
{
    public static async Task RunAsync(string rootDirectory, int port, bool openBrowser, CancellationToken cancellationToken)
    {
        var prefix = $"http://localhost:{port}/";
        var rootPath = Path.GetFullPath(rootDirectory);

        using var listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();

        Console.WriteLine();
        Console.WriteLine($"Local preview: {prefix}");
        Console.WriteLine("Press Ctrl+C to stop the preview server.");

        if (openBrowser)
        {
            TryOpenBrowser(prefix);
        }

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var getContextTask = listener.GetContextAsync();
                var completedTask = await Task.WhenAny(getContextTask, Task.Delay(Timeout.Infinite, cancellationToken));

                if (completedTask != getContextTask)
                {
                    break;
                }

                var context = await getContextTask;
                _ = HandleRequestAsync(context, rootPath, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            if (listener.IsListening)
            {
                listener.Stop();
            }
        }
    }

    private static async Task HandleRequestAsync(HttpListenerContext context, string rootPath, CancellationToken cancellationToken)
    {
        try
        {
            var requestPath = context.Request.Url?.AbsolutePath ?? "/";
            var relativePath = requestPath == "/"
                ? "index.html"
                : Uri.UnescapeDataString(requestPath.TrimStart('/'));

            var candidatePath = Path.GetFullPath(Path.Combine(rootPath, relativePath.Replace('/', Path.DirectorySeparatorChar)));

            if (!candidatePath.StartsWith(rootPath, StringComparison.Ordinal))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.Close();
                return;
            }

            if (!File.Exists(candidatePath))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await using var writer = new StreamWriter(context.Response.OutputStream);
                await writer.WriteAsync("Not found");
                context.Response.Close();
                return;
            }

            context.Response.ContentType = GetContentType(candidatePath);
            var buffer = await File.ReadAllBytesAsync(candidatePath, cancellationToken);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, cancellationToken);
            context.Response.Close();
        }
        catch
        {
            if (context.Response.OutputStream.CanWrite)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Close();
            }
        }
    }

    private static string GetContentType(string filePath) => Path.GetExtension(filePath).ToLowerInvariant() switch
    {
        ".html" => "text/html; charset=utf-8",
        ".css" => "text/css; charset=utf-8",
        ".js" => "application/javascript; charset=utf-8",
        ".json" => "application/json; charset=utf-8",
        ".geojson" => "application/geo+json; charset=utf-8",
        ".svg" => "image/svg+xml",
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        _ => "application/octet-stream"
    };

    private static void TryOpenBrowser(string url)
    {
        try
        {
            if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", url);
                return;
            }

            if (OperatingSystem.IsWindows())
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                return;
            }

            if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", url);
            }
        }
        catch
        {
            Console.WriteLine($"Open the preview manually: {url}");
        }
    }
}