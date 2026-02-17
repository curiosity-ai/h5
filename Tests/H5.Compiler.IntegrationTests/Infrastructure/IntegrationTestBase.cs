using H5.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace H5.Compiler.IntegrationTests
{
    public class IntegrationTestBase
    {
        public IntegrationTestBase()
        {
        }

        protected async Task<string> RunTest(string csharpCode, string? waitForOutput = null, bool skipRoslyn = false, string overrideRoslynCode = null, bool includeCorePackages = false, ModuleLoaderType loaderType = ModuleLoaderType.Global, [System.Runtime.CompilerServices.CallerMemberName] string membName = "", [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            string roslynOutput = "";

            if (!skipRoslyn)
            {
                try
                {
                    roslynOutput = await RoslynCompiler.CompileAndRunAsync(overrideRoslynCode ?? csharpCode);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Roslyn compilation/execution failed:\n{ex}");
                }
            }

            string h5Js = "";
            try
            {
                h5Js = await H5Compiler.CompileToJs(csharpCode, includeCorePackages, loaderType);

                string mocks = "";
                if (loaderType == ModuleLoaderType.AMD)
                {
                    mocks = @"
var definedModules = {};
function define(name, deps, factory) {
    if (typeof name !== 'string') { factory = deps; deps = name; name = null; }
    if (!Array.isArray(deps)) { factory = deps; deps = []; }
    var args = [];
    deps.forEach(function(d) {
        if (d === 'my-lib') {
             args.push({ DoWork: function() { console.log('MyLib.DoWork'); } });
        } else {
             args.push({});
        }
    });
    if (factory) factory.apply(null, args);
}
define.amd = {};
window.require = define;
";
                }
                else if (loaderType == ModuleLoaderType.CommonJS)
                {
                    mocks = @"
window.require = function(name) {
    if (name === 'my-lib') {
        return { DoWork: function() { console.log('MyLib.DoWork'); } };
    }
    console.log('require unknown: ' + name);
    return {};
};
window.module = { exports: {} };
window.global = window;
";
                }
                else
                {
                    // Global loader
                    mocks = @"
window.MyLib = { DoWork: function() { console.log('MyLib.DoWork'); } };
";
                }

                var appJsMarker = "// File: App.js";
                var index = h5Js.IndexOf(appJsMarker);
                if (index >= 0)
                {
                    h5Js = h5Js.Insert(index, mocks);
                }
                else
                {
                    h5Js = mocks + h5Js;
                }

                // Recalculate index after insertion
                index = h5Js.IndexOf(appJsMarker);
                if (index >= 0)
                {
                    var extractedJs = h5Js.Substring(index);
                    var fileName = Path.GetFileNameWithoutExtension(filePath) + "." + membName + ".js";
                    var dumpPath = Path.Combine(Path.GetTempPath(), "H5.Tests.GeneratedJavascript");
                    Directory.CreateDirectory(dumpPath);
                    var tempPath = Path.Combine(dumpPath, fileName);
                    File.WriteAllText(tempPath, extractedJs);
                }
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

        protected async Task RunTestExpectingError(string csharpCode, string expectedErrorMessage, bool includeCorePackages = false)
        {
            try
            {
                await H5Compiler.CompileToJs(csharpCode, includeCorePackages: includeCorePackages);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(expectedErrorMessage) || ex.ToString().Contains(expectedErrorMessage))
                {
                    return;
                }

                Assert.Fail($"Expected error '{expectedErrorMessage}' but got:\n{ex}");
            }

            Assert.Fail("Compilation should have failed but succeeded.");
        }
    }
}
