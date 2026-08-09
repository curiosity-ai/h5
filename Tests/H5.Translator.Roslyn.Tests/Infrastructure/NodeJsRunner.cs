using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace H5.Translator.Roslyn.Tests;

/// <summary>
/// Executes emitted JavaScript on Node.js (V8 — the same engine as Chromium) and
/// captures console output. Node is used instead of a headless browser because the
/// emitted runtime is browser-agnostic (it only touches <c>console.log</c> and
/// <c>globalThis</c>), which keeps the tests fast and deterministic in CI.
/// </summary>
public static class NodeJsRunner
{
    public static async Task<string> RunAsync(string jsCode)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), "h5roslyn_" + Guid.NewGuid().ToString("N") + ".js");
        await File.WriteAllTextAsync(tempFile, jsCode);

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = ResolveNode(),
                Arguments = "\"" + tempFile + "\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = Process.Start(psi)
                ?? throw new InvalidOperationException("Failed to start node.");

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"Node exited with code {process.ExitCode}.\nSTDERR:\n{stderr}\nSTDOUT:\n{stdout}");
            }

            return stdout;
        }
        finally
        {
            try { File.Delete(tempFile); } catch { /* ignore */ }
        }
    }

    private static string ResolveNode()
    {
        // Common locations; fall back to PATH.
        foreach (var candidate in new[] { "/opt/node22/bin/node", "/usr/bin/node", "/usr/local/bin/node" })
        {
            if (File.Exists(candidate)) return candidate;
        }
        return "node";
    }
}
