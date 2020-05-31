using Cysharp.Text;
using H5.Compiler;
using H5.Compiler.Hosted;
using H5.Translator;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using NuGet.Versioning;
using System;
using System.Threading.Tasks;
using ZLogger;

namespace Test.Compiler.Service
{
    class Program
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<Program>();

        static async Task Main(string[] args)
        {
            Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
            
            ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(l => l.SetMinimumLevel(LogLevel.Information)
                                                                           .AddZLoggerConsole(options => options.PrefixFormatter = (buf, info) => ZString.Utf8Format(buf, "[{0}] [{1:D2}:{2:D2}:{3:D2}] ", GetLogLevelString(info.LogLevel), info.Timestamp.LocalDateTime.Hour, info.Timestamp.LocalDateTime.Minute, info.Timestamp.LocalDateTime.Second))));

            var settings = new H5DotJson_AssemblySettings();
            var request = new CompilationRequest("App", settings)
                            .NoPackageResources()
                            .NoHTML()
                            .WithPackageReference("h5",      NuGetVersion.Parse("0.0.8537"))
                            .WithPackageReference("h5.Core", NuGetVersion.Parse("0.0.8533"))
                            .WithSourceFile("App.cs",
@"
using System;
using H5;

namespace Test
{
    internal static class App
    {
        private static int HelloWorld;
        private static void Main()
        {
            Console.WriteLine(nameof(HelloWorld));
        }
    }
}
");
            var compiledJavascript = await CompilationProcessor.CompileAsync(request);


            foreach(var (file, code) in compiledJavascript.Output)
            {
                Logger.ZLogInformation("File: {0}\n\n----------------------------\n\n{1}\n\n----------------------------\n\n", file, code);
            }

            await Task.Delay(1000); //Awaits to print all log messages
        }

        internal static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace: return "trce";
                case LogLevel.Debug: return "dbug";
                case LogLevel.Information: return "info";
                case LogLevel.Warning: return "warn";
                case LogLevel.Error: return "fail";
                case LogLevel.Critical: return "crit";
                case LogLevel.None: return "none";
                default: throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }
}
