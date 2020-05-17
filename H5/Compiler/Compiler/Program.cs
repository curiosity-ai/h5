using Cysharp.Text;
using Grpc.Core;
using H5.Compiler.Hosted;
using H5.Contract;
using H5.Translator;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ZLogger;
using System.Threading.Tasks;
using MagicOnion.Hosting;
using MagicOnion.Server;
using System.Diagnostics;
using System.Threading;

namespace H5.Compiler
{

    public class Program
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<Program>();
        
        private const int PORT = 51515;

        private static readonly CancellationTokenSource _exitToken = new CancellationTokenSource();
        private static readonly TaskCompletionSource<object> _exitTask    = new TaskCompletionSource<object>();

        private static async Task<int> Main(string[] args)
        {
            DefaultEncoding.ForceInvariantCultureAndUTF8Output();

            //TODO: get log level from command line
            //TODO: add options on SDK Target to set log level on command line

            ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(l => l.SetMinimumLevel(LogLevel.Information)
                                                                           .AddZLoggerConsole(options => options.PrefixFormatter = (buf, info) => ZString.Utf8Format(buf, "[{0}] [{1}:{2}:{3}] ", GetLogLevelString(info.LogLevel), info.Timestamp.LocalDateTime.Hour, info.Timestamp.LocalDateTime.Minute, info.Timestamp.LocalDateTime.Second))
                                                                           .AddZLoggerLogProcessor(new Logging.InMemoryPerCompilationProvider())));

            Console.CancelKeyPress += (sender, e) =>
            {
                _exitToken.Cancel();
                _exitTask.SetResult(new object());
                e.Cancel = true;
            };

            if (args.Length == 1 && args[0] == "server")
            {
                Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

                return await RunCompilationServerAsync();
            }
            else
            {
                var compilationRequest = ParseRequestFromCommandLine(args);

                if (compilationRequest is null) { ShowHelp(); return 0; }

                var channel = new Channel("localhost", PORT, ChannelCredentials.Insecure);
                var remoteCompiler = new RemoteCompiler(channel, TimeSpan.FromMilliseconds(30_000)); ;

                while (true)
                {
                    try
                    {
                        await remoteCompiler.Ping(_exitToken.Token);
                        break;
                    }
                    catch(Exception E)
                    {
                        Logger.LogInformation("Failed to reach compiler, will start it in the background");
                    }
                    await RestartCompilationServer();
                }

                Logger.LogInformation($"Executing h5 compiler with arguments: '{string.Join(" ", args)}'");

                var compilationUID = await remoteCompiler.RequestCompilationAsync(compilationRequest, _exitToken.Token);

                while(true)
                {
                    var status = await remoteCompiler.GetStatusAsync(compilationUID, _exitToken.Token);
                    
                    if(status.Messages is object)
                    {
                        foreach(var message in status.Messages)
                        {
                            Logger.Log(message.LogLevel, message.Message);
                        }
                    }

                    switch (status.Status)
                    {
                        case CompilationStatus.OnGoing:
                            break;
                        case CompilationStatus.Success:
                            return 0;
                        case CompilationStatus.Fail:
                            return 1;
                        case CompilationStatus.Pending:
                            break;
                    }
                    await Task.Delay(500);
                }
            }
        }

        private static async Task RestartCompilationServer()
        {
            var self = Process.GetCurrentProcess();


            //TODO: fix this, doesnt work (need to check if PID is listening to port to know if it is the other server
            //foreach (var other in Process.GetProcessesByName("h5"))
            //{
            //    if(other.Id != self.Id && other.Arguments == "server")
            //    {
            //        other.Kill();
            //    }
            //}


            var pInfo = new ProcessStartInfo()
            {
                FileName = self.MainModule.FileName,
                UseShellExecute = true,
                CreateNoWindow = false,
                WorkingDirectory = Directory.GetCurrentDirectory(),
            };

            if (self.MainModule.FileName.Contains("dotnet"))
            {
                pInfo.ArgumentList.Add("h5.dll");
            }

            pInfo.ArgumentList.Add("server");

            var process = new Process();
            process.StartInfo = pInfo;
            process.Start();
            await Task.Delay(5000);
        }

        private static async Task<int> RunCompilationServerAsync()
        {
            using (var host = MagicOnionHost.CreateDefaultBuilder()
                                      .UseMagicOnion(new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true),
                                                     new ServerPort("localhost", PORT, ServerCredentials.Insecure))
                                      .Build())
            {
                Logger.LogInformation("==== HOST Starting");
                await host.StartAsync();
                Logger.LogInformation("==== HOST Started Onion Server");
                CompilationProcessor.CompileForever();
                await _exitTask.Task;
                Logger.LogInformation("==== HOST Exit requested");
                await CompilationProcessor.StopAsync();
                Logger.LogInformation("==== HOST Compilation stopped");
                await host.StopAsync();
                Logger.LogInformation("==== HOST Onion Server stopped");
            }
            return 0;
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
        
        /// <summary>
        /// Commandline arguments based on http://docopt.org/
        /// </summary>
        private static void ShowHelp()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string programName = Path.GetFileName(codeBase);

            Console.WriteLine(@"Usage: " + programName + @" [options] (<project-file>|<assembly-file>)
       " + programName + @"

[-h|--help]                     Show this help message.
-r --rebuild                    Force assembly rebuilding.
-c --configuration <name>       Configuration name (Debug/Release etc)   [default: none].
-S --settings <name:value>      Comma-delimited list of project settings
                                I.e -S name1:value1,name2:value2)
                                List of allowed settings:
                                  AssemblyName, CheckForOverflowUnderflow,
                                  Configuration, DefineConstants,
                                  OutputPath, OutDir, OutputType,
                                  RootNamespace
                                options -c, -D have priority over -S
