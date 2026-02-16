using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using System.Linq;
using System.CommandLine;
using H5.Fuzzer.Infrastructure;
using System.Collections.Generic;

namespace H5.Fuzzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Initialize MSBuild first
            try
            {
                var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                if (instances.Length > 0)
                {
                    // Prefer .NET SDK instance
                    var instance = instances.OrderByDescending(x => x.Version).First();
                    MSBuildLocator.RegisterInstance(instance);
                    Console.WriteLine($"Registered MSBuild instance: {instance.Name} {instance.Version}");
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

            // Ensure Playwright browsers are installed
            await EnsurePlaywrightBrowsersInstalled();

            // Setup CLI
            var rootCommand = new RootCommand("H5 Fuzzer");

            var minutesOption = new Option<int>(
                name: "--minutes",
                description: "How many minutes to run the fuzzer.",
                getDefaultValue: () => 10);

            var seedOption = new Option<int?>(
                name: "--seed",
                description: "Seed for the random number generator. If not provided, a random seed is used.");

            var outputOption = new Option<string>(
                name: "--output",
                description: "Directory to store failure cases.",
                getDefaultValue: () => "failures");

            rootCommand.AddOption(minutesOption);
            rootCommand.AddOption(seedOption);
            rootCommand.AddOption(outputOption);

            rootCommand.SetHandler(RunFuzzerLoop, minutesOption, seedOption, outputOption);

            await rootCommand.InvokeAsync(args);
        }

        static async Task EnsurePlaywrightBrowsersInstalled()
        {
            try
            {
                Console.WriteLine("Ensuring Playwright browsers are installed...");
                var ret = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
                if (ret != 0)
                {
                    Console.WriteLine($"Playwright install chromium failed with code {ret}");
                }
                else
                {
                     Console.WriteLine("Playwright chromium installed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error installing Playwright: {ex}");
            }
        }

        static async Task RunFuzzerLoop(int minutes, int? seedInput, string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            int masterSeed = seedInput ?? new Random().Next();
            var random = new Random(masterSeed);
            Console.WriteLine($"Starting Fuzzer. Duration: {minutes} minutes. Master Seed: {masterSeed}. Output: {outputDir}");

            var endTime = DateTime.UtcNow.AddMinutes(minutes);
            int iterations = 0;
            int failures = 0;

            await using var playwrightRunner = new PlaywrightRunner();
            await playwrightRunner.StartAsync();

            while (DateTime.UtcNow < endTime)
            {
                iterations++;
                int currentSeed = random.Next();
                Console.Write($"[{DateTime.Now:HH:mm:ss}] Iteration {iterations} (Seed: {currentSeed})... ");

                string code = CodeGenerator.Generate(currentSeed);

                try
                {
                    string roslynOutput = "";
                    try
                    {
                        roslynOutput = await RoslynCompiler.CompileAndRunAsync(code);
                        roslynOutput = NormalizeOutput(roslynOutput);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Roslyn Failed!");
                        await SaveFailure(outputDir, currentSeed, code, $"Roslyn Exception: {ex}", "roslyn_crash");
                        failures++;
                        continue;
                    }

                    string h5Output = "";
                    try
                    {
                        // PlaywrightRunner usually needs a full HTML page or the script content.
                        // IntegrationTestBase passes raw JS code to PlaywrightRunner.RunJs which calls EvaluateAsync.
                        // H5Compiler.CompileToJs produces raw JS.
                        string h5Js = await H5Compiler.CompileToJs(code, includeCorePackages: true);
                        h5Output = await playwrightRunner.RunJsAsync(h5Js);
                        h5Output = NormalizeOutput(h5Output);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("H5 Failed!");
                        await SaveFailure(outputDir, currentSeed, code, $"H5/Playwright Exception: {ex}", "h5_crash");
                        failures++;
                        continue;
                    }

                    if (roslynOutput == h5Output)
                    {
                        Console.WriteLine("PASS");
                    }
                    else
                    {
                        Console.WriteLine("FAIL (Output Mismatch)");
                        await SaveFailure(outputDir, currentSeed, code,
                            $"Expected (Roslyn):\n{roslynOutput}\n\nActual (H5):\n{h5Output}", "mismatch");
                        failures++;
                    }
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"Unexpected Error: {ex}");
                }
            }

            Console.WriteLine($"Fuzzer finished. Iterations: {iterations}. Failures: {failures}.");
        }

        static string NormalizeOutput(string output)
        {
            if (string.IsNullOrEmpty(output)) return "";
            return string.Join("\n", output.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.TrimEnd()));
        }

        static async Task SaveFailure(string dir, int seed, string code, string details, string type)
        {
            string filename = Path.Combine(dir, $"{type}_{seed}.cs");
            await File.WriteAllTextAsync(filename, $"/*\n{details}\n*/\n\n{code}");
            Console.WriteLine($"Failure saved to {filename}");
        }
    }
}
