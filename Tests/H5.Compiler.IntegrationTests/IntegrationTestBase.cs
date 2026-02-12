using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Locator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static bool _msbuildLocatorInitialized;
        private static readonly System.Threading.SemaphoreSlim _lock = new System.Threading.SemaphoreSlim(1, 1);

        public IntegrationTestBase()
        {
            InitializeMSBuild();
        }

        private static void InitializeMSBuild()
        {
            if (_msbuildLocatorInitialized) return;

            try
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                if (instances.Length > 0)
                {
                    var instance = instances.OrderByDescending(x => x.Version).First();
                    MSBuildLocator.RegisterInstance(instance);
                    Console.WriteLine($"Registered MSBuild instance: {instance.Name} {instance.Version} at {instance.MSBuildPath}");
                }
                else
                {
                    MSBuildLocator.RegisterDefaults();
                    Console.WriteLine("Registered default MSBuild instance.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSBuildLocator initialization warning: {ex.Message}");
            }
            finally
            {
                _msbuildLocatorInitialized = true;
            }
        }

        protected async Task RunTest(string csharpCode)
        {
            string roslynOutput = "";

            await _lock.WaitAsync();
            try
            {
                try
                {
                    roslynOutput = RoslynCompiler.CompileAndRun(csharpCode);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Roslyn compilation/execution failed:\n{ex}");
                }
            }
            finally
            {
                _lock.Release();
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
                playwrightOutput = await PlaywrightRunner.RunJs(h5Js);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Playwright execution failed:\n{ex}");
            }

            roslynOutput = NormalizeOutput(roslynOutput);
            playwrightOutput = NormalizeOutput(playwrightOutput);

            Assert.AreEqual(roslynOutput, playwrightOutput, $"Output mismatch.\nExpected (Roslyn):\n{roslynOutput}\nActual (H5/Playwright):\n{playwrightOutput}");
        }

        private string NormalizeOutput(string output)
        {
            if (string.IsNullOrEmpty(output)) return "";
            return string.Join("\n", output.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.TrimEnd()));
        }
    }
}