-D --define <const-list>        Semicolon-delimited list of project constants.
");
        }

        private static bool BindCmdArgumentToOption(string arg, CompilationRequest h5Options)
        {
            if (h5Options.ProjectLocation == null)
            {
                if (arg.ToLower().EndsWith(".csproj"))
                {
                    h5Options.ProjectLocation = arg;
                    return true;
                }
            }
            return false; // didn't bind anywhere
        }

        public static CompilationRequest ParseRequestFromCommandLine(string[] args)
        {
            var compilationRequest = new CompilationRequest();

            // options -c, -P and -D have priority over -S
            string configuration = null;
            var hasPriorityConfiguration = false;
            string defineConstants = null;
            var hasPriorityDefineConstants = false;

            int i = 0;

            while (i < args.Length)
            {
                switch (args[i])
                {
                    case "-p":
                    case "--project":
                        compilationRequest.ProjectLocation = args[++i];
                        break;

                    case "--h5":
                        compilationRequest.H5Location = args[++i];
                        break;

                    case "-o":
                    case "--output":
                        compilationRequest.OutputLocation = args[++i];
                        break;

                    case "-c":
                    case "--configuration":
                        configuration = args[++i];
                        hasPriorityConfiguration = true;
                        break;

                    case "-D":
                    case "--define":
                        defineConstants = args[++i];
                        hasPriorityDefineConstants = true;
                        break;

                    case "--rebuild":
                    case "-r":
                        compilationRequest.Rebuild = true;
                        break;

                    case "-S":
                    case "--settings":
                        var error = ParseProjectProperties(compilationRequest, args[++i]);

                        if (error != null)
                        {
                            Logger.LogError("Invalid argument --setting(-S): " + args[i]);
                            Logger.LogError(error);
                            return null;
                        }

                        break;

                    case "-h":
                    case "--help":
                        return null;

                    case "--": // stop reading commandline arguments
                        // Only non-hyphen commandline argument accepted is the file name of the project or
                        // assembly file, so if not provided already, when this option is specified, check if
                        // it is still needed and bind the file to the correct location
                        if (i < (args.Length - 1))
                        {
                            // don't care about success. If not set already, then try next cmdline argument
                            // as the file parameter and ignore following arguments, if any.
                            BindCmdArgumentToOption(args[i + 1], compilationRequest);
                        }
                        i = args.Length; // move to the end of arguments list
                        break;

                    default:

                        // If this argument does not look like a cmdline switch and
                        // neither backwards -project nor -lib were specified
                        if (!BindCmdArgumentToOption(args[i], compilationRequest))
                        {
                            Logger.LogError("Invalid argument: " + args[i]);
                            return null;
                        }
                        break;
                }

                i++;
            }

            if (hasPriorityConfiguration)
            {
                compilationRequest.ProjectProperties.Configuration = configuration;
            }

            if (hasPriorityDefineConstants)
            {
                compilationRequest.ProjectProperties.DefineConstants = defineConstants;
            }

            if (compilationRequest.ProjectLocation == null)
            {
                var folder = Environment.CurrentDirectory;

                var csprojs = new string[] { };

                try
                {
                    csprojs = Directory.GetFiles(folder, "*.csproj", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }

                if (csprojs.Length > 1)
                {
                    Logger.LogError("Could not default to a csproj because multiple were found:");
                    Logger.LogInformation(string.Join(", ", csprojs.Select(path => Path.GetFileName(path))));
                    return null; // error: arguments not provided, so can't guess what to do
                }

                if (csprojs.Length == 0)
                {
                    Logger.LogWarning("Could not default to a csproj because none were found.");
                    Logger.LogError("Error: Project file name must be specified.");
                    return null;
                }

                var csproj = csprojs[0];
                compilationRequest.ProjectLocation = csproj;
                Logger.LogInformation("Defaulting Project Location to " + csproj);
            }

            if (string.IsNullOrEmpty(compilationRequest.OutputLocation))
            {
                compilationRequest.OutputLocation = Path.GetFileNameWithoutExtension(compilationRequest.ProjectLocation);
            }

            if (string.IsNullOrWhiteSpace(compilationRequest.DefaultFileName))
            {
                compilationRequest.DefaultFileName = Path.GetFileName(compilationRequest.OutputLocation);
            }

            return compilationRequest;
        }

        private static string ParseProjectProperties(CompilationRequest request, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                return null;
            }

            if (parameters != null && parameters.Length > 1 && parameters[0] == '"' && parameters.Last() == '"')
            {
                parameters = parameters.Trim('"');
            }

            var settings = new Dictionary<string, string>();

            var splitParameters = parameters.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in splitParameters)
            {
                if (pair == null)
                {
                    continue;
                }

                var parts = pair.Split(new char[] { ':' }, 2);
                if (parts.Length < 2)
                {
                    Logger.LogWarning("Skipped " + pair + " when parsing --settings as it is not well-formed like name:value");
                    continue;
                }

                var name = parts[0].Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Logger.LogWarning("Skipped " + pair + " when parsing --settings as name is empty in name:value");
                    continue;
                }

                string value;

                if (settings.ContainsKey(name))
                {
                    value = settings[name];
                    Logger.LogWarning("Skipped " + pair + " when parsing --settings as it already found in " + name + ":" + value);
                    continue;
                }

                value = parts[1];

                if (value != null && value.Length > 1 && (value[0] == '"' || value.Last() == '"'))
                {
                    value = value.Trim('"');
                }

                settings.Add(name, value);
            }

            try
            {
                request.ProjectProperties.SetValues(settings);
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }

            return null;
        }
    }
}