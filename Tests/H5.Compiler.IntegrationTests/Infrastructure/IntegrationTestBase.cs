using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public class IntegrationTestBase
    {
        public IntegrationTestBase()
        {
        }

        protected async Task<string> RunTest(string csharpCode, string? waitForOutput = null, bool skipRoslyn = false)
        {
            string roslynOutput = "";

            if (!skipRoslyn)
            {
                try
                {
                    roslynOutput = await RoslynCompiler.CompileAndRunAsync(csharpCode);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Roslyn compilation/execution failed:\n{ex}");
                }
            }

            string h5Js = "";
            try
            {
                h5Js = await H5Compiler.CompileToJs(csharpCode);
            }
            catch (Exception ex)
            {
                Assert.Fail($"H5 compilation failed:\n{ex}");
            }

            string playwrightOutput = "";
            try
            {
                playwrightOutput = await PlaywrightRunner.RunJs(h5Js, waitForOutput);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Playwright execution failed:\n{ex}");
            }

            playwrightOutput = NormalizeOutput(playwrightOutput);

            if (!skipRoslyn)
            {
                roslynOutput = NormalizeOutput(roslynOutput);
                Assert.AreEqual(roslynOutput, playwrightOutput, $"Output mismatch.\nExpected (Roslyn):\n{roslynOutput}\nActual (H5/Playwright):\n{playwrightOutput}");
            }

            return playwrightOutput;
        }

        private string NormalizeOutput(string output)
        {
            if (string.IsNullOrEmpty(output)) return "";
            return string.Join("\n", output.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.TrimEnd()));
        }
    }
}
