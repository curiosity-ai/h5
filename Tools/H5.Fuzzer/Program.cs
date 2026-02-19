using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using H5.Fuzzer.Generator;
using H5.Fuzzer.Infrastructure;
using Microsoft.Build.Locator;

namespace H5.Fuzzer
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var minutesOption = new Option<int>(
                "--minutes",
                getDefaultValue: () => 1,
                description: "Duration to run in minutes");

            var seedOption = new Option<int?>(
                "--seed",
                description: "Random seed");

            var outputOption = new Option<string>(
                "--output",
                getDefaultValue: () => "failures",
                description: "Output directory for failures");

            var rootCommand = new RootCommand("H5 Fuzzer")
            {
                minutesOption,
                seedOption,
                outputOption
            };

            rootCommand.SetHandler(async (int minutes, int? seed, string output) =>
            {
                await RunFuzzer(minutes, seed, output);
            }, minutesOption, seedOption, outputOption);

            AssemblyInitialize();

            return await rootCommand.InvokeAsync(args);
        }

        public static void AssemblyInitialize()
        {
            // Initialize MSBuild
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

            try
            {
                // Install Chromium binary
                var ret = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
                if (ret != 0)
                {
                    Console.WriteLine($"Playwright install chromium failed with code {ret}");
                }
                else
                {
                    Console.WriteLine("Playwright chromium installed successfully.");
                }

                // Install dependencies (might fail if not root/sudo, so logging but not throwing)
                ret = Microsoft.Playwright.Program.Main(new[] { "install-deps", "chromium" });
                if (ret != 0)
                {
                   Console.WriteLine($"Playwright install-deps chromium failed with code {ret}");
                }
                else
                {
                   Console.WriteLine("Playwright dependencies installed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error installing Playwright: {ex}");
            }
        }
        static async Task RunFuzzer(int minutes, int? seed, string output)
        {
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            var mainSeed = seed ?? new Random().Next();
            var random = new Random(mainSeed);
            var endTime = DateTime.Now.AddMinutes(minutes);
            
            Console.WriteLine($"Starting Fuzzer with seed {mainSeed} for {minutes} minutes.");
            Console.WriteLine($"Output directory: {Path.GetFullPath(output)}");

            using var playwrightRunner = new PlaywrightRunner();
            await playwrightRunner.InitializeAsync();

            int iterations = 0;
            int failures = 0;

            while (DateTime.Now < endTime)
            {
                iterations++;
                int currentSeed = random.Next();
                Console.Clear();
                Console.WriteLine($"Iteration {iterations}: Generating code with seed {currentSeed}...");

                var generator = new CodeGenerator(currentSeed);
                string code = generator.GenerateProgram();
                
                string roslynOutput = "";
                string h5Output = "";
                string error = null;
                string h5Js = null;
                try
                {
                    // Run Roslyn
                    roslynOutput = await RoslynRunner.CompileAndRunAsync(code);
                    roslynOutput = NormalizeOutput(roslynOutput);
                    
                    Console.WriteLine(code);

                    // Compile H5
                    h5Js = await H5Runner.CompileToJs(code);
                    
                    // Run H5
                    h5Output = await playwrightRunner.RunJsAsync(h5Js, "Program End");
                    h5Output = NormalizeOutput(h5Output);

                    if (roslynOutput != h5Output)
                    {
                         error = $"Output mismatch.\n\nRoslyn:\n---------------\n{roslynOutput}\n---------------\n\nH5:\n---------------\n{h5Output}\n---------------";
                         Console.WriteLine("FAIL: Output mismatch.");

                         try
                         {
                             var minimizer = new Minimizer(playwrightRunner);
                             var minimizedCode = await minimizer.MinimizeAsync(code);

                             if (minimizedCode != code)
                             {
                                 string minFilename = Path.Combine(output, $"fail_{currentSeed}_min.cs");
                                 await File.WriteAllTextAsync(minFilename, minimizedCode);
                                 Console.WriteLine($"Minimized failure saved to {minFilename}");
                             }
                         }
                         catch (Exception minEx)
                         {
                             Console.WriteLine($"Minimization failed: {minEx}");
                         }
                    }
                    else
                    {
                         Console.WriteLine("PASS");
                    }
                }
                catch (Exception ex)
                {
                    error = $"Exception: {ex}";
                    Console.WriteLine($"FAIL: Exception: {ex.Message}");
                }

                if (error != null)
                {
                    failures++;
                    string filename = Path.Combine(output, $"fail_{currentSeed}.cs");
                    string jsFilename = Path.Combine(output, $"fail_{currentSeed}.js");
                    string logFilename = Path.Combine(output, $"fail_{currentSeed}.log");
                    
                    var appJsMarker = "// File: App.js";
                    if (h5Js != null)
                    {
                        var index = h5Js.IndexOf(appJsMarker);
                        if (index >= 0)
                        {
                            var extractedJs = h5Js.Substring(index);
                            await File.WriteAllTextAsync(jsFilename, extractedJs);
                        }
                        else
                        {
                            await File.WriteAllTextAsync(jsFilename, h5Js);
                        }
                    }

                    await File.WriteAllTextAsync(filename, code);
                    await File.WriteAllTextAsync(logFilename, error);
                    
                    Console.WriteLine($"Failure saved to {filename}");
                }
            }

            Console.WriteLine($"Fuzzer finished. {iterations} iterations, {failures} failures.");
        }

        static string NormalizeOutput(string output)
        {
            if (string.IsNullOrEmpty(output)) return "";
            return string.Join("\n", output.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(s => s.TrimEnd())
                .Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}
