using Cysharp.Text;
using H5.Contract;
using H5.Translator;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ZLogger;

namespace H5.Compiler
{

    public class Program
    {
        private static ILogger Logger = ApplicationLogging.CreateLogger<Program>();

        private static int Main(string[] args)
        {
            Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();
            DefaultEncoding.ForceInvariantCultureAndUTF8Output();

            //TODO: get log level from command line
            //TODO: add options on SDK Target to set log level on command line

            ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(l => l.SetMinimumLevel(LogLevel.Information).AddZLoggerConsole(options => options.PrefixFormatter = (buf, info) => ZString.Utf8Format(buf, "[{0}] [{1}:{2}:{3}] ", GetLogLevelString(info.LogLevel), info.Timestamp.LocalDateTime.Hour, info.Timestamp.LocalDateTime.Minute, info.Timestamp.LocalDateTime.Second))));

            var h5Options = GetH5OptionsFromCommandLine(args);

            if (h5Options is null)
            {
                ShowHelp();
                return 0;
            }

            Logger.LogInformation($"Executing h5 compiler with arguments: '{string.Join(" ", args)}'");

            var processor = new TranslatorProcessor(h5Options);

            try
            {
                using (new Measure(Logger, "H5 Compilation"))
                {
                    processor.PreProcess();
                    processor.Process();
                    processor.PostProcess();
                }
            }
            catch (EmitterException ex)
            {
                Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ex.ToString(), ex.FileName, ex.StartLine, ex.StartColumn));
                Logger.LogError(ex.StackTrace.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                var ee = processor.Translator?.CreateExceptionFromLastNode();

                if (ee != null)
                {
                    Logger.LogError(string.Format("H5 Compiler error: {1} ({2}, {3}) {0}", ee.ToString(), ee.FileName, ee.StartLine, ee.StartColumn));
                }
                else
                {
                    Logger.LogError(string.Format("H5 Compiler error: {0}", ex.ToString()));
                }

                Logger.LogError(ex.StackTrace.ToString());

                return 1;
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

[-h|--help]                Show this help message.
-r --rebuild               Force assembly rebuilding.
-c --configuration <name>  Configuration name (Debug/Release etc)   [default: none].
-S --settings <name:value> Comma-delimited list of project settings
                           I.e -S name1:value1,name2:value2)
                           List of allowed settings:
                             AssemblyName, CheckForOverflowUnderflow,
                             Configuration, DefineConstants,
                             OutputPath, OutDir, OutputType,
                             RootNamespace
                           options -c, -D have priority over -S
-D --define <const-list>   Semicolon-delimited list of project constants.");
        }

        private static bool BindCmdArgumentToOption(string arg, H5Options h5Options)
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

        public static H5Options GetH5OptionsFromCommandLine(string[] args)
        {
            var h5Options = new H5Options();

            h5Options.ProjectProperties = new ProjectProperties();

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
                        h5Options.ProjectLocation = args[++i];
                        break;

                    case "--h5":
                        h5Options.H5Location = args[++i];
                        break;

                    case "-o":
                    case "--output":
                        h5Options.OutputLocation = args[++i];
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
                        h5Options.Rebuild = true;
                        break;

                    case "-S":
                    case "--settings":
                        var error = ParseProjectProperties(h5Options, args[++i]);

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
                            BindCmdArgumentToOption(args[i + 1], h5Options);
                        }
                        i = args.Length; // move to the end of arguments list
                        break;

                    default:

                        // If this argument does not look like a cmdline switch and
                        // neither backwards -project nor -lib were specified
                        if (!BindCmdArgumentToOption(args[i], h5Options))
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
                h5Options.ProjectProperties.Configuration = configuration;
            }

            if (hasPriorityDefineConstants)
            {
                h5Options.ProjectProperties.DefineConstants = defineConstants;
            }

            if (h5Options.ProjectLocation == null)
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
                    Logger.LogError("Error: Project or assembly file name must be specified.");
                    return null;
                }

                var csproj = csprojs[0];
                h5Options.ProjectLocation = csproj;
                Logger.LogInformation("Defaulting Project Location to " + csproj);
            }

            if (string.IsNullOrEmpty(h5Options.OutputLocation))
            {
                h5Options.OutputLocation = Path.GetFileNameWithoutExtension(h5Options.ProjectLocation);
            }

            h5Options.DefaultFileName = Path.GetFileName(h5Options.OutputLocation);

            if (string.IsNullOrWhiteSpace(h5Options.DefaultFileName))
            {
                h5Options.DefaultFileName = Path.GetFileName(h5Options.OutputLocation);
            }

            return h5Options;
        }

        private static string ParseProjectProperties(H5Options h5Options, string parameters)
        {
            var properties = new ProjectProperties();
            h5Options.ProjectProperties = properties;

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
                properties.SetValues(settings);
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }

            return null;
        }
    }
}