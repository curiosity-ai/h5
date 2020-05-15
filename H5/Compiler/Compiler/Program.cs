using H5.Contract;
using H5.Translator;
using H5.Translator.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace H5.Builder
{
    public class Program
    {
        private static int Main(string[] args)
        {
            Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

            var logger = new Logger(null, false, LoggerLevel.Info, true, new ConsoleLoggerWriter(), new FileLoggerWriter());

            logger.Info($"Executing h5 compiler with arguments: '{string.Join(" ", args)}'");

            var h5Options = GetH5OptionsFromCommandLine(args, logger);

            if (h5Options == null)
            {
                ShowHelp(logger);
                return 1;
            }

            if (h5Options.NoCompilation)
            {
                return 0;
            }

            logger.Trace("Command line arguments:");
            logger.Trace("\t" + (string.Join(" ", args) ?? ""));

            var processor = new TranslatorProcessor(h5Options, logger);

            try
            {
                processor.PreProcess();

                processor.Process();

                processor.PostProcess();
            }
            catch (EmitterException ex)
            {
                logger.Error(string.Format("H5.NET Compiler error: {1} ({2}, {3}) {0}", ex.ToString(), ex.FileName, ex.StartLine, ex.StartColumn));
                return 1;
            }
            catch (Exception ex)
            {
                var ee = processor.Translator?.CreateExceptionFromLastNode();

                if (ee != null)
                {
                    logger.Error(string.Format("H5.NET Compiler error: {1} ({2}, {3}) {0}", ee.ToString(), ee.FileName, ee.StartLine, ee.StartColumn));
                }
                else
                {
                    logger.Error(string.Format("H5.NET Compiler error: {0}", ex.ToString()));
                }

                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Commandline arguments based on http://docopt.org/
        /// </summary>
        private static void ShowHelp(ILogger logger)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string programName = Path.GetFileName(codeBase);

            logger.Warn(@"Usage: " + programName + @" [options] (<project-file>|<assembly-file>)
       " + programName + @" [-h|--help]

-h --help                  This help message.
-c --configuration <name>  Configuration name (Debug/Release etc)
                           [default: none].
-P --platform <name>       Platform name (AnyCPU etc) [default: none].
-S --settings <name:value> Comma-delimited list of project settings
                           I.e -S name1:value1,name2:value2)
                           List of allowed settings:
                             AssemblyName, CheckForOverflowUnderflow,
                             Configuration, DefineConstants,
                             OutputPath, OutDir, OutputType,
                             Platform, RootNamespace
                           options -c, -P and -D have priority over -S
-r --rebuild               Force assembly rebuilding.
--nocore                   Do not extract core javascript files.
-D --define <const-list>   Semicolon-delimited list of project constants.
-s --source <file>         Source files name/pattern [default: *.cs].
-f --folder <path>         Builder working directory relative to current WD
                           [default: current wd].
-R --recursive             Recursively search for .cs source files inside
                           current workind directory.
-notimestamp --notimestamp Do not show timestamp in log messages
                           [default: shows timestamp]");

#if DEBUG
            // This code and logic is only compiled in when building H5 in Debug configuration
            logger.Warn(@"-d --debug                 Attach the builder to a visual studio debugging
                           session. Use this to attach the process to an
                           open H5 solution. This option is equivalent
                           to Build.dll's 'AttachDebugger'.");
#endif
        }

        private static bool BindCmdArgumentToOption(string arg, H5Options h5Options, ILogger logger)
        {
            if (h5Options.ProjectLocation == null && h5Options.Lib == null)
            {
                if (arg.ToLower().EndsWith(".csproj"))
                {
                    h5Options.ProjectLocation = arg;
                    return true;
                }
                else if (arg.ToLower().EndsWith(".dll"))
                {
                    h5Options.Lib = arg;
                    return true;
                }
            }
            return false; // didn't bind anywhere
        }

        public static H5Options GetH5OptionsFromCommandLine(string[] args, ILogger logger)
        {
            var h5Options = new H5Options();

            h5Options.Name = "";
            h5Options.ProjectProperties = new ProjectProperties();

            // options -c, -P and -D have priority over -S
            string configuration = null;
            var hasPriorityConfiguration = false;
            string platform = null;
            var hasPriorityPlatform = false;
            string defineConstants = null;
            var hasPriorityDefineConstants = false;

            int i = 0;

            while (i < args.Length)
            {
                switch (args[i])
                {
                    // backwards compatibility -- now is non-switch argument to builder
                    case "-p":
                    case "-project":
                    case "--project":
                        if (h5Options.Lib != null)
                        {
                            logger.Error("Error: Project and assembly file specification is mutually exclusive.");
                            return null;
                        };
                        h5Options.ProjectLocation = args[++i];
                        break;

                    case "-b":
                    case "-h5": // backwards compatibility
                    case "--h5":
                        h5Options.H5Location = args[++i];
                        break;

                    case "-o":
                    case "-output": // backwards compatibility
                    case "--output":
                        h5Options.OutputLocation = args[++i];
                        break;

                    case "-c":
                    case "-cfg": // backwards compatibility
                    case "-configuration": // backwards compatibility
                    case "--configuration":
                        configuration = args[++i];
                        hasPriorityConfiguration = true;
                        break;

                    case "-P":
                    case "--platform":
                        platform = args[++i];
                        hasPriorityPlatform = true;
                        break;

                    case "-def": // backwards compatibility
                    case "-D":
                    case "-define": // backwards compatibility
                    case "--define":
                        defineConstants = args[++i];
                        hasPriorityDefineConstants = true;
                        break;

                    case "-rebuild": // backwards compatibility
                    case "--rebuild":
                    case "-r":
                        h5Options.Rebuild = true;
                        break;

                    case "-nocore": // backwards compatibility
                    case "--nocore":
                        h5Options.ExtractCore = false;
                        break;

                    case "-s":
                    case "-src": // backwards compatibility
                    case "--source":
                        h5Options.Sources = args[++i];
                        break;

                    case "-S":
                    case "--settings":
                        var error = ParseProjectProperties(h5Options, args[++i], logger);

                        if (error != null)
                        {
                            logger.Error("Invalid argument --setting(-S): " + args[i]);
                            logger.Error(error);
                            return null;
                        }

                        break;

                    case "-lib": // backwards compatibility -- now is non-switch argument to builder
                        if (h5Options.ProjectLocation != null)
                        {
                            logger.Error("Error: Project and assembly file specification is mutually exclusive.");
                            return null;
                        }
                        h5Options.Lib = args[++i];
                        break;

                    case "-h":
                    case "--help":
                        ShowHelp(logger);
                        h5Options.NoCompilation = true;
                        return h5Options; // success. Asked for help. Help provided.
                    case "--": // stop reading commandline arguments
                        // Only non-hyphen commandline argument accepted is the file name of the project or
                        // assembly file, so if not provided already, when this option is specified, check if
                        // it is still needed and bind the file to the correct location
                        if (i < (args.Length - 1))
                        {
                            // don't care about success. If not set already, then try next cmdline argument
                            // as the file parameter and ignore following arguments, if any.
                            BindCmdArgumentToOption(args[i + 1], h5Options, logger);
                        }
                        i = args.Length; // move to the end of arguments list
                        break;

                    default:

                        // If this argument does not look like a cmdline switch and
                        // neither backwards -project nor -lib were specified
                        if (!BindCmdArgumentToOption(args[i], h5Options, logger))
                        {
                            logger.Error("Invalid argument: " + args[i]);
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

            if (hasPriorityPlatform)
            {
                h5Options.ProjectProperties.Platform = platform;
            }

            if (hasPriorityDefineConstants)
            {
                h5Options.ProjectProperties.DefineConstants = defineConstants;
            }

            if (h5Options.ProjectLocation == null && h5Options.Lib == null)
            {
                var folder = Environment.CurrentDirectory;

                var csprojs = new string[] { };

                try
                {
                    csprojs = Directory.GetFiles(folder, "*.csproj", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                if (csprojs.Length > 1)
                {
                    logger.Error("Could not default to a csproj because multiple were found:");
                    logger.Info(string.Join(", ", csprojs.Select(path => Path.GetFileName(path))));
                    return null; // error: arguments not provided, so can't guess what to do
                }

                if (csprojs.Length == 0)
                {
                    logger.Warn("Could not default to a csproj because none were found.");
                    logger.Error("Error: Project or assembly file name must be specified.");
                    return null;
                }

                var csproj = csprojs[0];
                h5Options.ProjectLocation = csproj;
                logger.Info("Defaulting Project Location to " + csproj);
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

        private static string ParseProjectProperties(H5Options h5Options, string parameters, ILogger logger)
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
                    logger.Warn("Skipped " + pair + " when parsing --settings as it is not well-formed like name:value");
                    continue;
                }

                var name = parts[0].Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.Warn("Skipped " + pair + " when parsing --settings as name is empty in name:value");
                    continue;
                }

                string value;

                if (settings.ContainsKey(name))
                {
                    value = settings[name];
                    logger.Warn("Skipped " + pair + " when parsing --settings as it already found in " + name + ":" + value);
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