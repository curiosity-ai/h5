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
            SayHi();

            //TODO: get log level from command line
            //TODO: add options on SDK Target to set log level on command line

            ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(l => l.SetMinimumLevel(LogLevel.Information)
                                                                           .AddZLoggerConsole(options => options.PrefixFormatter = (buf, info) => ZString.Utf8Format(buf, "[{0}] [{1}:{2}:{3}] ", GetLogLevelString(info.LogLevel), info.Timestamp.LocalDateTime.Hour, info.Timestamp.LocalDateTime.Minute, info.Timestamp.LocalDateTime.Second))
                                                                           .AddZLoggerLogProcessor(new Logging.InMemoryPerCompilationProvider())));

            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Ctrl+C received");
                if(_exitToken.IsCancellationRequested) //Called twice, so just kill the entire process
                {
                    Environment.Exit(1);
                }
                _exitToken.Cancel();
                _exitTask.SetResult(new object());
                e.Cancel = true;
            };

            // We need the logic for compiler -> startserver -> server because otherwise the server process is marked as a child of the compiler process
            // Which msbuild keeps track of, so it would kill the server

            if(args.Length == 0)
            {
                ShowHelp(); 
                return 0;
            }
            if (args.Length == 1 && args[0] == "server")
            {
                TrySetConsoleTitle();
                Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

                return await RunCompilationServerAsync();
            }
            else if (args.Length == 1 && args[0] == "startserver")
            {
                TrySetConsoleTitle();
                Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

                ActuallyStartCompilationServer();
                return 0;
            }
            else
            {
                var compilationRequest = ParseRequestFromCommandLine(args);

                if (compilationRequest is null) { ShowHelp(); return 0; }

                var channel = new Channel("localhost", PORT, ChannelCredentials.Insecure);
                var remoteCompiler = new RemoteCompiler(channel, TimeSpan.FromMilliseconds(10_000)); ;

                Logger.LogInformation($"Executing h5 compiler with arguments: '{string.Join(" ", args)}'");

                if (compilationRequest.NoCompilationServer)
                {
                    Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
                    return CompilationProcessor.Compile(compilationRequest, default, _exitToken.Token);
                }
                else
                {
                    while (true)
                    {
                        try
                        {
                            await remoteCompiler.Ping(_exitToken.Token);
                            Logger.LogInformation("Found compilation server, sending compilation request\n\n");
                            break;
                        }
                        catch (Exception E)
                        {
                            Logger.LogInformation("Compilation server not online, will start it in the background\n\n");
                        }
                        await TriggerCompilationServerChildProcessStarter();
                    }


                    var compilationUID = await remoteCompiler.RequestCompilationAsync(compilationRequest, _exitToken.Token);

                    while (true)
                    {
                        try
                        {
                            var status = await remoteCompiler.GetStatusAsync(compilationUID, _exitToken.Token);

                            if (status.Messages is object)
                            {
                                foreach (var message in status.Messages)
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
                            await Task.Delay(500, _exitToken.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            //Ignore and send an abort request bellow
                        }

                        if (_exitToken.IsCancellationRequested)
                        {
                            await remoteCompiler.AbortAsync(compilationUID, default);
                            return 0;
                        }
                    }
                }
            }
        }

        private static void TrySetConsoleTitle()
        {
            try
            {
                Console.Title = "H5 Compilation server";
            }
            catch
            {
                //Ignore, will fail in some platforms
            }
        }

        private static void SayHi()
        {
            void Print(string text)
            {
                for(int i = 0; i < text.Length; i++)
                {
                    if (text[i] == 'R')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else if (text[i] == 'Y')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    else if (text[i] == 'W')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.Write(text[i]);
                    }
                }
            }

            try
            {

                Console.ResetColor();

                Print(
@"


    Y    )                                                      
     ( /( (  (       (                           (             
     )\R()Y))\))(      )\           )          (   )\   (   (    
    (R(_)Y\(R(_)Y()\   ((R(_)Y   (     (     `  )  )\ (R(_)Y ))\  )(   
    W _R((W_R)Y(R()Y(R(_)Y  )\W___Y)  Y)\    )\  ' /(/( (R(_)Y W_Y  /(R(W_R)Y(()\  
    W| || | | __|  Y(R(W/ __| Y(R(W_R)W _Y(R(W_R)Y) (R(W_R)W_Y\ R(W_R)W| |R(W_R)W)   (R(W_R) 
    W| __ | |__ \   | (__ / _ \| '  \R()W/ '_ \)| || |/ -_) | '_| 
    |_||_| |___/    \___|\___/|_|_|_| | .__/ |_||_|\___| |_|   
                                      |_|                      


");

                // The above call prints the following:

                //        )                                              
                //     ( /((  (       (                      (           
                //     )\())\))(      )\         )        (  )\  (  (    
                //    ((_)((_)()\   (((_)  (    (    `  ) )\((_)))\ )(   
                //     _((_|()((_)  )\___  )\   )\  '/(/(((_)_ /((_|()\  
                //    | || || __|  ((/ __|((_)_((_))((_)_\(_) (_))  ((_) 
                //    | __ ||__ \   | (__/ _ \ '  \() '_ \) | / -_)| '_| 
                //    |_||_||___/    \___\___/_|_|_|| .__/|_|_\___||_|   
                //                                  |_|                  

                // Alternative:
                //     _  _ ___    ___                _ _         
                //    | || | __|  / __|___ _ __  _ __(_) |___ _ _ 
                //    | __ |__ \ | (__/ _ \ '  \| '_ \ | / -_) '_|
                //    |_||_|___/  \___\___/_|_|_| .__/_|_\___|_|  
                //                              |_|               

                Console.ResetColor();
            }
            catch
            {
                //Ignore
            }
        }

        private static async Task TriggerCompilationServerChildProcessStarter()
        {
            var self = Process.GetCurrentProcess();

            var pInfo = new ProcessStartInfo()
            {
                FileName = self.MainModule.FileName,
                UseShellExecute = true,
                CreateNoWindow = false,
                WorkingDirectory = Directory.GetCurrentDirectory(),
            };

            if (Path.GetFileNameWithoutExtension(self.MainModule.FileName) == "dotnet") //Need to check the file name, as otherwise when running as a tool we're in a folder called /.dotnet/
            {
                pInfo.ArgumentList.Add("h5.dll");
            }

            pInfo.ArgumentList.Add("startserver");

            var process = new Process();
            process.StartInfo = pInfo;
            process.Start();

            //Change this delay to monitoring the process console output for the ready message, or error message    
            await Task.Delay(4000);
        }

        private static void ActuallyStartCompilationServer()
        {
            var self = Process.GetCurrentProcess();

            var pInfo = new ProcessStartInfo()
            {
                FileName = self.MainModule.FileName,
                UseShellExecute = true,
                CreateNoWindow = false,
                WorkingDirectory = Directory.GetCurrentDirectory(),
            };

            if (Path.GetFileNameWithoutExtension(self.MainModule.FileName) == "dotnet") //Need to check the file name, as otherwise when running as a tool we're in a folder called /.dotnet/
            {
                pInfo.ArgumentList.Add("h5.dll");
            }

            pInfo.ArgumentList.Add("server");

            var process = new Process();
            process.StartInfo = pInfo;
            process.Start();
        }

        private static async Task<int> RunCompilationServerAsync()
        {
            using (var host = MagicOnionHost.CreateDefaultBuilder()
                                      .UseMagicOnion(new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true),
                                                     new ServerPort("localhost", PORT, ServerCredentials.Insecure))
                                      .Build())
            {
                Logger.LogInformation("==== HOST Starting");
                try
                {
                    await host.StartAsync();
                }
                catch (Exception E)
                {
                    if(!E.Message.Contains("Failed to bind"))
                    {
                        Console.WriteLine($"Failed to start server: " + E.Message);
                    }
                    return 1;
                }

                Logger.LogInformation("==== HOST Started Onion Server");
                CompilationProcessor.CompileForever(_exitToken.Token);
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

                    case "--no-server":
                        compilationRequest.NoCompilationServer = true;
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

            // TODO: Add more checks
            var isAzure  = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("Build.BuildId")); //From here: https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml
            var isJenkis = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("BUILD_ID"));      //From here: https://wiki.jenkins.io/display/JENKINS/Building+a+software+project

            if (isAzure || isJenkis)
            {
                Logger.LogInformation("Running on build machine, bypassing compilation server");
                compilationRequest.NoCompilationServer = true;
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