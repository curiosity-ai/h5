using System;
using System.Linq;
using System.Threading.Tasks;

namespace H5.Translator.Roslyn.Tests;

/// <summary>
/// Base class for the Roslyn translator integration tests. Mirrors the behavior
/// (and signature) of the existing H5 <c>IntegrationTestBase</c>: it runs the same
/// C# both natively (Roslyn) and as translated JavaScript (Node), then diffs the
/// normalized console output.
/// </summary>
public abstract class TranslatorTestBase
{
    /// <summary>
    /// Compiles + runs the C# natively and as JS, asserting the outputs match.
    /// Signature-compatible with the legacy harness so ported tests need no edits.
    /// </summary>
    /// <param name="csharpCode">The C# to translate + run as JS.</param>
    /// <param name="waitForOutput">Ignored (Node runs the program to completion, incl. async).</param>
    /// <param name="skipRoslyn">Skip native execution / comparison (H5-only behavior check).</param>
    /// <param name="overrideRoslynCode">Alternate source to run natively (when it must differ).</param>
    /// <param name="includeCorePackages">Ignored (the clean-room runtime models no h5.core bindings).</param>
    protected async Task<string> RunTest(
        string csharpCode,
        string? waitForOutput = null,
        bool skipRoslyn = false,
        string? overrideRoslynCode = null,
        bool includeCorePackages = false)
    {
        var translator = new RoslynTranslator();
        var result = translator.Translate(csharpCode);

        if (!result.Success)
        {
            var errors = string.Join("\n", result.Errors.Select(d => d.GetMessage()));
            Assert.Fail($"H5.Translator.Roslyn translation failed:\n{errors}");
        }

        string jsOutput;
        try
        {
            // Prepend the real h5.js runtime + shim; h5.js auto-runs the entry point.
            var full = RoslynTranslator.LoadRuntime() + "\n" + result.Javascript!;
            jsOutput = await NodeJsRunner.RunAsync(full);
        }
        catch (Exception ex)
        {
            Assert.Fail($"JavaScript execution failed:\n{ex}\n\n--- Generated JS ---\n{result.Javascript}");
            throw;
        }

        jsOutput = Normalize(jsOutput);

        if (!skipRoslyn)
        {
            var nativeOutput = Normalize(RoslynNativeRunner.CompileAndRun(overrideRoslynCode ?? csharpCode));
            Assert.AreEqual(nativeOutput, jsOutput,
                $"Output mismatch.\n\nExpected (native Roslyn):\n----\n{nativeOutput}\n----\n\nActual (H5.Translator.Roslyn / JS):\n----\n{jsOutput}\n----\n\n--- Generated JS ---\n{result.Javascript}");
        }

        return jsOutput;
    }

    /// <summary>Asserts that translation reports the given unsupported-feature error.</summary>
    protected Task RunTestExpectingError(string csharpCode, string expectedErrorSubstring, bool includeCorePackages = false)
    {
        var translator = new RoslynTranslator();
        var result = translator.Translate(csharpCode);

        if (result.Success)
        {
            Assert.Fail("Translation should have failed but succeeded.");
        }

        var combined = string.Join("\n", result.Diagnostics.Select(d => d.GetMessage()));
        Assert.IsTrue(combined.Contains(expectedErrorSubstring, StringComparison.OrdinalIgnoreCase),
            $"Expected error containing '{expectedErrorSubstring}' but got:\n{combined}");
        return Task.CompletedTask;
    }

    private static string Normalize(string output)
    {
        if (string.IsNullOrEmpty(output)) return "";
        return string.Join("\n", output.Trim()
            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
            .Select(s => s.TrimEnd()));
    }
}
